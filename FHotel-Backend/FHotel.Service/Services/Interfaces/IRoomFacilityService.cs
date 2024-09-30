using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.RoomFacilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoomFacilityService
    {
        public Task<List<RoomFacilityResponse>> GetAll();

        public Task<RoomFacilityResponse> Get(Guid id);

        public Task<RoomFacilityResponse> Create(RoomFacilityRequest request);

        public Task<RoomFacilityResponse> Delete(Guid id);

        public Task<RoomFacilityResponse> Update(Guid id, RoomFacilityRequest request);
    }
}
