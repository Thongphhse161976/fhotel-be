using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class BillPayment
    {
        public Guid BillPaymentId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
