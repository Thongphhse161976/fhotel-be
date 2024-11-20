using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Hotels;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.RoomTypes;
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

namespace FHotel.Services.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Hotel>().GetAll()
                                            .ProjectTo<HotelResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelResponse> Get(Guid id)
        {
            try
            {
                Hotel hotel = null;
                hotel = await _unitOfWork.Repository<Hotel>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.District)
                        .ThenInclude(x => x.City)
                     .Include(x => x.Owner)
                    .Where(x => x.HotelId == id)
                    .FirstOrDefaultAsync();

                if (hotel == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelResponse> Create(HotelCreateRequest request)
        {
            // Validate the create request
            var validator = new HotelCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (await _unitOfWork.Repository<Repository.Models.User>().FindAsync(u => u.Email == request.OwnerEmail) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Email", "Email already exists."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);
            DateTime localTime = DateTime.UtcNow + utcOffset;

            try
            {
                var hotel = _mapper.Map<HotelCreateRequest, Hotel>(request);
                hotel.HotelId = Guid.NewGuid();

                // Generate a unique code for the hotel
                hotel.Code = await GenerateUniqueHotelCode();

                hotel.CreatedDate = localTime;
                hotel.IsActive = false;
                hotel.VerifyStatus = "Pending";
                await _unitOfWork.Repository<Hotel>().InsertAsync(hotel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task<string> GenerateUniqueHotelCode()
        {
            // Find the highest existing code number in the database
            var lastHotel = await _unitOfWork.Repository<Hotel>()
                .GetAll()
                .OrderByDescending(h => h.Code)
                .FirstOrDefaultAsync();

            // Extract the numeric part from the last code (e.g., "HTL-01" => 1)
            int lastNumber = 0;
            if (lastHotel != null && int.TryParse(lastHotel.Code.Replace("HTL", ""), out int parsedNumber))
            {
                lastNumber = parsedNumber;
            }

            // Generate the new code with an incremented number
            int newNumber = lastNumber + 1;
            string newCode = $"HTL{newNumber:D2}";

            // Check for uniqueness just in case
            while (await _unitOfWork.Repository<Hotel>().FindAsync(h => h.Code == newCode) != null)
            {
                newNumber++;
                newCode = $"HTL{newNumber:D2}";
            }

            return newCode;
        }

        public async Task<HotelResponse> CreateMore(HotelRequest request)
        {
            // Validate the create request
            var validator = new HotelRequestValidate();
            var validationResult = await validator.ValidateAsync(request);


            if (await _unitOfWork.Repository<Hotel>().FindAsync(u => u.Email == request.Email) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Email", "Email already exists."));
            }
            if (await _unitOfWork.Repository<Hotel>().FindAsync(u => u.Phone == request.Phone) != null)
            {
                validationResult.Errors.Add(new ValidationFailure("Phone", "Phone already exists."));
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
                var ownerhotel = await _unitOfWork.Repository<User>().GetAll()
                     .AsNoTracking()
                     .Where(x => x.UserId == request.OwnerId)
                    .FirstOrDefaultAsync();
                if (ownerhotel == null)
                {
                    throw new Exception("Owner of hotel not found");
                }
                var hotel = _mapper.Map<HotelRequest, Hotel>(request);
                hotel.HotelId = Guid.NewGuid();
                // Generate a unique code for the hotel
                hotel.Code = await GenerateUniqueHotelCode();
                hotel.CreatedDate = localTime;
                hotel.OwnerName = ownerhotel.Name;
                hotel.OwnerPhoneNumber = ownerhotel.PhoneNumber;
                hotel.OwnerEmail = ownerhotel.Email;
                hotel.OwnerIdentificationNumber = ownerhotel.IdentificationNumber;
                hotel.IsActive = false;
                hotel.VerifyStatus = "Pending";
                await _unitOfWork.Repository<Hotel>().InsertAsync(hotel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<HotelResponse> Delete(Guid id)
        {
            try
            {
                Hotel hotel = null;
                hotel = _unitOfWork.Repository<Hotel>()
                    .Find(p => p.HotelId == id);
                if (hotel == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Hotel>().HardDeleteGuid(hotel.HotelId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelResponse> Update(Guid id, HotelUpdateRequest request)
        {
            // Validate the update request
            var validator = new HotelUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);
            DateTime utcNow = DateTime.UtcNow;
            DateTime localTime = utcNow + utcOffset;

            try
            {
                // Retrieve hotel details from database
                Hotel hotel = _unitOfWork.Repository<Hotel>().Find(x => x.HotelId == id);
                if (hotel == null)
                {
                    throw new Exception("Hotel not found");
                }

                // Store the current status for comparison
                var previousStatus = hotel.IsActive;

                // Map updated data to the existing hotel object
                hotel = _mapper.Map(request, hotel);
                hotel.UpdatedDate = localTime;

                await _unitOfWork.Repository<Hotel>().UpdateDetached(hotel);
                await _unitOfWork.CommitAsync();

                // Check if the status has changed and send email notification
                if (previousStatus != hotel.IsActive)
                {
                    var hotelOwner = await _unitOfWork.Repository<User>().FindAsync(x => x.UserId == hotel.OwnerId); // Assuming OwnerId links to the user
                    if (hotelOwner != null)
                    {
                        string subject = hotel.IsActive == true
                            ? "Thông báo kích hoạt khách sạn"
                            : "Thông báo cấm khách sạn";

                        string emailBody = hotel.IsActive == true
                            ? $"Kính gửi {hotelOwner.Name},<br><br>Khách sạn '{hotel.HotelName}' của bạn đã được kích hoạt và hiện đang hoạt động trên hệ thống của chúng tôi.<br>Mọi thắc mắc xin liên hệ qua email sau: companyfhotel@gmail.com<br><br>Trân trọng,<br>FHotel"
                            : $"Kính gửi {hotelOwner.Name},<br><br>Khách sạn '{hotel.HotelName}' của bạn đã bị cấm. Vui lòng liên hệ với bộ phận hỗ trợ để biết thêm thông tin.<br>Mọi thắc mắc xin liên hệ qua email sau: companyfhotel@gmail.com<br><br>Trân trọng,<br>FHotel";

                        await SendEmail(hotelOwner.Email, emailBody, subject);
                    }
                }

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<List<HotelAmenityResponse>> GetHotelAmenityByHotel(Guid id)
        {
            var hotel = await _unitOfWork.Repository<Hotel>().GetAll()
                .Where(x => x.HotelId == id)
                .FirstOrDefaultAsync();
            if (hotel == null)
            {
                return null;
            }
            try
            {
                var hotelAmenities = _unitOfWork.Repository<HotelAmenity>().GetAll()
                    .Where(a => a.HotelId == id)
                    .ProjectTo<HotelAmenityResponse>(_mapper.ConfigurationProvider)
                    .ToList();

                return hotelAmenities;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<HotelResponse>> GetHotelsWithAvailableRoomTypesInRangeAsync(DateTime checkIn, DateTime checkOut)
        {
            // Lấy tất cả các khách sạn cùng loại phòng của họ
            var roomTypes = await _unitOfWork.Repository<RoomType>().GetAll()
                .ProjectTo<RoomTypeResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var hotels = await GetAll();
            var availableHotels = new List<HotelResponse>();
            foreach (var roomType in roomTypes)
            {
                int totalRooms = roomType.TotalRooms ?? 0;

                // Tìm tất cả các đặt phòng trong loại phòng này và có giao với khoảng thời gian check-in và check-out
                var reservations = await _unitOfWork.Repository<Reservation>().GetAll()
                    .Where(r => r.RoomTypeId == roomType.RoomTypeId &&
                                r.CheckInDate <= checkOut && r.CheckOutDate >= checkIn)
                    .ToListAsync();

                // Tính tổng số phòng đang bận trong khoảng thời gian đó
                int reservedRoomsInRange = reservations.Sum(r => r.NumberOfRooms ?? 0);

                // Kiểm tra xem còn phòng trống không
                int availableRoomsInRange = totalRooms - reservedRoomsInRange;

                if (availableRoomsInRange > 0)
                {
                    // Tìm khách sạn chứa loại phòng này trong danh sách kết quả
                    var hotelResponse = hotels
                        .FirstOrDefault(h => h.HotelId == roomType.Hotel.HotelId);

                    if (hotelResponse != null)
                    {

                        availableHotels.Add(hotelResponse);
                    }


                }
            }

            return availableHotels;
        }

        public async Task SendEmail(string toEmail, string body, string subject)
        {
            var emailSettings = GetEmailSettings();

            var fromAddress = new MailAddress(emailSettings.Sender, emailSettings.SystemName);
            var toAddress = new MailAddress(toEmail);

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
                Body = body,
                IsBodyHtml = true
            })
            {
                await smtp.SendMailAsync(message);
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
    }
}
