using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Service.Validators.RoomTypeValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomTypeResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomType>().GetAll()
                                            .ProjectTo<RoomTypeResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomTypeResponse> Get(Guid id)
        {
            try
            {
                RoomType roomType = null;
                roomType = await _unitOfWork.Repository<RoomType>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Hotel)
                    .Where(x => x.RoomTypeId == id)
                    .FirstOrDefaultAsync();

                if (roomType == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomTypeResponse> Create(RoomTypeCreateRequest request)
        {
            // Validate the create request
            var validator = new RoomTypeCreateRequestValidator();
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
                var roomType = _mapper.Map<RoomTypeCreateRequest, RoomType>(request);
                roomType.RoomTypeId = Guid.NewGuid();
                roomType.IsActive = false;
                roomType.CreatedDate = localTime;
                await _unitOfWork.Repository<RoomType>().InsertAsync(roomType);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomTypeResponse> Delete(Guid id)
        {
            try
            {
                RoomType roomType = null;
                roomType = _unitOfWork.Repository<RoomType>()
                    .Find(p => p.RoomTypeId == id);
                if (roomType == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(roomType.RoomTypeId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomTypeResponse> Update(Guid id, RoomTypeUpdateRequest request)
        {
            // Validate the update request
            var validator = new RoomTypeUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            try
            {
                RoomType roomType = _unitOfWork.Repository<RoomType>()
                            .Find(x => x.RoomTypeId == id);
                if (roomType == null)
                {
                    throw new Exception();
                }
                roomType = _mapper.Map(request, roomType);
                // Set the UTC offset for UTC+7
                TimeSpan utcOffset = TimeSpan.FromHours(7);

                // Get the current UTC time
                DateTime utcNow = DateTime.UtcNow;

                // Convert the UTC time to UTC+7
                DateTime localTime = utcNow + utcOffset;
                await _unitOfWork.Repository<RoomType>().UpdateDetached(roomType);
                roomType.UpdatedDate = localTime; // Ensure you update this field automatically
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
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

        public async Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypeByHotelId(Guid hotelId)
        {
            // Fetch the room types from the repository based on hotelId
            var roomTypeList = await _unitOfWork.Repository<RoomType>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelId == hotelId)
                    .ToListAsync();

            // Map the hotel room type to response DTOs
            return _mapper.Map<IEnumerable<RoomType>, IEnumerable<RoomTypeResponse>>(roomTypeList);
        }

    }
}
