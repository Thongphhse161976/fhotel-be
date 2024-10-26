using FHotel.Service.DTOs.Areas;
using FHotel.Services.DTOs.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IAreaService
    {
        public Task<List<AreaResponse>> GetAll();

        public Task<AreaResponse> Get(Guid id);

        public Task<AreaResponse> Create(AreaRequest request);

        public Task<AreaResponse> Delete(Guid id);

        public Task<AreaResponse> Update(Guid id, AreaRequest request);
    }
}
