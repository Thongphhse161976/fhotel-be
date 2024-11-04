using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HolidayPricingRules
{
    public class HolidayPricingRuleCreateRequest
    {
        public Guid? DistrictId { get; set; }
        public Guid? HolidayId { get; set; }
        public decimal? PercentageIncrease { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
