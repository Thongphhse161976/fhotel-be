using FHotel.Repository.Models;
using FHotel.Services.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Feedbacks
{
    public class FeedbackResponse
    {
        public Guid FeedbackId { get; set; }
        public Guid? ReservationId { get; set; }
        public int? HotelRating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ReservationResponse? Reservation { get; set; }
    }
}
