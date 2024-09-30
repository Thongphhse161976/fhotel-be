using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.DamagedFactilities
{
    public class DamagedFacilityRequest
    {
        public Guid? RoomId { get; set; }
        public Guid? RoomFacilityId { get; set; }
        public string? DamageDescription { get; set; }
        public decimal? DamageCost { get; set; }
        public DateTime? ReportedDate { get; set; }
        public string? Status { get; set; }
    }
}
