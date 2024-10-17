using FHotel.Service.DTOs.Facilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IFacilityService
    {
        public Task<List<FacilityResponse>> GetAll();

        public Task<FacilityResponse> Get(Guid id);

        public Task<FacilityResponse> Create(FacilityRequest request);

        public Task<FacilityResponse> Delete(Guid id);

        public Task<FacilityResponse> Update(Guid id, FacilityRequest request);
    }
}
