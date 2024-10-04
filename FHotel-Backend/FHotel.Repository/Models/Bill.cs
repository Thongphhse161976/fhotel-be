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
            BillPayments = new HashSet<BillPayment>();
        }

        public Guid BillId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? BillStatus { get; set; }

        public virtual Reservation? Reservation { get; set; }
        public virtual ICollection<BillLateCheckOutCharge> BillLateCheckOutCharges { get; set; }
        public virtual ICollection<BillOrder> BillOrders { get; set; }
        public virtual ICollection<BillPayment> BillPayments { get; set; }
    }
}
