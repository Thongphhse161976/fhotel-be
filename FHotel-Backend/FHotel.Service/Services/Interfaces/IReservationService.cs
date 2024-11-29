using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.Reservations;
using FHotel.Services.DTOs.Reservations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IReservationService
    {
        public Task<List<ReservationResponse>> GetAll();

        public Task<ReservationResponse> Get(Guid id);

        public Task<ReservationResponse> Create(ReservationCreateRequest request);

        public Task<ReservationResponse> Delete(Guid id);

        public Task<ReservationResponse> Update(Guid id, ReservationUpdateRequest request);
        public Task<object> CalculateTotalAmount(Guid roomTypeId
            , DateTime checkInDate, DateTime checkOutDate, int numberOfRooms);

        public Task<List<ReservationResponse>> GetAllByHotelId(Guid id);

        public Task<List<ReservationResponse>> GetAllByUserId(Guid id);

        public Task<List<ReservationResponse>> GetAllByUserStaffId(Guid customerId, Guid staffId);

        public Task<List<ReservationResponse>> GetAllByUserOwnerId(Guid customerId, Guid ownerId);

        public Task<List<ReservationResponse>> GetAllReservationByStaffId(Guid staffId);

        public Task<List<ReservationResponse>> GetAllByOwnerId(Guid id);

        public Task<List<ReservationResponse>> GetAllByRoomTypeId(Guid id);

        public Task<List<ReservationResponse>> SearchReservations(Guid staffId, string? query);

        public Task<string> Refund(Guid id);
        public Task CheckAndProcessReservationsAsync();
        public Task ProcessReservationAsync(ReservationResponse reservation);

        public Task<ReservationResponse[]> GetEligibleReservationsAsync();

        public bool Is60SecondsAfterReservation(ReservationResponse reservation);
        public Task<string?> Pay(Guid id, HttpContext httpContext);
    }
}
