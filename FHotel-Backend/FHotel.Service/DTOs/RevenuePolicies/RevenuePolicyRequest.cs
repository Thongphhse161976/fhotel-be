using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RevenuePolicies
{
    public class RevenuePolicyRequest
    {
        public Guid? HotelId { get; set; }
        public decimal? AdminPercentage { get; set; }
        public decimal? HotelPercentage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
