using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.DamagedFactilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IDamagedFacilityService
    {
        public Task<List<DamagedFacilityResponse>> GetAll();

        public Task<DamagedFacilityResponse> Get(Guid id);

        public Task<DamagedFacilityResponse> Create(DamagedFacilityRequest request);

        public Task<DamagedFacilityResponse> Delete(Guid id);

        public Task<DamagedFacilityResponse> Update(Guid id, DamagedFacilityRequest request);
    }
}
