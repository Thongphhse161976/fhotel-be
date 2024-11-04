using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HolidayPricingRules
{
    public class HolidayPricingRuleUpdateRequest
    {
        public Guid HolidayPricingRuleId { get; set; }
        public DateTime? HolidayDate { get; set; }
        public decimal? PercentageIncrease { get; set; }
        public string? Description { get; set; }
        public Guid? DistrictId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
