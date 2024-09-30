using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IPriceService
    {
        public Task<List<PriceResponse>> GetAll();

        public Task<PriceResponse> Get(Guid id);

        public Task<PriceResponse> Create(PriceRequest request);

        public Task<PriceResponse> Delete(Guid id);

        public Task<PriceResponse> Update(Guid id, PriceRequest request);
    }
}
