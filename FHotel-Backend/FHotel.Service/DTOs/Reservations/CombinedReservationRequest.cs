using FHotel.Services.DTOs.ReservationDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Reservations
{
    public class CombinedReservationRequest
    {
        public ReservationCreateRequest Reservation { get; set; }
        public ReservationDetailRequest Detail { get; set; }
    }
}
