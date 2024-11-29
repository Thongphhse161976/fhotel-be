using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RevenuePolicies
{
    public class RevenuePolicyResponse
    {
        public Guid RevenuePolicyId { get; set; }
        public Guid? HotelId { get; set; }
        public decimal? AdminPercentage { get; set; }
        public decimal? HotelPercentage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
    }
}
