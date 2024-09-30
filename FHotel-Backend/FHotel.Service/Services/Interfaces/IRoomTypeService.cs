using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoomTypeService
    {
        public Task<List<RoomTypeResponse>> GetAll();

        public Task<RoomTypeResponse> Get(Guid id);

        public Task<RoomTypeResponse> Create(RoomTypeRequest request);

        public Task<RoomTypeResponse> Delete(Guid id);

        public Task<RoomTypeResponse> Update(Guid id, RoomTypeRequest request);
    }
}
