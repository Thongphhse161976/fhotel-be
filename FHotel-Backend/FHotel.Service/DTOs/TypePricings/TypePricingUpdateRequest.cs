using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.TypePricings
{
    public class TypePricingUpdateRequest
    {
        public Guid? TypeId { get; set; }
        public Guid? DistrictId { get; set; }
        public int? DayOfWeek { get; set; }
        public decimal? Price { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public decimal? PercentageIncrease { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
