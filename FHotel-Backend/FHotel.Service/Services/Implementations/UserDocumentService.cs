using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.UserDocuments;
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
    public class UserDocumentService : IUserDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public UserDocumentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDocumentResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<UserDocument>().GetAll()
                                            .ProjectTo<UserDocumentResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<UserDocumentResponse> Get(Guid id)
        {
            try
            {
                UserDocument userDocument = null;
                userDocument = await _unitOfWork.Repository<UserDocument>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.UserDocumentId == id)
                    .FirstOrDefaultAsync();

                if (userDocument == null)
                {
                    throw new Exception("UserDocument not found");
                }

                return _mapper.Map<UserDocument, UserDocumentResponse>(userDocument);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDocumentResponse> Create(UserDocumentRequest request)
        {
            try
            {
                var userDocument = _mapper.Map<UserDocumentRequest, UserDocument>(request);
                userDocument.UserDocumentId = Guid.NewGuid();
                await _unitOfWork.Repository<UserDocument>().InsertAsync(userDocument);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<UserDocument, UserDocumentResponse>(userDocument);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserDocumentResponse> Delete(Guid id)
        {
            try
            {
                UserDocument userDocument = null;
                userDocument = _unitOfWork.Repository<UserDocument>()
                    .Find(p => p.UserDocumentId == id);
                if (userDocument == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<UserDocument>().HardDeleteGuid(userDocument.UserDocumentId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<UserDocument, UserDocumentResponse>(userDocument);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDocumentResponse> Update(Guid id, UserDocumentRequest request)
        {
            try
            {
                UserDocument userDocument = _unitOfWork.Repository<UserDocument>()
                            .Find(x => x.UserDocumentId == id);
                if (userDocument == null)
                {
                    throw new Exception();
                }
                userDocument = _mapper.Map(request, userDocument);

                await _unitOfWork.Repository<UserDocument>().UpdateDetached(userDocument);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<UserDocument, UserDocumentResponse>(userDocument);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<UserDocumentResponse>> GetAllUserDocumentByReservationId(Guid reservationId)
        {
            var userDocumentList = await _unitOfWork.Repository<UserDocument>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.ReservationId == reservationId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<UserDocument>, IEnumerable<UserDocumentResponse>>(userDocumentList);
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
