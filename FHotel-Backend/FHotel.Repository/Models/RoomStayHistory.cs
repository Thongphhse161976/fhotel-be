using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomStayHistory
    {
        public Guid RoomStayHistoryId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? RoomId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Reservation? Reservation { get; set; }
        public virtual Room? Room { get; set; }
    }
}
