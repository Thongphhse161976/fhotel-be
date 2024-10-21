using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Hotels;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.Hotels;
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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;

            try
            {
                var hotel = _mapper.Map<HotelCreateRequest, Hotel>(request);
                hotel.HotelId = Guid.NewGuid();
                hotel.CreatedDate = localTime;
                hotel.IsActive = false;
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
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                Hotel hotel = _unitOfWork.Repository<Hotel>()
                            .Find(x => x.HotelId == id);
                if (hotel == null)
                {
                    throw new Exception("Not Found");
                }
                hotel = _mapper.Map(request, hotel);

              

                hotel.UpdatedDate = localTime; // Ensure you update this field automatically
                await _unitOfWork.Repository<Hotel>().UpdateDetached(hotel);
                await _unitOfWork.CommitAsync();

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
    }
}
