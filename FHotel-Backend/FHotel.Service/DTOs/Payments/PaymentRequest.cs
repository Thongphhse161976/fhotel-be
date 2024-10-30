using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Payments
{
    public class PaymentRequest
    {
        public Guid? BillId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
