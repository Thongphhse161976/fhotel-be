using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class RoomStayHistoryService: IRoomStayHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IReservationService _reservationService;
        private IRoomService _roomService;
        public RoomStayHistoryService(IUnitOfWork unitOfWork, IMapper mapper, IReservationService reservationService, IRoomService roomService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reservationService = reservationService;
            _roomService = roomService;
        }

        public async Task<List<RoomStayHistoryResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomStayHistory>().GetAll()
                                            .ProjectTo<RoomStayHistoryResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomStayHistoryResponse> Get(Guid id)
        {
            try
            {
                RoomStayHistory roomStayHistory = null;
                roomStayHistory = await _unitOfWork.Repository<RoomStayHistory>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomStayHistoryId == id)
                    .FirstOrDefaultAsync();

                if (roomStayHistory == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomStayHistoryResponse> Create(RoomStayHistoryRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            var reservation = await _unitOfWork.Repository<Reservation>()
                        .AsNoTracking()
                        .Where(x => x.ReservationId == request.ReservationId)
                        .FirstOrDefaultAsync();

            if (reservation == null)
            {
                throw new Exception("Reservation not found");
            }
            var room = await _unitOfWork.Repository<Room>()
                        .AsNoTracking()
                        .Where(x => x.RoomId == request.RoomId)
                        .FirstOrDefaultAsync();

            if (room == null)
            {
                throw new Exception("Room not found");
            }
            // Check if the room is already used in another stay history
            var roomStayAlready = await _unitOfWork.Repository<RoomStayHistory>()
                        .AsNoTracking()
                        .Where(x => x.RoomId == request.RoomId)
                        .FirstOrDefaultAsync();

            if (roomStayAlready != null)
            {
                throw new Exception("Room is already used");
            }
            var existingRoomStayHistories = await _unitOfWork.Repository<RoomStayHistory>()
                                            .AsNoTracking()
                                            .Where(x => x.ReservationId == reservation.ReservationId)
                                            .CountAsync();
            if (existingRoomStayHistories == reservation.NumberOfRooms)
            {
                throw new Exception("All rooms for this reservation have already been checked in. No more RoomStayHistory can be added.");
            }
            try
            {
                var roomStayHistory = _mapper.Map<RoomStayHistoryRequest, RoomStayHistory>(request);
                var roomUpdateRequest = _mapper.Map<Room, RoomRequest>(room);
                room.Status = "Occupied";
                await _roomService.Update(room.RoomId, roomUpdateRequest);
                roomStayHistory.RoomStayHistoryId = Guid.NewGuid();
                roomStayHistory.CreatedDate = localTime;
                roomStayHistory.CheckInDate = localTime;
                await _unitOfWork.Repository<RoomStayHistory>().InsertAsync(roomStayHistory);
                await _unitOfWork.CommitAsync();
                // Get the total number of RoomStayHistory entries for this reservation
                var roomAlreadyStayed = await _unitOfWork.Repository<RoomStayHistory>()
                                            .AsNoTracking()
                                            .Where(x => x.ReservationId == reservation.ReservationId)
                                            .CountAsync();

                // Check if the number of RoomStayHistories has reached the number of reserved rooms
                if (roomAlreadyStayed == reservation.NumberOfRooms)
                {
                    var reservationUpdateRequest = _mapper.Map<Reservation, ReservationUpdateRequest>(reservation);
                    reservationUpdateRequest.ReservationStatus = "Confirmed";
                    reservationUpdateRequest.ActualCheckInTime = localTime;
                    await _reservationService.Update(reservation.ReservationId, reservationUpdateRequest);
                }
                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomStayHistoryResponse> Delete(Guid id)
        {
            try
            {
                RoomStayHistory roomStayHistory = null;
                roomStayHistory = _unitOfWork.Repository<RoomStayHistory>()
                    .Find(p => p.RoomStayHistoryId == id);
                if (roomStayHistory == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<RoomStayHistory>().HardDeleteGuid(roomStayHistory.RoomStayHistoryId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomStayHistoryResponse> Update(Guid id, RoomStayHistoryRequest request)
        {
            try
            {
                RoomStayHistory roomStayHistory = _unitOfWork.Repository<RoomStayHistory>()
                            .Find(x => x.RoomStayHistoryId == id);
                if (roomStayHistory == null)
                {
                    throw new Exception();
                }
                roomStayHistory = _mapper.Map(request, roomStayHistory);

                await _unitOfWork.Repository<RoomStayHistory>().UpdateDetached(roomStayHistory);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RoomStayHistoryResponse>> GetAllByReservationId(Guid id)
        {

            var list = await _unitOfWork.Repository<RoomStayHistory>().GetAll()
                                            .ProjectTo<RoomStayHistoryResponse>(_mapper.ConfigurationProvider)
                                            .Where(r=> r.ReservationId == id)
                                            .ToListAsync();
            if (list == null)
            {
                throw new Exception("Room stay not found");
            }
            return list;
        }
    }
}
