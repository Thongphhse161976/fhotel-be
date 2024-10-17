using FHotel.Services.DTOs.ReservationDetails;
using FHotel.Services.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Reservations
{
    public class CombinedReservationResponse
    {
        public ReservationResponse Reservation { get; set; }
        public ReservationDetailResponse ReservationDetail { get; set; }
    }
}
