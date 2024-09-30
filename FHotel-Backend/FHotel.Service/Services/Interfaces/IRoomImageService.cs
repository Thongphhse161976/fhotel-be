using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoomImageService
    {
        public Task<List<RoomImageResponse>> GetAll();

        public Task<RoomImageResponse> Get(Guid id);

        public Task<RoomImageResponse> Create(RoomImageRequest request);

        public Task<RoomImageResponse> Delete(Guid id);

        public Task<RoomImageResponse> Update(Guid id, RoomImageRequest request);
    }
}
