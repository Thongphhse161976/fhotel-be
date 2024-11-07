using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class TypePricing
    {
        public Guid TypePricingId { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? DistrictId { get; set; }
        public int? DayOfWeek { get; set; }
        public decimal? Price { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public decimal? PercentageIncrease { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual District? District { get; set; }
        public virtual Type? Type { get; set; }
    }
}
