using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomFacilities
{
    public class RoomFacilityRequest
    {
      
        public string? RoomFacilityName { get; set; }
        public decimal? Price { get; set; }
        public Guid? RoomTypeId { get; set; }
    }
}
