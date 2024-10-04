using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Order
    {
        public Order()
        {
            BillOrders = new HashSet<BillOrder>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid OrderId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? OrderStatus { get; set; }

        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual ICollection<BillOrder> BillOrders { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
