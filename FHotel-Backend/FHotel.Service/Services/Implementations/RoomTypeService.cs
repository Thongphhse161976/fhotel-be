using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.FirebaseStorages.Models;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.DTOs.Rooms;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.DTOs.Types;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Service.Validators.RoomTypeValidator;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.Rooms;
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
using Type = FHotel.Repository.Models.Type;

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
                     .Include(x => x.Type)
                    .Where(x => x.RoomTypeId == id)
                    .FirstOrDefaultAsync();

                if (roomType == null)
                {
                    throw new Exception("Room type not found");
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
                // Create the RoomType entity
                var roomType = _mapper.Map<RoomTypeCreateRequest, RoomType>(request);
                roomType.RoomTypeId = Guid.NewGuid();
                roomType.IsActive = false;
                roomType.CreatedDate = localTime;
                roomType.AvailableRooms = request.TotalRooms;

                // Insert the RoomType into the database
                await _unitOfWork.Repository<RoomType>().InsertAsync(roomType);
                await _unitOfWork.CommitAsync();

                // Now create the individual rooms based on the total room count
                for (int i = 0; i < request.TotalRooms; i++)
                {
                    var room = new RoomCreateRequest
                    {
                        RoomNumber = i + 1, // Assign room number starting from 1
                        RoomTypeId = roomType.RoomTypeId,
                        Status = "Available", // Set default status
                        CreatedDate = localTime
                    };

                    // Map and insert the Room into the database
                    var roomEntity = _mapper.Map<RoomCreateRequest, Room>(room);
                    roomEntity.RoomId = Guid.NewGuid();
                    await _unitOfWork.Repository<Room>().InsertAsync(roomEntity);
                }

                // Commit the transaction after creating all rooms
                await _unitOfWork.CommitAsync();

                // Return the created RoomType as a response
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
                await _unitOfWork.Repository<RoomType>().HardDeleteGuid(roomType.RoomTypeId);
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
                     .Include(x => x.Type)
                    .Where(x => x.HotelId == hotelId)
                    .ToListAsync();

            // Map the hotel room type to response DTOs
            return _mapper.Map<IEnumerable<RoomType>, IEnumerable<RoomTypeResponse>>(roomTypeList);
        }

        //public async Task<IEnumerable<HotelResponse>> SearchHotelsWithRoomTypes(List<RoomSearchRequest> searchRequests, string? cityName)
        //{
        //    var roomTypeList = await _unitOfWork.Repository<RoomType>()
        //        .GetAll()
        //        .AsNoTracking()
        //        .Include(rt => rt.Hotel)
        //            .ThenInclude(hotel => hotel.District)
        //                .ThenInclude(district => district.City)
        //        .Include(rt => rt.Type)
        //        .Where(rt => rt.IsActive == true)
        //        .ToListAsync();

        //    // Filter by city if provided
        //    if (!string.IsNullOrEmpty(cityName))
        //    {
        //        roomTypeList = roomTypeList
        //            .Where(rt => rt.Hotel.District.City != null &&
        //                         rt.Hotel.District.City.CityName.Contains(cityName, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    var hotels = new List<HotelResponse>();
        //    var hotelIds = new HashSet<Guid>();

        //    // Group room types by hotel
        //    var hotelRoomTypes = roomTypeList
        //        .GroupBy(rt => rt.Hotel.HotelId) // Group by HotelId to avoid duplicates
        //        .Select(g => new
        //        {
        //            Hotel = g.First().Hotel, // Get the hotel from the first room type
        //            RoomTypes = g.ToList()
        //        })
        //        .ToList();

        //    // Log after grouping
        //    Console.WriteLine($"Grouped Hotels Count After Grouping: {hotelRoomTypes.Count}");
        //    foreach (var hotelGroup in hotelRoomTypes)
        //    {
        //        Console.WriteLine($"Hotel: {hotelGroup.Hotel.HotelName}, Room Types Count: {hotelGroup.RoomTypes.Count}");
        //        foreach (var roomType in hotelGroup.RoomTypes)
        //        {
        //            Console.WriteLine($"- Room Type ID: {roomType.TypeId}, Available: {roomType.AvailableRooms}");
        //        }
        //    }

        //    // Check each hotel against all search requests
        //    foreach (var searchRequest in searchRequests)
        //    {
        //        // Iterate over the hotels
        //        foreach (var hotelGroup in hotelRoomTypes)
        //        {
        //            // Check if the hotel has all required room types with sufficient availability
        //            bool hasAllRequiredRoomTypes = searchRequests.All(sr =>
        //                hotelGroup.RoomTypes.Any(rt => rt.TypeId == sr.TypeId
        //                                                && rt.AvailableRooms >= sr.Quantity));

        //            if (hasAllRequiredRoomTypes)
        //            {
        //                // Directly check HotelId without using Value
        //                if (!hotelIds.Contains(hotelGroup.Hotel.HotelId))
        //                {
        //                    hotels.Add(_mapper.Map<HotelResponse>(hotelGroup.Hotel));
        //                    hotelIds.Add(hotelGroup.Hotel.HotelId); // Directly use HotelId
        //                }
        //            }
        //        }
        //    }

        //    return hotels;
        //}

        public async Task<IEnumerable<HotelResponse>> SearchHotelsWithRoomTypes(List<RoomSearchRequest> searchRequests, string? query)
        {
            var roomTypesQuery = await _unitOfWork.Repository<RoomType>()
                .GetAll()
                .AsNoTracking()
                .Include(rt => rt.Hotel)
                    .ThenInclude(h => h.District)
                        .ThenInclude(d => d.City)
                .Where(rt => rt.IsActive == true)
                .ToListAsync();

            // Combine address, district name, and city name into a single string for each hotel
            var hotelDetails = roomTypesQuery
                .Select(rt => new
                {
                    Hotel = rt.Hotel,
                    CombinedAddress = $"{rt.Hotel.Address} {rt.Hotel.District.DistrictName} {rt.Hotel.District.City.CityName}".Trim()
                })
                .ToList();

            // Apply the single query filter across the combined address if provided
            if (!string.IsNullOrEmpty(query))
            {
                hotelDetails = hotelDetails.Where(h =>
                    h.CombinedAddress.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var hotels = new List<HotelResponse>();
            var hotelIds = new HashSet<Guid>();

            // Group room types by hotel
            var hotelRoomTypes = hotelDetails
                .GroupBy(h => h.Hotel.HotelId) // Group by HotelId to avoid duplicates
                .Select(g => new
                {
                    Hotel = g.First().Hotel, // Get the hotel from the first room type
                    RoomTypes = g.SelectMany(h => roomTypesQuery.Where(rt => rt.Hotel.HotelId == h.Hotel.HotelId)).ToList()
                })
                .ToList();

            // Log after grouping
            Console.WriteLine($"Grouped Hotels Count After Grouping: {hotelRoomTypes.Count}");
            foreach (var hotelGroup in hotelRoomTypes)
            {
                Console.WriteLine($"Hotel: {hotelGroup.Hotel.HotelName}, Room Types Count: {hotelGroup.RoomTypes.Count}");
                foreach (var roomType in hotelGroup.RoomTypes)
                {
                    Console.WriteLine($"- Room Type ID: {roomType.TypeId}, Available: {roomType.AvailableRooms}");
                }
            }

            // Check each hotel against all search requests
            foreach (var searchRequest in searchRequests)
            {
                // Iterate over the hotels
                foreach (var hotelGroup in hotelRoomTypes)
                {
                    // Check if the hotel has all required room types with sufficient availability
                    bool hasAllRequiredRoomTypes = searchRequests.All(sr =>
                        hotelGroup.RoomTypes.Any(rt => rt.TypeId == sr.TypeId
                                                        && rt.AvailableRooms >= sr.Quantity));

                    if (hasAllRequiredRoomTypes)
                    {
                        // Directly check HotelId without using Value
                        if (!hotelIds.Contains(hotelGroup.Hotel.HotelId))
                        {
                            hotels.Add(_mapper.Map<HotelResponse>(hotelGroup.Hotel));
                            hotelIds.Add(hotelGroup.Hotel.HotelId); // Directly use HotelId
                        }
                    }
                }
            }

            return hotels;
        }

        public async Task<List<RoomTypeResponse>> GetAllRoomTypeByStaffId(Guid staffId)
        {
            // Retrieve the HotelID associated with the HotelStaff
            var hotelStaff = await _unitOfWork.Repository<HotelStaff>()
                                              .GetAll()
                                              .Where(hs => hs.UserId == staffId)
                                              .FirstOrDefaultAsync();

            if (hotelStaff == null)
            {
                throw new Exception("Staff not found or not associated with any hotel.");
            }

            var hotelId = hotelStaff.HotelId;

            // Retrieve all reservations for the hotel associated with the staff member
            var roomTypes = await _unitOfWork.Repository<RoomType>()
                                                .GetAll()
                                                .Where(r => r.HotelId == hotelId)
                                                .ProjectTo<RoomTypeResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

            // Check if any roomStayHistories were found
            if (roomTypes == null || !roomTypes.Any())
            {
                throw new Exception("No roomTypes found for this staff's hotel.");
            }

            return roomTypes;
        }

    }
}
