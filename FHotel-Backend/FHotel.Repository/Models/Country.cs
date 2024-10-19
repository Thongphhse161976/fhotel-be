using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
