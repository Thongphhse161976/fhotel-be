using FHotel.Service.DTOs.Amenities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IAmenityService
    {
        public Task<List<AmenityResponse>> GetAll();

        public Task<AmenityResponse> Get(Guid id);

        public Task<AmenityResponse> Create(AmenityRequest request);

        public Task<AmenityResponse> Delete(Guid id);

        public Task<AmenityResponse> Update(Guid id, AmenityRequest request);
    }
}
