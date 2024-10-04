using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RefundPolicy
    {
        public RefundPolicy()
        {
            Refunds = new HashSet<Refund>();
        }

        public Guid RefundPolicyId { get; set; }
        public string? CancellationTime { get; set; }
        public decimal? RefundPercentage { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Refund> Refunds { get; set; }
    }
}
