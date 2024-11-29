using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class CancellationPolicy
    {
        public Guid CancellationPolicyId { get; set; }
        public Guid? HotelId { get; set; }
        public decimal? RefundPercentage { get; set; }
        public int? CancellationDays { get; set; }
        public string? CancellationTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }
}
