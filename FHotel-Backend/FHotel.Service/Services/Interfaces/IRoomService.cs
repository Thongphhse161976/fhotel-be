using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoomService
    {
        public Task<List<RoomResponse>> GetAll();

        public Task<RoomResponse> Get(Guid id);

        public Task<RoomResponse> Create(RoomRequest request);

        public Task<RoomResponse> Delete(Guid id);

        public Task<RoomResponse> Update(Guid id, RoomRequest request);
        public Task<List<RoomResponse>> GetAllRoomByRoomTypeId(Guid id);
        public Task<List<RoomResponse>> GetAllRoomByStaffId(Guid staffId);
    }
}
