using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Users;
using FHotel.Service.DTOs.Wallets;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.UserValidator;
using FHotel.Services.DTOs.Users;
using FHotel.Services.Services.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using User = FHotel.Repository.Models.User;
using FHotel.Repository.SMTPs.Models;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Repository.SMS;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.DTOs.Reservations;

namespace FHotel.Services.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IWalletService _walletService;
        private readonly ISpeedSMSAPI _smsService;
        private readonly IReservationService _reservationService;
        private readonly InMemoryOtpStore _inMemoryOtpStore;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IRoleService roleService,
            IWalletService walletService, ISpeedSMSAPI smsService, IReservationService reservationService, InMemoryOtpStore inMemoryOtpStore)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleService = roleService;
            _walletService = walletService;
            _smsService = smsService;
            _reservationService = reservationService;
            _inMemoryOtpStore = inMemoryOtpStore;
        }

        public async Task<List<UserResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<User>().GetAll()
                                            .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<UserResponse> Get(Guid id)
        {
            try
            {
                User user = null;
                user = await _unitOfWork.Repository<User>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.UserId == id)
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("Not found");
                }

                return _mapper.Map<User, UserResponse>(user);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> GetByEmail(string email)
        {
            try
            {
                User user = null;
                user = await _unitOfWork.Repository<User>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.Email == email)
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("Not found");
                }

                return _mapper.Map<User, UserResponse>(user);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }





        public async Task<UserResponse> Create(UserCreateRequest request)
        {
            var validator = new UserCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Check dupplicated IdentificationNumber
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.IdentificationNumber == request.IdentificationNumber) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("IdentificationNumber", "Identification number already exists."));
            }

            // Check dupplicated Email
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Email", "Email already exists."));
            }

            // Check dupplicated PhoneNumber
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.PhoneNumber == request.PhoneNumber) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("PhoneNumber", "Phone number already exists."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var role = await _roleService.Get((Guid)request.RoleId);
                var user = _mapper.Map<UserCreateRequest, User>(request);
                user.UserId = Guid.NewGuid();
                user.CreatedDate = localTime;
                await _unitOfWork.Repository<User>().InsertAsync(user);
                await _unitOfWork.CommitAsync();
                if (user.UserId != Guid.Empty)
                {
                    var wallet = new WalletRequest
                    {
                        UserId = user.UserId,
                    };
                    await _walletService.Create(wallet);
                }
                if(role.RoleName == "Hotel Manager")
                {
                    await SendEmail(user.Email, user);
                }
                if (role.RoleName == "Receptionist")
                {
                    await SendEmail(user.Email, user);
                }
                if (role.RoleName == "Room Attendant")
                {
                    await SendEmail(user.Email, user);
                }
                return _mapper.Map<User, UserResponse>(user);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> Delete(Guid id)
        {
            try
            {
                User user = null;
                user = _unitOfWork.Repository<User>()
                    .Find(p => p.UserId == id);
                if (user == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<User>().HardDeleteGuid(user.UserId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<User, UserResponse>(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserResponse> Update(Guid id, UserUpdateRequest request)
        {
            var validator = new UserUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            User user = _unitOfWork.Repository<User>()
                            .Find(x => x.UserId == id);
            if (user == null)
            {
                var validationErrors = new List<ValidationFailure>
                    {
                        new ValidationFailure("User", "User not found.")
                    };
                throw new ValidationException(validationErrors);
            }

            // Check for duplicate IdentificationNumber (excluding current user)
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.IdentificationNumber == request.IdentificationNumber && u.UserId != id) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("IdentificationNumber", "Identification number already exists."));
            }

            // Check for duplicate Email (excluding current user)
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email && u.UserId != id) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Email", "Email already exists."));
            }

            // Check for duplicate PhoneNumber (excluding current user, if required)
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.PhoneNumber == request.PhoneNumber && u.UserId != id) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("PhoneNumber", "Phone number already exists."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                await _roleService.Get((Guid)request.RoleId);
                user.UpdatedDate = localTime;
                user = _mapper.Map(request, user);

                await _unitOfWork.Repository<User>().UpdateDetached(user);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<User, UserResponse>(user);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserResponse> Login(UserLoginRequest request)
        {
            // Validate the login request using FluentValidation
            var validator = new UserLoginRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If validation fails, throw a validation exception with the validation failures
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Retrieve the user based on email and password
            var user = _unitOfWork.Repository<User>()
                .Find(x => x.Email == request.Email && x.Password == request.Password);

            // Return null if the user is not found or if credentials are incorrect
            if (user == null)
            {
                return null;
            }

            // Return null if the user's email or password is null (additional precaution)
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            // Retrieve additional user details
            var userRes = await Get(user.UserId);

            return userRes;
        }

      

        public async Task<string> UploadImage(IFormFile file)
        {
            string link = "";

            if (file != null && file.Length > 0)
            {
                // Get Firebase configuration
                var firebaseProps = GetFirebaseStorageProperties();

                // Authenticate Firebase
                var auth = new FirebaseAuthProvider(new FirebaseConfig(firebaseProps.ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(firebaseProps.AuthEmail, firebaseProps.AuthPassword);

                var cancellation = new CancellationTokenSource();
                var fileName = file.FileName;
                var stream = file.OpenReadStream();

                // Upload file to Firebase Storage
                var task = new FirebaseStorage(
                    firebaseProps.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                )
                .Child("images")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);

                try
                {
                    // Get the download link after upload
                    link = await task;
                    Debug.WriteLine($"File uploaded: {link}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during file upload: {ex.Message}");
                    // You can also log the exception or handle it accordingly
                }
                finally
                {
                    stream.Close(); // Ensure stream is closed properly
                }
            }

            return link;
        }

        private FirebaseStorageModel GetFirebaseStorageProperties()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                  .SetBasePath(Directory.GetCurrentDirectory())
                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            return new FirebaseStorageModel()
            {
                ApiKey = configuration.GetSection("FirebaseStorage:apiKey").Value,
                Bucket = configuration.GetSection("FirebaseStorage:bucket").Value,
                AuthEmail = configuration.GetSection("FirebaseStorage:authEmail").Value,
                AuthPassword = configuration.GetSection("FirebaseStorage:authPassword").Value
            };
        }

        public async Task<UserResponse> Register(UserCreateRequest request)
        {
            var validator = new UserRegisterRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Check dupplicated IdentificationNumber
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.IdentificationNumber == request.IdentificationNumber) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("IdentificationNumber", "Identification number already exists."));
            }

            // Check dupplicated Email
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.Email == request.Email) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Email", "Email already exists."));
            }

            // Check dupplicated PhoneNumber
            if (await _unitOfWork.Repository<User>().FindAsync(u => u.PhoneNumber == request.PhoneNumber) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("PhoneNumber", "Phone number already exists."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var user = _mapper.Map<UserCreateRequest, User>(request);
                user.UserId = Guid.NewGuid();
                user.CreatedDate = localTime;
                user.IsActive = false;
                user.RoleId = await _roleService.GetRoleIdByName("Customer");
                await _unitOfWork.Repository<User>().InsertAsync(user);
                await _unitOfWork.CommitAsync();
                if (user.UserId != Guid.Empty)
                {
                    var wallet = new WalletRequest
                    {
                        UserId = user.UserId,
                    };
                    await _walletService.Create(wallet);
                    string otpCode = GenerateOTP();
                    _smsService.SendOTP(request.PhoneNumber, otpCode);
                    _inMemoryOtpStore.StoreOTP(request.PhoneNumber, otpCode, TimeSpan.FromMinutes(5));
                }
                
                return _mapper.Map<User, UserResponse>(user);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task VerifyOTP(string phoneNumber, string otpCode)
        {
            bool isOtpValid = _inMemoryOtpStore.ValidateOTP(phoneNumber, otpCode);
            if (!isOtpValid)
            {
                throw new Exception("Invalid OTP. Please try again.");
            }

            var user = await _unitOfWork.Repository<User>().FindAsync(u => u.PhoneNumber == phoneNumber);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (user.IsActive != true)
            {
                user.IsActive = true;

                await _unitOfWork.Repository<User>().UpdateDetached(user);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("Account already activated");
            }
        }
        public async Task SendActivationEmail(string toEmail)
        {
            // Retrieve email settings from appsettings.json
            var emailSettings = GetEmailSettings();

            var fromAddress = new MailAddress(emailSettings.Sender, emailSettings.SystemName);
            var toAddress = new MailAddress(toEmail);
            const string subject = "Account Activation"; // Email subject

            // Construct the activation link with email as a query parameter
            string activationLink = $"https://fhotelapi.azurewebsites.net/api/authentications/activate?email={toEmail}";
            string body = $"Please activate your account by clicking the following link: {activationLink}";

            var smtp = new SmtpClient
            {
                Host = emailSettings.Host,
                Port = emailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, emailSettings.Password)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }



        public async Task ActivateUser(string email)
        {
            // Retrieve the user by email
            var user = await GetByEmail(email);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Create a UserUpdateRequest from the UserResponse
            var updateRequest = new UserUpdateRequest
            {
                Email = user.Email,
                // Populate other fields if necessary
                // Example: PhoneNumber = user.PhoneNumber
                // Add any other necessary properties that your UserUpdateRequest has
            };

            // Validate the UserUpdateRequest before activation
            var validator = new UserUpdateStatusRequestValidator();
            var validationResult = await validator.ValidateAsync(updateRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Activate the user if not already active
            if (user.IsActive != true)
            {
                user.IsActive = true; // Activate the user
                User userUpdate = _unitOfWork.Repository<User>()
                            .Find(x => x.UserId == user.UserId);
                userUpdate.IsActive = user.IsActive;
                await _unitOfWork.Repository<User>().UpdateDetached(userUpdate);
                await _unitOfWork.CommitAsync(); // Update the user in the database
            }
            else
            {
                throw new Exception("Account already activated");
            }
        }


        private Email GetEmailSettings()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();

            return new Email()
            {
                SystemName = configuration.GetSection("Email:SystemName").Value,
                Sender = configuration.GetSection("Email:Sender").Value,
                Password = configuration.GetSection("Email:Password").Value,
                Port = int.Parse(configuration.GetSection("Email:Port").Value),
                Host = configuration.GetSection("Email:Host").Value
            };
        }

        public async Task<List<HotelResponse>> GetHotelByUser(Guid id)
        {
            var user = await _unitOfWork.Repository<User>().GetAll()
                .Where(x => x.UserId == id)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            try
            {
                var hotel = _unitOfWork.Repository<Hotel>().GetAll()
                    .Where(a => a.OwnerId == id)
                    .ProjectTo<HotelResponse>(_mapper.ConfigurationProvider)
                    .ToList();

                return hotel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SendEmail(string toEmail, User user)
        {
            // Retrieve email settings from appsettings.json
            var emailSettings = GetEmailSettings();

            var fromAddress = new MailAddress(emailSettings.Sender, emailSettings.SystemName);
            var toAddress = new MailAddress(toEmail);
            const string subject = "Hotel Registration Confirmation"; // Email subject

            // Construct the email body with HTML template
            string body = $@"
        <h1>Hotel Registration Confirmation</h1>
        <p>Dear {user.Name},</p>
        <p>Thanks for giving time with us.</p>
        <p>You now can access our system FHotel</p>
        <p>Email: {user.Email}</p>
        <p>Password: {user.Password}</p>     
        <p>Best regards,<br>FHotel company.</p>";

            // Set up the SMTP client
            var smtp = new SmtpClient
            {
                Host = emailSettings.Host,
                Port = emailSettings.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, emailSettings.Password)
            };

            // Configure and send the email
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Specify that the email body is HTML
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
        public async Task SendSMS(string phonenumber)
        {
            string otpCode = GenerateOTP();
            var hello =_smsService.SendOTP(phonenumber, otpCode);
            Console.WriteLine(hello);
        }

        public string GenerateOTP(int length = 5)
        {
            var random = new Random();
            var otpCode = new string(Enumerable.Repeat("0123456789", length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return otpCode;
        }

        public async Task<List<UserResponse>> GetAllCustomerByStaffId(Guid staffId)
        {
            // Retrieve the HotelID associated with the HotelStaff
            var hotelStaff = await _unitOfWork.Repository<HotelStaff>()
                                               .GetAll()
                                               .FirstOrDefaultAsync(hs => hs.UserId == staffId);

            if (hotelStaff == null)
            {
                throw new Exception("Staff not found or not associated with any hotel.");
            }

            var hotelId = hotelStaff.HotelId;

            // Retrieve all reservations for the hotel associated with the staff member
            var reservations = await _reservationService
                                                 .GetAll(); // Ensure this is awaited and converts to a list

            // Apply the Where clause after awaiting the task
            var filteredReservations = reservations
                                        .Where(r => r.RoomType.HotelId == hotelId)
                                        .ToList(); // Convert to list if necessary

            // Check if any reservations were found
            if (!filteredReservations.Any())
            {
                throw new Exception("No reservations found for this staff's hotel.");
            }

            // Extract customer IDs from filtered reservations and retrieve customer details
            var customerIds = filteredReservations.Select(r => r.CustomerId).Distinct().ToList(); // Ensure unique customer IDs
            var customers = await _unitOfWork.Repository<User>()
                                               .GetAll()
                                               .Where(c => customerIds.Contains(c.UserId))
                                               .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                               .ToListAsync();

            // Return the list of customer responses
            return customers;
        }

        public async Task<List<UserResponse>> GetAllCustomerByOwnerId(Guid ownerId)
        {
           

            // Retrieve all reservations for the hotel associated with the staff member
            var reservations = await _reservationService
                                                 .GetAll(); // Ensure this is awaited and converts to a list

            // Apply the Where clause after awaiting the task
            var filteredReservations = reservations
                                        .Where(r => r.RoomType.Hotel.OwnerId == ownerId)
                                        .ToList(); // Convert to list if necessary

            // Check if any reservations were found
            if (!filteredReservations.Any())
            {
                throw new Exception("No reservations found for this owner.");
            }

            // Extract customer IDs from filtered reservations and retrieve customer details
            var customerIds = filteredReservations.Select(r => r.CustomerId).Distinct().ToList(); // Ensure unique customer IDs
            var customers = await _unitOfWork.Repository<User>()
                                               .GetAll()
                                               .Where(c => customerIds.Contains(c.UserId))
                                               .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                               .ToListAsync();

            // Return the list of customer responses
            return customers;
        }


    }

}
