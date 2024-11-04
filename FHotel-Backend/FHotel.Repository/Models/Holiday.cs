using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Holiday
    {
        public Holiday()
        {
            HolidayPricingRules = new HashSet<HolidayPricingRule>();
        }

        public Guid HolidayId { get; set; }
        public DateTime? HolidayDate { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<HolidayPricingRule> HolidayPricingRules { get; set; }
    }
}
