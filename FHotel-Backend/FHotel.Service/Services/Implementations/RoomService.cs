using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.Validators.ReservationValidator;
using FHotel.Service.Validators.RoomValidator;
using FHotel.Service.Validators.TypePricingValidator;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        public async Task<List<RoomResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Room>().GetAll()
                                            .ProjectTo<RoomResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomResponse> Get(Guid id)
        {
            try
            {
                Room room = null;
                room = await _unitOfWork.Repository<Room>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.RoomType)
                        .ThenInclude(x => x.Type)
                    .Where(x => x.RoomId == id)
                    .FirstOrDefaultAsync();

                if (room == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Room, RoomResponse>(room);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomResponse> Create(RoomRequest request)
        {
            request.Status = "Available";
            request.IsCleaned = true;
            var validator = new RoomRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            // Get the total number of room for this room type

            var roomtype = await _unitOfWork.Repository<RoomType>()
                        .AsNoTracking()
                        .Where(x => x.RoomTypeId == request.RoomTypeId)
                        .FirstOrDefaultAsync();

            if (roomtype == null)
            {
                validationResult.Errors.Add(new ValidationFailure("RoomType", "Room type not found"));
            }
            var existingRooms = await _unitOfWork.Repository<Room>()
                                        .AsNoTracking()
                                        .Where(x => x.RoomTypeId == request.RoomTypeId)
                                        .CountAsync();

            // Check if the number of Rooms has reached the number of rooms in room type
            if (existingRooms >= roomtype.TotalRooms)
            {
                validationResult.Errors.Add(new ValidationFailure("TotalRooms", "No more Room can be added."));
            }
            var _roomTypeService = _serviceProvider.GetService<IRoomTypeService>();
            var roomTypeResponse = await _roomTypeService.Get(request.RoomTypeId.Value);

            // Use GetAll with a LINQ filter to check for duplicates
            var existingRoomNumber = (await GetAll())
                .Where(u => u.RoomType.HotelId == roomTypeResponse.HotelId &&
                            u.RoomNumber == request.RoomNumber)
                .ToList();


            if (existingRoomNumber.Any())
            {
                validationResult.Errors.Add(new ValidationFailure("Room Number", "Số phòng đã tồn tại trong khách sạn!"));
                await _roomTypeService.Delete(request.RoomTypeId.Value);
            }

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var room = _mapper.Map<RoomRequest, Room>(request);
                room.RoomId = Guid.NewGuid();
                room.CreatedDate = localTime;
                await _unitOfWork.Repository<Room>().InsertAsync(room);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> CheckDuplicateRoomNumbers(List<int> roomNumbers, Guid hotelId)
        {
            if (roomNumbers == null || roomNumbers.Count == 0)
            {
                // If the list of room numbers is empty or null, no duplicates exist
                return false;
            }

            // Convert the roomNumbers list to a HashSet for faster lookups
            var roomNumbersSet = new HashSet<int>(roomNumbers);

            // Query the database to check if any of the room numbers are already taken in the hotel
            var duplicateRooms = await _unitOfWork.Repository<Room>()
                .AsNoTracking()
                .Where(r => roomNumbersSet.Contains(r.RoomNumber.Value) && r.RoomType.HotelId == hotelId)
                .AnyAsync(); // Use AnyAsync to check if there are any duplicates

            return duplicateRooms; // Return true if duplicates exist, false if not
        }



        public async Task<RoomResponse> Delete(Guid id)
        {
            try
            {
                Room room = null;
                room = _unitOfWork.Repository<Room>()
                    .Find(p => p.RoomId == id);
                if (room == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Room>().HardDeleteGuid(room.RoomId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public async Task<RoomResponse> Update2(Guid id, RoomRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;

            try
            {
                // Find the room to update
                Room room = _unitOfWork.Repository<Room>()
                            .Find(x => x.RoomId == id);
                if (room == null)
                {
                    throw new KeyNotFoundException("Room not found.");
                }


                // Map the update request to the room entity
                room = _mapper.Map(request, room);
                room.UpdatedDate = localTime;

                // Update the room
                await _unitOfWork.Repository<Room>().UpdateDetached(room);
                await _unitOfWork.CommitAsync();

                // If the status has changed, update RoomType availability
                //if (request.Status == "Occupied")
                //{
                //    var _roomTypeService = _serviceProvider.GetService<IRoomTypeService>();
                //    var roomType = await _roomTypeService.Get((Guid)request.RoomTypeId);
                //    var updateRoomType = new RoomTypeUpdateRequest
                //    {
                //        RoomTypeId = roomType.RoomTypeId,
                //        AvailableRooms = roomType.AvailableRooms - 1,
                //        Description = roomType.Description,
                //        HotelId = roomType.HotelId,
                //        IsActive = roomType.IsActive,
                //        Note = roomType.Note,
                //        RoomSize = roomType.RoomSize,
                //        TotalRooms = roomType.TotalRooms,
                //        TypeId = roomType.TypeId,
                //        UpdatedDate = roomType.UpdatedDate
                //    };
                //    await _roomTypeService.Update(roomType.RoomTypeId, updateRoomType);
                //}

                // Return the updated room
                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception ex)
            {
                // Log error (if logging is available)
                throw new ApplicationException($"Error updating room: {ex.Message}", ex);
            }
        }
        //room attendatn update room
        public async Task<RoomResponse> Update(Guid id, RoomRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            //var validator = new RoomRequestValidator();
            //var validationResult = await validator.ValidateAsync(request);
            //var roomResponse = await Get(request.RoomId);

            //// Use GetAll with a LINQ filter to check for duplicates
            //var existingRoomNumber = (await GetAll())
            //    .Where(u => u.RoomType.HotelId == roomResponse.RoomType.HotelId &&
            //                u.RoomNumber == request.RoomNumber)
            //    .ToList();


            //if (existingRoomNumber.Any())
            //{
            //    validationResult.Errors.Add(new ValidationFailure("Room Number", "Số phòng đã tồn tại trong khách sạn!"));
            //}

            //// If there are any validation errors, throw a ValidationException
            //if (validationResult.Errors.Any())
            //{
            //    throw new ValidationException(validationResult.Errors);
            //}

            try
            {
                // Find the room to update
                Room room = _unitOfWork.Repository<Room>()
                            .Find(x => x.RoomId == id);
                if (room == null)
                {
                    throw new KeyNotFoundException("Room not found.");
                }

                // Store the previous status
                string previousStatus = room.Status;

                // Map the update request to the room entity
                room = _mapper.Map(request, room);
                room.UpdatedDate = localTime;

                // Update the room
                await _unitOfWork.Repository<Room>().UpdateDetached(room);
                await _unitOfWork.CommitAsync();

                // If the status has changed, update RoomType availability
                if (previousStatus != request.Status)
                {
                    // Call the helper method to update RoomType availability based on the room's status
                    await UpdateRoomTypeAvailability(room.RoomTypeId.Value, request.Status, previousStatus);
                }

                // Return the updated room
                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception ex)
            {
                // Log error (if logging is available)
                throw new ApplicationException($"Error updating room: {ex.Message}", ex);
            }
        }

        public async Task<RoomResponse> Update3(Guid id, RoomRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            var validator = new RoomRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            var roomResponse = await Get(request.RoomId);

            // Use GetAll with a LINQ filter to check for duplicates
            var existingRoomNumber = (await GetAll())
                .Where(u => u.RoomType.HotelId == roomResponse.RoomType.HotelId &&
                            u.RoomNumber == request.RoomNumber)
                .ToList();


            if (existingRoomNumber.Any())
            {
                validationResult.Errors.Add(new ValidationFailure("Room Number", "Số phòng đã tồn tại trong khách sạn!"));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                // Find the room to update
                Room room = _unitOfWork.Repository<Room>()
                            .Find(x => x.RoomId == id);
                if (room == null)
                {
                    throw new KeyNotFoundException("Room not found.");
                }

                // Map the update request to the room entity
                room = _mapper.Map(request, room);
                room.UpdatedDate = localTime;

                // Update the room
                await _unitOfWork.Repository<Room>().UpdateDetached(room);
                await _unitOfWork.CommitAsync();


                // Return the updated room
                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception ex)
            {
                // Log error (if logging is available)
                throw new ApplicationException($"Error updating room: {ex.Message}", ex);
            }
        }


        private async Task UpdateRoomTypeAvailability(Guid roomTypeId, string newStatus, string previousStatus)
        {
            var _roomTypeService = _serviceProvider.GetService<IRoomTypeService>();
            var roomType = await _roomTypeService.Get(roomTypeId);

            if (roomType == null)
            {
                throw new KeyNotFoundException("Room type not found.");
            }

            int newAvailableRooms = roomType.AvailableRooms.Value;

            // Only change AvailableRooms based on transitions from Available -> Occupied or Maintenance
            if (previousStatus == "Available" && (newStatus == "Occupied" || newStatus == "Maintenance"))
            {
                // Decrease AvailableRooms only if the room was available before
                if (newAvailableRooms > 0)
                {
                    newAvailableRooms -= 1; // Room is now occupied or in maintenance, decrement available rooms
                }
                else
                {
                    throw new InvalidOperationException("Cannot reduce available rooms below 0.");
                }
            }
            else if (previousStatus != "Available" && newStatus == "Available")
            {
                // Increase AvailableRooms only if the room was previously not available (Occupied or Maintenance)
                if (newAvailableRooms < roomType.TotalRooms)
                {
                    newAvailableRooms += 1; // Room is now available, increment available rooms
                }
                else
                {
                    throw new InvalidOperationException("Available rooms cannot exceed total rooms.");
                }
            }

            // Ensure AvailableRooms never exceed TotalRooms or drop below 0
            if (newAvailableRooms < 0 || newAvailableRooms > roomType.TotalRooms)
            {
                throw new InvalidOperationException("Available rooms cannot exceed total rooms or be negative.");
            }

            // Update room type with the new available room count
            await _roomTypeService.Update(roomType.RoomTypeId, new RoomTypeUpdateRequest
            {
                RoomTypeId = roomType.RoomTypeId,
                AvailableRooms = newAvailableRooms,
                TotalRooms = roomType.TotalRooms,
                HotelId = roomType.HotelId,
                TypeId = roomType.TypeId,
                Description = roomType.Description,
                RoomSize = roomType.RoomSize,
                IsActive = roomType.IsActive,
                Note = roomType.Note,
            });
        }





        public async Task<List<RoomResponse>> GetAllRoomByRoomTypeId(Guid id)
        {

            var list = await _unitOfWork.Repository<Room>().GetAll()
                                            .ProjectTo<RoomResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.RoomTypeId == id)
                                            .ToListAsync();
            return list;
        }

        public async Task<List<RoomResponse>> GetAllRoomByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<Room>().GetAll()
                                            .ProjectTo<RoomResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.RoomType.HotelId == id)
                                            .ToListAsync();
            return list;
        }

        public async Task<List<RoomResponse>> GetAllRoomByStaffId(Guid staffId)
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
            var rooms = await _unitOfWork.Repository<Room>()
                                                .GetAll()
                                                .Where(r => r.RoomType.HotelId == hotelId)
                                                .ProjectTo<RoomResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

            // Check if any rooms were found
            if (rooms == null || !rooms.Any())
            {
                throw new Exception("No rooms found for this staff's hotel.");
            }

            return rooms;
        }
    }
}
