using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomFacility
    {
        public Guid RoomFacilityId { get; set; }
        public string? RoomFacilityName { get; set; }
        public decimal? Price { get; set; }
        public Guid? RoomTypeId { get; set; }

        public virtual RoomType? RoomType { get; set; }
    }
}
