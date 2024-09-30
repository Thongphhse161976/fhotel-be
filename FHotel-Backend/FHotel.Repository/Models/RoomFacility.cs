using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomFacility
    {
        public RoomFacility()
        {
            DamagedFacilities = new HashSet<DamagedFacility>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid RoomFacilityId { get; set; }
        public string? RoomFacilityName { get; set; }
        public decimal? Price { get; set; }
        public Guid? RoomTypeId { get; set; }

        public virtual RoomType? RoomType { get; set; }
        public virtual ICollection<DamagedFacility> DamagedFacilities { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
