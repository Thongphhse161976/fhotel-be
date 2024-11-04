using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.Holidays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HolidayPricingRules
{
    public class HolidayPricingRuleResponse
    {
        public Guid HolidayPricingRuleId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? HolidayId { get; set; }
        public decimal? PercentageIncrease { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual DistrictResponse? District { get; set; }
        public virtual HolidayResponse? Holiday { get; set; }
    }
}
