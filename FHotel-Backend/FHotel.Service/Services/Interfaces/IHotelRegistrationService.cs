using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.HotelRegistations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IHotelRegistrationService
    {
        public Task<List<HotelRegistrationResponse>> GetAll();

        public Task<HotelRegistrationResponse> Get(Guid id);

        public Task<HotelRegistrationResponse> Create(HotelRegistrationRequest request);

        public Task<HotelRegistrationResponse> Delete(Guid id);

        public Task<HotelRegistrationResponse> Update(Guid id, HotelRegistrationRequest request);
    }
}
