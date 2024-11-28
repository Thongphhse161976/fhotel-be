using FHotel.Repository.Models;
using FHotel.Service.DTOs.Policies;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelPolicies
{
    public class HotelPolicyResponse
    {
        public Guid HotelPolicyId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? PolicyId { get; set; }
        public string? SpecificDetails { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
        public virtual PolicyResponse? Policy { get; set; }
    }
}
