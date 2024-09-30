using FHotel.Services.DTOs.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface ICityService
    {
        public Task<List<CityResponse>> GetAll();

        public Task<CityResponse> Get(Guid id);

        public Task<CityResponse> Create(CityRequest request);

        public Task<CityResponse> Delete(Guid id);

        public Task<CityResponse> Update(Guid id, CityRequest request);
    }
}
