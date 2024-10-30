using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Payment
    {
        public Guid PaymentId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
