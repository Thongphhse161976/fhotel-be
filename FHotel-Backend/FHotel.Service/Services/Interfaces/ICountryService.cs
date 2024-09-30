using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface ICountryService
    {

        public Task<List<CountryResponse>> GetAll();

        public Task<CountryResponse> Get(Guid id);

        public Task<CountryResponse> Create(CountryRequest request);

        public Task<CountryResponse> Delete(Guid id);

        public Task<CountryResponse> Update(Guid id, CountryRequest request);
    }
}
