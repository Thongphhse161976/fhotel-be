using FHotel.Repository.Models;
using FHotel.Services.DTOs.RoomFacilities;
using FHotel.Services.DTOs.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.DamagedFactilities
{
    public class DamagedFacilityResponse
    {
        public Guid DamagedFacilityId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? RoomFacilityId { get; set; }
        public string? DamageDescription { get; set; }
        public decimal? DamageCost { get; set; }
        public DateTime? ReportedDate { get; set; }
        public string? Status { get; set; }

        public virtual RoomResponse? Room { get; set; }
        public virtual RoomFacilityResponse? RoomFacility { get; set; }
    }
}
