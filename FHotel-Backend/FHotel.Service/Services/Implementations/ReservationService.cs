﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.ReservationValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IRoomTypeService _roomTypeService;
        private readonly ITypePricingService _typePricingService;
        public ReservationService(IUnitOfWork unitOfWork, IMapper mapper, IRoomTypeService roomTypeService,
            ITypePricingService typePricingService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roomTypeService = roomTypeService;
            _typePricingService = typePricingService;
        }

        public async Task<List<ReservationResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }

        public async Task<ReservationResponse> Get(Guid id)
        {
            try
            {
                ReservationResponse reservation = null;
                reservation = await _unitOfWork.Repository<Reservation>().GetAll()
                     .AsNoTracking()
                     .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                    .Where(x => x.ReservationId == id)
                    .FirstOrDefaultAsync();

                if (reservation == null)
                {
                    throw new Exception("Reservation not found");
                }

                return reservation;
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ReservationResponse> Create(ReservationCreateRequest request)
        {
            var validator = new ReservationCreateRequestValidator();
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
                // Fetch the RoomType entity
                var roomType = await _roomTypeService.Get(request.RoomTypeId.Value);

                if (roomType == null)
                {
                    throw new Exception("Room type not found.");
                }

                // Check if there are enough available rooms
                if (roomType.AvailableRooms < request.NumberOfRooms)
                {
                    throw new Exception("Not enough available rooms.");
                }

                // Reduce the available rooms
                roomType.AvailableRooms -= request.NumberOfRooms;

                var updateRoomType = new RoomTypeUpdateRequest()
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
                };

                // Update the RoomType in the database using RoomTypeUpdateRequest
                await _roomTypeService.Update(request.RoomTypeId.Value, updateRoomType);

                // Proceed with creating the reservation
                var reservation = _mapper.Map<ReservationCreateRequest, Reservation>(request);
                reservation.ReservationId = Guid.NewGuid();
                reservation.CreatedDate = localTime;
                reservation.ReservationStatus = "Pending";
                reservation.PaymentStatus = "Not Paid";

                await _unitOfWork.Repository<Reservation>().InsertAsync(reservation);

                // Commit both changes: the room type update and the new reservation
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Reservation, ReservationResponse>(reservation);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }




        public async Task<ReservationResponse> Delete(Guid id)
        {
            try
            {
                Reservation reservation = null;
                reservation = _unitOfWork.Repository<Reservation>()
                    .Find(p => p.ReservationId == id);
                if (reservation == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Reservation>().HardDeleteGuid(reservation.ReservationId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Reservation, ReservationResponse>(reservation);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReservationResponse> Update(Guid id, ReservationUpdateRequest request)
        {
            // Fetch the existing reservation
            var reservation = await _unitOfWork.Repository<Reservation>().FindAsync(x => x.ReservationId == id);

            // Check if the reservation exists
            if (reservation == null)
            {
                throw new KeyNotFoundException($"Reservation with ID {id} does not exist.");
            }

            // Validate the request (consider using a separate validator)
            var validator = new ReservationUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var updatereservation = _mapper.Map(request, reservation);

                await _unitOfWork.Repository<Reservation>().UpdateDetached(updatereservation);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Reservation, ReservationResponse>(reservation);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> CalculateTotalAmount(Guid roomTypeId, DateTime checkInDate, DateTime checkOutDate, int numberOfRooms)
        {
            var roomType = await _roomTypeService.Get(roomTypeId);

            if (roomType == null || roomType.IsActive != true || numberOfRooms <= 0)
            {
                throw new ArgumentException("Invalid room type or number of rooms.");
            }

            decimal totalAmount = 0;

            // Get the district ID from the room type
            var districtId = roomType.Hotel.DistrictId;

            for (DateTime currentDate = checkInDate.Date; currentDate < checkOutDate.Date; currentDate = currentDate.AddDays(1))
            {
                int dayOfWeek = (int)currentDate.DayOfWeek == 0 ? 7 : (int)currentDate.DayOfWeek;

                var dailyPricing = await _typePricingService.GetPricingByTypeAndDistrict(roomType.TypeId ?? Guid.Empty, districtId ?? Guid.Empty, dayOfWeek);

                if (dailyPricing == null)
                {
                    throw new Exception($"No pricing available for {currentDate.ToShortDateString()}.");
                }

                totalAmount += dailyPricing.Price.Value * numberOfRooms;
            }

            return totalAmount;
        }

        public async Task<List<ReservationResponse>> GetAllByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r=> r.RoomType.HotelId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }
        public async Task<List<ReservationResponse>> GetAllByUserId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r=> r.CustomerId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }
        public async Task<List<ReservationResponse>> GetAllByOwnerId(Guid id)
        {

            var list = await _unitOfWork.Repository<Reservation>().GetAll()
                                            .Where(r=> r.RoomType.Hotel.OwnerId == id)
                                            .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No reservations found.");
            }
            return list;
        }


        public async Task<List<ReservationResponse>> GetAllReservationByStaffId(Guid staffId)
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
            var reservations = await _unitOfWork.Repository<Reservation>()
                                                .GetAll()
                                                .Where(r => r.RoomType.HotelId == hotelId) // Assuming RoomTypeID or some other way links to the hotel
                                                .ProjectTo<ReservationResponse>(_mapper.ConfigurationProvider)
                                                .ToListAsync();

            // Check if any reservations were found
            if (reservations == null || !reservations.Any())
            {
                throw new Exception("No reservations found for this staff's hotel.");
            }

            return reservations;
        }


    }
}
