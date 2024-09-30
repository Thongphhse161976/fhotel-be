using FHotel.Services.DTOs.Cities;
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

        public Task<ReservationResponse> Create(ReservationRequest request);

        public Task<ReservationResponse> Delete(Guid id);

        public Task<ReservationResponse> Update(Guid id, ReservationRequest request);
    }
}
