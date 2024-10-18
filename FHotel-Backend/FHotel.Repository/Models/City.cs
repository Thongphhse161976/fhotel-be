using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
            TypePricings = new HashSet<TypePricing>();
        }

        public Guid CityId { get; set; }
        public string? CityName { get; set; }
        public string? PostalCode { get; set; }
        public Guid? CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<TypePricing> TypePricings { get; set; }
    }
}
