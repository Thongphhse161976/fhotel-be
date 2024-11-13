using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Feedback
    {
        public Guid FeedbackId { get; set; }
        public Guid? ReservationId { get; set; }
        public int? HotelRating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Reservation? Reservation { get; set; }
      
    }
}
