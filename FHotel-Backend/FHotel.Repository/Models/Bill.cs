using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillTransactionImages = new HashSet<BillTransactionImage>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
            Transactions = new HashSet<Transaction>();
        }

        public Guid BillId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? BillStatus { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual Reservation? Reservation { get; set; }
        public virtual ICollection<BillTransactionImage> BillTransactionImages { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
