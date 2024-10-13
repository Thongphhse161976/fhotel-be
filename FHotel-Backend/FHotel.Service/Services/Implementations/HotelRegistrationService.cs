using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Repository.SMTPs.Models;
using FHotel.Service.DTOs.HotelRegistations;
using FHotel.Service.Validators.HotelResgistrationValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FHotel.Service.DTOs.Users;
using FHotel.Service.Validators.UserValidator;
using FHotel.Services.DTOs.Users;

namespace FHotel.Services.Services.Implementations
{
    public class HotelRegistrationService : IHotelRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IUserService _userService;
        public HotelRegistrationService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<List<HotelRegistrationResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelRegistration>().GetAll()
                                            .ProjectTo<HotelRegistrationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelRegistrationResponse> Get(Guid id)
        {
            try
            {
                HotelRegistration hotelRegistration = null;
                hotelRegistration = await _unitOfWork.Repository<HotelRegistration>().GetAll()
                     .AsNoTracking()
                     .Include(x=> x.Owner)
                    .Where(x => x.HotelRegistrationId == id)
                    .FirstOrDefaultAsync();

                if (hotelRegistration == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelRegistrationResponse> Create(HotelRegistrationCreateRequest request)
        {
            // Validate the create request
            var validator = new HotelRegistrationCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
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
                var hotelRegistration = _mapper.Map<HotelRegistrationCreateRequest, HotelRegistration>(request);
                hotelRegistration.HotelRegistrationId = Guid.NewGuid();
                hotelRegistration.RegistrationDate = localTime;
                hotelRegistration.RegistrationStatus = "Pending";
                await _unitOfWork.Repository<HotelRegistration>().InsertAsync(hotelRegistration);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<HotelRegistrationResponse> Delete(Guid id)
        {
            try
            {
                HotelRegistration hotelRegistration = null;
                hotelRegistration = _unitOfWork.Repository<HotelRegistration>()
                    .Find(p => p.HotelRegistrationId == id);
                if (hotelRegistration == null)
                {
                    throw new Exception("Not found");
                }
                await _unitOfWork.Repository<HotelRegistration>().HardDeleteGuid(hotelRegistration.HotelRegistrationId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }
            catch (Exception eu)
            {
                throw new Exception(eu.Message);
            }
        }

        public async Task<HotelRegistrationResponse> Update(Guid id, HotelRegistrationUpdateRequest request)
        {
            // Validate the update request
            var validator = new HotelRegistrationUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Proceed with hotel registration update
            var hotelRegistration = _unitOfWork.Repository<HotelRegistration>()
                .Find(x => x.HotelRegistrationId == id); // Use '==' for comparison

            if (hotelRegistration == null)
            {
                throw new Exception("Hotel registration not found.");
            }

            // Update fields
            hotelRegistration.OwnerId = request.OwnerId;
            hotelRegistration.NumberOfHotels = request.NumberOfHotels;
            hotelRegistration.Description = request.Description;
            hotelRegistration.RegistrationStatus = request.RegistrationStatus;

            await _unitOfWork.Repository<HotelRegistration>().UpdateDetached(hotelRegistration);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
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

        public async Task SendEmail(string toEmail, UserResponse user)
        {
            // Retrieve email settings from appsettings.json
            var emailSettings = GetEmailSettings();

            var fromAddress = new MailAddress(emailSettings.Sender, emailSettings.SystemName);
            var toAddress = new MailAddress(toEmail);
            const string subject = "Hotel Registration Confirmation"; // Email subject

            // Construct the email body with HTML template
            string body = $@"
        <h1>Hotel Registration Confirmation</h1>
        <p>Dear {user.FirstName},</p>
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

        public async Task ApproveHotelRegistration(string email)
        {
            //Retrieve hotel registration by owner email
            var hotelregistration = await GetByOwnerEmail(email);
            if (hotelregistration == null)
            {
                throw new Exception("Hotel registration not found");
            }
            // Create a hotelUpdateRequest from the HotelRegistrationResponse
            var hotelUpdateRequest = new HotelRegistrationUpdateRequest
            {
                OwnerId = hotelregistration.OwnerId,
            };

            // Validate hotel-registration before Approve
            var validator = new HotelRegistrationUpdateStatusRequestValidator();
            var validationResult = await validator.ValidateAsync(hotelUpdateRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Approve hotel-registration if already approve
            if (hotelregistration.RegistrationStatus == "Approved")
            {
                HotelRegistration  hotelRegistrationUpdate = _unitOfWork.Repository<HotelRegistration>()
                            .Find(x => x.HotelRegistrationId == hotelregistration.HotelRegistrationId);
                await _unitOfWork.Repository<HotelRegistration>().UpdateDetached(hotelRegistrationUpdate);
                await _unitOfWork.CommitAsync(); // Update hotel-registration in the database
                var user = await _userService.Get((Guid)hotelRegistrationUpdate.OwnerId);
                await SendEmail(email, user);
            }
            else
            {
                throw new Exception("Email is sent");
            }
        }
        public async Task<HotelRegistrationResponse> GetByOwnerEmail(String? email)
        {
            try
            {
                HotelRegistration hotelRegistration = null;
                hotelRegistration = await _unitOfWork.Repository<HotelRegistration>().GetAll()
                     .AsNoTracking()
                     .Include(x=> x.Owner)
                    .Where(x => x.Owner.Email == email)
                    .FirstOrDefaultAsync();

                if (hotelRegistration == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
