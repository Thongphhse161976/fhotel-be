using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HolidayPricingRule
    {
        public Guid HolidayPricingRuleId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? HolidayId { get; set; }
        public decimal? PercentageIncrease { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual District? District { get; set; }
        public virtual Holiday? Holiday { get; set; }
    }
}
