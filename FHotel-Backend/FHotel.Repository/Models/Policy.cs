using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Policy
    {
        public Policy()
        {
            HotelPolicies = new HashSet<HotelPolicy>();
        }

        public Guid PolicyId { get; set; }
        public string? PolicyName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<HotelPolicy> HotelPolicies { get; set; }
    }
}
