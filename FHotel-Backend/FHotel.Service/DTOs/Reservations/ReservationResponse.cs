using FHotel.Repository.Models;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Reservations
{
    public class ReservationResponse
    {
        public Guid ReservationId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ReservationStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutDate { get; set; }

        public virtual UserResponse? Customer { get; set; }
    }
}
