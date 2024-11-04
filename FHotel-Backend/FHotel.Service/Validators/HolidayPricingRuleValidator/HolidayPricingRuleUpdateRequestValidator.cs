using FHotel.Service.DTOs.HolidayPricingRules;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HolidayPricingRuleValidator
{
    public class HolidayPricingRuleUpdateRequestValidator: AbstractValidator<HolidayPricingRuleUpdateRequest>
    {
        public HolidayPricingRuleUpdateRequestValidator()
        {
            
        }
    }
}
