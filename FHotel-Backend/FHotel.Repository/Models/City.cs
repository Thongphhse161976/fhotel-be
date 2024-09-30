using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class City
    {
        public City()
        {
            Hotels = new HashSet<Hotel>();
        }

        public Guid CityId { get; set; }
        public string? CityName { get; set; }
        public string? PostalCode { get; set; }
        public Guid? CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
