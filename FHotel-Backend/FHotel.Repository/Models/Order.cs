using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid OrderId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? BillId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? OrderStatus { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
