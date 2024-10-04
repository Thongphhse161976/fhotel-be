using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Room
    {
        public Room()
        {
            LateCheckOutCharges = new HashSet<LateCheckOutCharge>();
            RoomStayHistories = new HashSet<RoomStayHistory>();
        }

        public Guid RoomId { get; set; }
        public int? RoomNumber { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }

        public virtual RoomType? RoomType { get; set; }
        public virtual ICollection<LateCheckOutCharge> LateCheckOutCharges { get; set; }
        public virtual ICollection<RoomStayHistory> RoomStayHistories { get; set; }
    }
}
