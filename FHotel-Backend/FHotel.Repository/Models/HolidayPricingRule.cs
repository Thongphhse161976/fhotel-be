using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HolidayPricingRule
    {
        public Guid HolidayPricingRuleId { get; set; }
        public DateTime? HolidayDate { get; set; }
        public decimal? PercentageIncrease { get; set; }
        public string? Description { get; set; }
        public Guid? DistrictId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual District? District { get; set; }
    }
}
