using FHotel.Service.DTOs.TypePricings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface ITypePricingService
    {
        public Task<List<TypePricingResponse>> GetAll();

        public Task<TypePricingResponse> Get(Guid id);

        public Task<TypePricingResponse> Create(TypePricingCreateRequest request);

        public Task<TypePricingResponse> Delete(Guid id);

        public Task<TypePricingResponse> Update(Guid id, TypePricingUpdateRequest request);

        public Task<List<TypePricingResponse>> GetAllByTypeId(Guid id);

        public Task<List<TypePricingResponse>> GetAllByRoomTypeId(Guid roomTypeId);

        public Task<TypePricingResponse> GetPricingByTypeAndDistrict(Guid typeId, Guid districtId, int dayOfWeek);
        public Task<decimal> GetTodayPricingByRoomType(Guid roomTypeId);
    }
}
