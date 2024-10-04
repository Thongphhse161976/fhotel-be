using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            Bills = new HashSet<Bill>();
            Feedbacks = new HashSet<Feedback>();
            LateCheckOutCharges = new HashSet<LateCheckOutCharge>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
            ReservationDetails = new HashSet<ReservationDetail>();
            RoomStayHistories = new HashSet<RoomStayHistory>();
        }

        public Guid ReservationId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ReservationStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutDate { get; set; }

        public virtual User? Customer { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<LateCheckOutCharge> LateCheckOutCharges { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<ReservationDetail> ReservationDetails { get; set; }
        public virtual ICollection<RoomStayHistory> RoomStayHistories { get; set; }
    }
}
