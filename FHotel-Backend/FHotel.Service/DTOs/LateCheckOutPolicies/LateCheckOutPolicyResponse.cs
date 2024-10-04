using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.LateCheckOutPolicies
{
    public class LateCheckOutPolicyResponse
    {
        public Guid LateCheckOutPolicyId { get; set; }
        public string? Description { get; set; }
        public decimal? ChargePercentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
