using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Amenity
    {
        public Amenity()
        {
            HotelAmenities = new HashSet<HotelAmenity>();
        }

        public Guid AmenityId { get; set; }
        public string? AmenityName { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<HotelAmenity> HotelAmenities { get; set; }
    }
}
