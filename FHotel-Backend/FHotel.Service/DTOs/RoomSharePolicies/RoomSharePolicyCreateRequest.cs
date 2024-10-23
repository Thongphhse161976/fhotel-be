using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomSharePolicies
{
    public class RoomSharePolicyCreateRequest
    {
        public Guid? RoleId { get; set; }
        public decimal? PercentageShare { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
