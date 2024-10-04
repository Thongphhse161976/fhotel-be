using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Payment
    {
        public Payment()
        {
            BillPayments = new HashSet<BillPayment>();
            Refunds = new HashSet<Refund>();
        }

        public Guid PaymentId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public string? PaymentStatus { get; set; }

        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual ICollection<BillPayment> BillPayments { get; set; }
        public virtual ICollection<Refund> Refunds { get; set; }
    }
}
