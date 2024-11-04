using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class District
    {
        public District()
        {
            HolidayPricingRules = new HashSet<HolidayPricingRule>();
            Hotels = new HashSet<Hotel>();
            TypePricings = new HashSet<TypePricing>();
        }

        public Guid DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public Guid? CityId { get; set; }

        public virtual City? City { get; set; }
        public virtual ICollection<HolidayPricingRule> HolidayPricingRules { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<TypePricing> TypePricings { get; set; }
    }
}
