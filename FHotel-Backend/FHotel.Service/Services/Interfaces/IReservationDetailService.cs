using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.ReservationDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IReservationDetailService
    {
        public Task<List<ReservationDetailResponse>> GetAll();

        public Task<ReservationDetailResponse> Get(Guid id);

        public Task<ReservationDetailResponse> Create(ReservationDetailRequest request);

        public Task<ReservationDetailResponse> Delete(Guid id);

        public Task<ReservationDetailResponse> Update(Guid id, ReservationDetailRequest request);

        public Task<List<ReservationDetailResponse>> GetAllByReservationId(Guid id);
    }
}
