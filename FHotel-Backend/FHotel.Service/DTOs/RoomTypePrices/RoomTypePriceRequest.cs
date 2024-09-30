using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomTypePrices
{
    public class RoomTypePriceRequest
    {
      
        public Guid? PriceId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? DayOfWeek { get; set; }
        public decimal? Price { get; set; }

      
    }
}
