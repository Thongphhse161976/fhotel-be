using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.CancellationPolicies
{
    public class CancellationPolicyRequest
    {
        public Guid? HotelId { get; set; }
        public decimal? RefundPercentage { get; set; }
        public int? CancellationDays { get; set; }
        public TimeSpan? CancellationTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
