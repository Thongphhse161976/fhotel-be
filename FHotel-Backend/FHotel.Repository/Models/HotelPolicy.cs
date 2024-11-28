using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelPolicy
    {
        public Guid HotelPolicyId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? PolicyId { get; set; }
        public string? SpecificDetails { get; set; }
        public decimal? Percentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Policy? Policy { get; set; }
    }
}
