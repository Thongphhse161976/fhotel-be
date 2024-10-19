using FHotel.Service.DTOs.Districts;
using FHotel.Services.DTOs.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IDistrictService
    {
        public Task<List<DistrictResponse>> GetAll();

        public Task<DistrictResponse> Get(Guid id);

        public Task<DistrictResponse> Create(DistrictRequest request);

        public Task<DistrictResponse> Delete(Guid id);

        public Task<DistrictResponse> Update(Guid id, DistrictRequest request);

        public Task<List<DistrictResponse>> GetAllByCityId(Guid id);
    }
}
