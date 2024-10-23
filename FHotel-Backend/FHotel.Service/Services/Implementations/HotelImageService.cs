using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.HotelImages;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.Interfaces;
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

namespace FHotel.Service.Services.Implementations
{
    public class HotelImageService : IHotelImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelImageResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelImage>().GetAll()
                                            .ProjectTo<HotelImageResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelImageResponse> Get(Guid id)
        {
            try
            {
                HotelImage roomImage = null;
                roomImage = await _unitOfWork.Repository<HotelImage>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelImageId == id)
                    .FirstOrDefaultAsync();

                if (roomImage == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelImage, HotelImageResponse>(roomImage);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelImageResponse> Create(HotelImageRequest request)
        {
            try
            {
                var roomImage = _mapper.Map<HotelImageRequest, HotelImage>(request);
                roomImage.HotelImageId = Guid.NewGuid();
                await _unitOfWork.Repository<HotelImage>().InsertAsync(roomImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelImage, HotelImageResponse>(roomImage);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelImageResponse> Delete(Guid id)
        {
            try
            {
                HotelImage roomImage = null;
                roomImage = _unitOfWork.Repository<HotelImage>()
                    .Find(p => p.HotelImageId == id);
                if (roomImage == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<HotelImage>().HardDeleteGuid(roomImage.HotelImageId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelImage, HotelImageResponse>(roomImage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelImageResponse> Update(Guid id, HotelImageRequest request)
        {
            try
            {
                HotelImage roomImage = _unitOfWork.Repository<HotelImage>()
                            .Find(x => x.HotelImageId == id);
                if (roomImage == null)
                {
                    throw new Exception();
                }
                roomImage = _mapper.Map(request, roomImage);

                await _unitOfWork.Repository<HotelImage>().UpdateDetached(roomImage);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelImage, HotelImageResponse>(roomImage);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<HotelImageResponse>> GetAllHotelImageByHotelId(Guid hotelId)
        {
            var roomImageList = await _unitOfWork.Repository<HotelImage>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelId == hotelId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<HotelImage>, IEnumerable<HotelImageResponse>>(roomImageList);
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
