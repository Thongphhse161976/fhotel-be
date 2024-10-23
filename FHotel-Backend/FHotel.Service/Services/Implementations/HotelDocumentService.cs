using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Amenities;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.HotelDocuments;
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
    public class HotelDocumentService : IHotelDocumentService
    {
            private readonly IUnitOfWork _unitOfWork;
            private IMapper _mapper;
            public HotelDocumentService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            public async Task<List<HotelDocumentResponse>> GetAll()
            {

                var list = await _unitOfWork.Repository<HotelDocument>().GetAll()
                                                .ProjectTo<HotelDocumentResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();
                return list;
            }

            public async Task<HotelDocumentResponse> Get(Guid id)
            {
                try
                {
                    HotelDocument hotelDocument = null;
                    hotelDocument = await _unitOfWork.Repository<HotelDocument>().GetAll()
                         .AsNoTracking()
                        .Where(x => x.HotelDocumentId == id)
                        .FirstOrDefaultAsync();

                    if (hotelDocument == null)
                    {
                        throw new Exception("HotelDocument not found");
                    }

                    return _mapper.Map<HotelDocument, HotelDocumentResponse>(hotelDocument);
                }

                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            public async Task<HotelDocumentResponse> Create(HotelDocumentRequest request)
            {
                try
                {
                    var hotelDocument = _mapper.Map<HotelDocumentRequest, HotelDocument>(request);
                    hotelDocument.HotelDocumentId = Guid.NewGuid();
                    await _unitOfWork.Repository<HotelDocument>().InsertAsync(hotelDocument);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<HotelDocument, HotelDocumentResponse>(hotelDocument);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            public async Task<HotelDocumentResponse> Delete(Guid id)
            {
                try
                {
                    HotelDocument hotelDocument = null;
                    hotelDocument = _unitOfWork.Repository<HotelDocument>()
                        .Find(p => p.HotelDocumentId == id);
                    if (hotelDocument == null)
                    {
                        throw new Exception("Bi trung id");
                    }
                    await _unitOfWork.Repository<HotelDocument>().HardDeleteGuid(hotelDocument.HotelDocumentId);
                    await _unitOfWork.CommitAsync();
                    return _mapper.Map<HotelDocument, HotelDocumentResponse>(hotelDocument);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public async Task<HotelDocumentResponse> Update(Guid id, HotelDocumentRequest request)
            {
                try
                {
                    HotelDocument hotelDocument = _unitOfWork.Repository<HotelDocument>()
                                .Find(x => x.HotelDocumentId == id);
                    if (hotelDocument == null)
                    {
                        throw new Exception();
                    }
                    hotelDocument = _mapper.Map(request, hotelDocument);

                    await _unitOfWork.Repository<HotelDocument>().UpdateDetached(hotelDocument);
                    await _unitOfWork.CommitAsync();

                    return _mapper.Map<HotelDocument, HotelDocumentResponse>(hotelDocument);
                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        public async Task<IEnumerable<HotelDocumentResponse>> GetAllHotelDocumentByHotelId(Guid hotelId)
        {
            var hotelDocumentList = await _unitOfWork.Repository<HotelDocument>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelId == hotelId)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<HotelDocument>, IEnumerable<HotelDocumentResponse>>(hotelDocumentList);
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
