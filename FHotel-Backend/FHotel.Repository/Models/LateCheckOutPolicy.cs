using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class LateCheckOutPolicy
    {
        public LateCheckOutPolicy()
        {
            LateCheckOutCharges = new HashSet<LateCheckOutCharge>();
        }

        public Guid LateCheckOutPolicyId { get; set; }
        public string? Description { get; set; }
        public decimal? ChargePercentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<LateCheckOutCharge> LateCheckOutCharges { get; set; }
    }
}
