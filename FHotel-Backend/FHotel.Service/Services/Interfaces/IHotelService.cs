using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IHotelService 
    {
        public Task<List<HotelResponse>> GetAll();

        public Task<HotelResponse> Get(Guid id);

        public Task<HotelResponse> Create(HotelRequest request);

        public Task<HotelResponse> Delete(Guid id);

        public Task<HotelResponse> Update(Guid id, HotelRequest request);
    }
}
