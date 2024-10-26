using FHotel.Service.DTOs.HotelVerifications;
using FHotel.Services.DTOs.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IHotelVerificationService
    {
        public Task<List<HotelVerificationResponse>> GetAll();

        public Task<HotelVerificationResponse> Get(Guid id);

        public Task<HotelVerificationResponse> Create(HotelVerificationRequest request);

        public Task<HotelVerificationResponse> Delete(Guid id);

        public Task<HotelVerificationResponse> Update(Guid id, HotelVerificationRequest request);

        public Task<List<HotelVerificationResponse>> GetAllByHotelId(Guid id);
        public Task<List<HotelVerificationResponse>> GetAllByAssignManagerId(Guid id);
    }
}
