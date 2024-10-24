using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            BillReservations = new HashSet<BillReservation>();
            Bills = new HashSet<Bill>();
            Feedbacks = new HashSet<Feedback>();
            LateCheckOutCharges = new HashSet<LateCheckOutCharge>();
            Orders = new HashSet<Order>();
            Refunds = new HashSet<Refund>();
            RoomStayHistories = new HashSet<RoomStayHistory>();
            UserDocuments = new HashSet<UserDocument>();
        }

        public Guid ReservationId { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ReservationStatus { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int? NumberOfRooms { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutDate { get; set; }
        public string? PaymentStatus { get; set; }
        public Guid? PaymentMethodId { get; set; }

        public virtual User? Customer { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual RoomType? RoomType { get; set; }
        public virtual ICollection<BillReservation> BillReservations { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<LateCheckOutCharge> LateCheckOutCharges { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Refund> Refunds { get; set; }
        public virtual ICollection<RoomStayHistory> RoomStayHistories { get; set; }
        public virtual ICollection<UserDocument> UserDocuments { get; set; }
    }
}
