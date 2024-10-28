using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class OrderDetail
    {
        public Guid OrderDetailId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ServiceId { get; set; }
        public int? Quantity { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Service? Service { get; set; }
    }
}
