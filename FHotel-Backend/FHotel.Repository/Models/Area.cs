using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Area
    {
        public Area()
        {
            TypePricings = new HashSet<TypePricing>();
        }

        public Guid AreaId { get; set; }
        public string? AreaName { get; set; }
        public Guid? DistrictId { get; set; }
        public string? AddressPattern { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual District District { get; set; } = null!;
        public virtual ICollection<TypePricing> TypePricings { get; set; }
    }
}
