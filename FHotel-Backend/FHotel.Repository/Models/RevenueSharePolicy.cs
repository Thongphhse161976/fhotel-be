using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RevenueSharePolicy
    {
        public Guid RevenueSharePolicyId { get; set; }
        public Guid? RoleId { get; set; }
        public decimal? PercentageShare { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Role? Role { get; set; }
    }
}
