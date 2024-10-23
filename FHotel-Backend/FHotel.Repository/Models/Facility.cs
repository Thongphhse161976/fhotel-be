using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Facility
    {
        public Facility()
        {
            RoomFacilities = new HashSet<RoomFacility>();
        }

        public Guid FacilityId { get; set; }
        public string? FacilityName { get; set; }

        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }
    }
}
