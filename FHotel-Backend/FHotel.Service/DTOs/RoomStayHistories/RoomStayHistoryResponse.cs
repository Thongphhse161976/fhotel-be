using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomStayHistories
{
    public class RoomStayHistoryResponse
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
