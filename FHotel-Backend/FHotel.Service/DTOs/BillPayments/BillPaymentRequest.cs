using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillPayments
{
    public class BillPaymentRequest
    {
        public Guid? BillId { get; set; }
        public Guid? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
