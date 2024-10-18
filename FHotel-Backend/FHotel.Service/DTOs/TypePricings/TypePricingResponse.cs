using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.Types;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.TypePricings
{
    public class TypePricingResponse
    {
        public Guid TypePricingId { get; set; }
        public Guid? TypeId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public int? DayOfWeek { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual CityResponse? City { get; set; }
        public virtual CountryResponse? Country { get; set; }
        public virtual DistrictResponse? District { get; set; }
        public virtual TypeResponse? Type { get; set; }
    }
}
