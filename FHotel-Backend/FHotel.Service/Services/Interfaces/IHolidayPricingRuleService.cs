using FHotel.Service.DTOs.HolidayPricingRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IHolidayPricingRuleService
    {
        public Task<List<HolidayPricingRuleResponse>> GetAll();

        public Task<HolidayPricingRuleResponse> Get(Guid id);

        public Task<HolidayPricingRuleResponse> Create(HolidayPricingRuleCreateRequest request);

        public Task<HolidayPricingRuleResponse> Delete(Guid id);

        public Task<HolidayPricingRuleResponse> Update(Guid id, HolidayPricingRuleUpdateRequest request);
    }
}
