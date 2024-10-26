using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
        }

        public Guid CityId { get; set; }
        public string? CityName { get; set; }
        public string? PostalCode { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
