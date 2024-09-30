using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class DamagedFacility
    {
        public Guid DamagedFacilityId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? RoomFacilityId { get; set; }
        public string? DamageDescription { get; set; }
        public decimal? DamageCost { get; set; }
        public DateTime? ReportedDate { get; set; }
        public string? Status { get; set; }

        public virtual Room? Room { get; set; }
        public virtual RoomFacility? RoomFacility { get; set; }
    }
}
