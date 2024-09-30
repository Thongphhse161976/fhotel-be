using FHotel.Repository.Models;
using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomFacilities
{
    public class RoomFacilityResponse
    {
        public Guid RoomFacilityId { get; set; }
        public string? RoomFacilityName { get; set; }
        public decimal? Price { get; set; }
        public Guid? RoomTypeId { get; set; }

        public virtual RoomTypeResponse? RoomType { get; set; }
    }
}
