using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class ReservationDetail
    {
        public Guid ReservationDetailId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int? NumberOfRooms { get; set; }

        public virtual Reservation? Reservation { get; set; }
        public virtual RoomType? RoomType { get; set; }
    }
}
