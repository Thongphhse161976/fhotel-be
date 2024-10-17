using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomFacility
    {
        public Guid RoomFacilityId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public Guid? FacilityId { get; set; }

        public virtual Facility? Facility { get; set; }
        public virtual RoomType? RoomType { get; set; }
    }
}
