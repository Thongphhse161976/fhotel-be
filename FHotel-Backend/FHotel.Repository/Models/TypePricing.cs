using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class TypePricing
    {
        public Guid TypePricingId { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? AreaId { get; set; }
        public int? DayOfWeek { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Area? Area { get; set; }
        public virtual Type? Type { get; set; }
    }
}
