using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Feedbacks
{
    public class FeedbackRequest
    {
        public Guid? ReservationId { get; set; }
        public int? HotelRating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
        
    }
}
