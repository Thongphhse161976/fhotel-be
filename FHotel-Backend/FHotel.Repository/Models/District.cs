using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class District
    {
        public District()
        {
            Hotels = new HashSet<Hotel>();
        }

        public Guid DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public Guid? CityId { get; set; }

        public virtual City? City { get; set; }
        public virtual Area? Area { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
