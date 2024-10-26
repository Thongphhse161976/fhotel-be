using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillLateCheckOutCharges = new HashSet<BillLateCheckOutCharge>();
            BillOrders = new HashSet<BillOrder>();
            BillTransactionImages = new HashSet<BillTransactionImage>();
        }

        public Guid BillId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? PrepaidAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? BillStatus { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual ICollection<BillLateCheckOutCharge> BillLateCheckOutCharges { get; set; }
        public virtual ICollection<BillOrder> BillOrders { get; set; }
        public virtual ICollection<BillTransactionImage> BillTransactionImages { get; set; }
    }
}
