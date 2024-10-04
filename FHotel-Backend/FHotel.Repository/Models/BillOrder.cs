using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class BillOrder
    {
        public Guid BillOrderId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal? Amount { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual Order? Order { get; set; }
    }
}
