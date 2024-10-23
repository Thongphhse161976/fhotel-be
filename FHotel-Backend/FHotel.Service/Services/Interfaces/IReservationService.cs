using FHotel.Service.DTOs.Reservations;
using FHotel.Services.DTOs.Reservations;
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
        public Task<decimal> CalculateTotalAmount(Guid roomTypeId
            , DateTime checkInDate, DateTime checkOutDate, int numberOfRooms);

        public Task<List<ReservationResponse>> GetAllByHotelId(Guid id);

        public Task<List<ReservationResponse>> GetAllByUserId(Guid id);

        public Task<List<ReservationResponse>> GetAllReservationByStaffId(Guid staffId);

    }
}
