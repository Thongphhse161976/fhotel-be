using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelAmenities;
using FHotel.Service.Validators.HotelAmenityValidator;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelAmenities;
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
    public class HotelAmenityService : IHotelAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelAmenityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelAmenityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelAmenity>().GetAll()
                                            .ProjectTo<HotelAmenityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelAmenityResponse> Get(Guid id)
        {
            try
            {
                HotelAmenity hotelAmenity = null;
                hotelAmenity = await _unitOfWork.Repository<HotelAmenity>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelAmenityId == id)
                    .FirstOrDefaultAsync();

                if (hotelAmenity == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelAmenityResponse> Create(HotelAmenityCreateRequest request)
        {
            var validator = new HotelAmenityCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var hotelAmenity = _mapper.Map<HotelAmenityCreateRequest, HotelAmenity>(request);
                hotelAmenity.HotelAmenityId = Guid.NewGuid();
             
                await _unitOfWork.Repository<HotelAmenity>().InsertAsync(hotelAmenity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelAmenityResponse> Delete(Guid id)
        {
            try
            {
                HotelAmenity hotelAmenity = null;
                hotelAmenity = _unitOfWork.Repository<HotelAmenity>()
                    .Find(p => p.HotelAmenityId == id);
                if (hotelAmenity == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<HotelAmenity>().HardDeleteGuid(hotelAmenity.HotelAmenityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelAmenityResponse> Update(Guid id, HotelAmenityUpdateRequest request)
        {
            var validator = new HotelAmenityUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            try
            {
                HotelAmenity hotelAmenity = _unitOfWork.Repository<HotelAmenity>()
                            .Find(x => x.HotelAmenityId == id);
                if (hotelAmenity == null)
                {
                    throw new Exception();
                }
                hotelAmenity = _mapper.Map(request, hotelAmenity);
                hotelAmenity.Image = request.Image;
                await _unitOfWork.Repository<HotelAmenity>().UpdateDetached(hotelAmenity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
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
        

    }
}
