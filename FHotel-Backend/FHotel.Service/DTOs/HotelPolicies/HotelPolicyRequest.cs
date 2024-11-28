using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelPolicies
{
    public class HotelPolicyRequest
    {
        public Guid? HotelId { get; set; }
        public Guid? PolicyId { get; set; }
        public string? SpecificDetails { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
