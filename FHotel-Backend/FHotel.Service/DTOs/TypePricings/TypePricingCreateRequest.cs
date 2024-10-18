using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.TypePricings
{
    public class TypePricingCreateRequest
    {
        public Guid? TypeId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public int? DayOfWeek { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
