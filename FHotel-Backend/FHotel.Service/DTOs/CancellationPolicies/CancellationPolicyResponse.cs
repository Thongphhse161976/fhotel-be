using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.CancellationPolicies
{
    public class CancellationPolicyResponse
    {
        public Guid CancellationPolicyId { get; set; }
        public Guid? HotelId { get; set; }
        public decimal? RefundPercentage { get; set; }
        public int? CancellationDays { get; set; }
        public string? CancellationTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
    }
}
