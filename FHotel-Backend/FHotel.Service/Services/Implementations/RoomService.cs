using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.Validators.ReservationValidator;
using FHotel.Service.Validators.RoomValidator;
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

        public async Task<RoomResponse> Update(Guid id, RoomRequest request)
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

                // If room is available, update the RoomType's available rooms
                if (room.Status == "Available")
                {
                    var _roomTypeService = _serviceProvider.GetService<IRoomTypeService>();
                    var roomType = await _roomTypeService.Get(room.RoomTypeId.Value);
                    if (roomType.AvailableRooms < roomType.TotalRooms)
                    {
                        roomType.AvailableRooms += 1;

                        await _roomTypeService.Update(roomType.RoomTypeId, new RoomTypeUpdateRequest
                        {
                            RoomTypeId = roomType.RoomTypeId,
                            AvailableRooms = roomType.AvailableRooms,
                            TotalRooms = roomType.TotalRooms,
                            HotelId = roomType.HotelId,
                            TypeId = roomType.TypeId,
                            Description = roomType.Description,
                            RoomSize = roomType.RoomSize,
                            IsActive = roomType.IsActive,
                            Note = roomType.Note,
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException("Available rooms cannot exceed total rooms.");
                    }
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
