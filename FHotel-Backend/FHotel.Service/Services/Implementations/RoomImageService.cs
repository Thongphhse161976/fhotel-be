using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
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
    public class RoomImageService : IRoomImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomImageResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomImage>().GetAll()
                                            .ProjectTo<RoomImageResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomImageResponse> Get(Guid id)
        {
            try
            {
                RoomImage roomImage = null;
                roomImage = await _unitOfWork.Repository<RoomImage>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomImageId == id)
                    .FirstOrDefaultAsync();

                if (roomImage == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomImageResponse> Create(RoomImageRequest request)
        {
            try
            {
                var roomImage = _mapper.Map<RoomImageRequest, RoomImage>(request);
                roomImage.RoomImageId = Guid.NewGuid();
                await _unitOfWork.Repository<RoomImage>().InsertAsync(roomImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomImageResponse> Delete(Guid id)
        {
            try
            {
                RoomImage roomImage = null;
                roomImage = _unitOfWork.Repository<RoomImage>()
                    .Find(p => p.RoomImageId == id);
                if (roomImage == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<RoomImage>().HardDeleteGuid(roomImage.RoomImageId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomImageResponse> Update(Guid id, RoomImageRequest request)
        {
            try
            {
                RoomImage roomImage = _unitOfWork.Repository<RoomImage>()
                            .Find(x => x.RoomImageId == id);
                if (roomImage == null)
                {
                    throw new Exception();
                }
                roomImage = _mapper.Map(request, roomImage);

                await _unitOfWork.Repository<RoomImage>().UpdateDetached(roomImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomImage, RoomImageResponse>(roomImage);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RoomImageResponse>> GetAllRoomImageByRoomTypeId(Guid roomTypeId)
        {
            var roomImageList = await _unitOfWork.Repository<RoomImage>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomTypeId == roomTypeId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<RoomImage>, IEnumerable<RoomImageResponse>>(roomImageList);
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
