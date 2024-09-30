using FHotel.Repository.Models;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.ReservationDetails
{
    public class ReservationDetailResponse
    {
        public Guid ReservationDetailId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int? NumberOfRooms { get; set; }

        public virtual ReservationResponse? Reservation { get; set; }
        public virtual RoomTypeResponse? RoomType { get; set; }
    }
}
