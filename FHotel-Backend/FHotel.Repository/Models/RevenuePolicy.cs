using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RevenuePolicy
    {
        public Guid RevenuePolicyId { get; set; }
        public Guid? HotelId { get; set; }
        public decimal? AdminPercentage { get; set; }
        public decimal? HotelPercentage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }
}
