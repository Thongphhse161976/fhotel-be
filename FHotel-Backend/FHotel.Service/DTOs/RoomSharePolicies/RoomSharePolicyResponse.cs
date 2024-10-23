using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomSharePolicies
{
    internal class RoomSharePolicyResponse
    {
        public Guid RevenueSharePolicyId { get; set; }
        public Guid? RoleId { get; set; }
        public decimal? PercentageShare { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Role? Role { get; set; }
    }
}
