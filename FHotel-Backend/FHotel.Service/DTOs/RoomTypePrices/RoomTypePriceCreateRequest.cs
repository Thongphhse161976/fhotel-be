using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomTypePrices
{
    public class RoomTypePriceCreateRequest
    { 
        public string? DayOfWeek { get; set; }
        public decimal? PercentageIncrease { get; set; }
    }
}
