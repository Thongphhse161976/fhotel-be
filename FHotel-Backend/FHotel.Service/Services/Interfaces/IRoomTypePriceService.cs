using FHotel.Service.DTOs.RoomTypePrices;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomTypePrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoomTypePriceService
    {
        public Task<List<RoomTypePriceResponse>> GetAll();

        public Task<RoomTypePriceResponse> Get(Guid id);

        public Task<RoomTypePriceResponse> Create(RoomTypePriceCreateRequest request);

        public Task<RoomTypePriceResponse> Delete(Guid id);

        public Task<RoomTypePriceResponse> Update(Guid id, RoomTypePriceUpdateRequest request);
    }
}
