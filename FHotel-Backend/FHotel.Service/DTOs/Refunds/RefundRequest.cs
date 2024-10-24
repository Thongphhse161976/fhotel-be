using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Refunds
{
    public class RefundRequest
    {
        public Guid? ReservationId { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? RefundStatus { get; set; }
        public DateTime? RefundDate { get; set; }
        public Guid? RefundPolicyId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
