using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RefundPolicies
{
    public class RefundPolicyResponse
    {
        public Guid RefundPolicyId { get; set; }
        public string? CancellationTime { get; set; }
        public decimal? RefundPercentage { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
