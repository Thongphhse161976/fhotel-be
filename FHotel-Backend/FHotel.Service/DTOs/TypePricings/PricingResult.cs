using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.TypePricings
{
    public class PricingResult
    {
        public decimal TotalAmount { get; set; }
        public string PriceBreakdown { get; set; }
    }
}
