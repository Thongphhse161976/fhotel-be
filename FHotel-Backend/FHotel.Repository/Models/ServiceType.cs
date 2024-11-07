using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Services = new HashSet<Service>();
        }

        public Guid ServiceTypeId { get; set; }
        public string? ServiceTypeName { get; set; }
        public bool? IsVisibleToCustomer { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
