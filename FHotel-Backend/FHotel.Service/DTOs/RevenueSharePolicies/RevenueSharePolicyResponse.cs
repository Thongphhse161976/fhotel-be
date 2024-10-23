using FHotel.Repository.Models;
using FHotel.Services.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RevenueSharePolicies
{
    public class RevenueSharePolicyResponse
    {
        public Guid RevenueSharePolicyId { get; set; }
        public Guid? RoleId { get; set; }
        public decimal? PercentageShare { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual RoleResponse? Role { get; set; }
    }
}
