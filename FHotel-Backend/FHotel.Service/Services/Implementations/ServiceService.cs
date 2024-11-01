using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Services;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Service.Validators.ServiceValidator;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.DTOs.Services;
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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ServiceResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Repository.Models.Service>().GetAll()
                                            .ProjectTo<ServiceResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<ServiceResponse> Get(Guid id)
        {
            try
            {
                Repository.Models.Service service = null;
                service = await _unitOfWork.Repository<Repository.Models.Service>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.ServiceType )
                    .Where(x => x.ServiceId == id)
                    
                    .FirstOrDefaultAsync();

                if (service == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceResponse> Create(ServiceCreateRequest request)
        {
            // Validate the create request
            var validator = new ServiceCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var service = _mapper.Map<ServiceCreateRequest, Repository.Models.Service>(request);
                service.ServiceId = Guid.NewGuid();
                service.IsActive = true;
                await _unitOfWork.Repository<Repository.Models.Service>().InsertAsync(service);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceResponse> Delete(Guid id)
        {
            try
            {
                Repository.Models.Service service = null;
                service = _unitOfWork.Repository<Repository.Models.Service>()
                    .Find(p => p.ServiceId == id);
                if (service == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Repository.Models.Service>().HardDeleteGuid(service.ServiceId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceResponse> Update(Guid id, ServiceUpdateRequest request)
        {
            // Validate the create request
            var validator = new ServiceUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                Repository.Models.Service service = _unitOfWork.Repository<Repository.Models.Service>()
                            .Find(x => x.ServiceId == id);
                if (service == null)
                {
                    throw new Exception();
                }
                service = _mapper.Map(request, service);

                await _unitOfWork.Repository<Repository.Models.Service>().UpdateDetached(service);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
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


        public async Task<List<ServiceResponse>> GetAllServiceByServiceTypeId(Guid id)
        {

            var list = await _unitOfWork.Repository<Repository.Models.Service>().GetAll()
                                            .ProjectTo<ServiceResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.ServiceTypeId == id)
                                            .ToListAsync();
            return list;
        }
    }
}
