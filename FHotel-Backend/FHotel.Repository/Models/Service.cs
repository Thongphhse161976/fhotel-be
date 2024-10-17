using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Service
    {
        public Service()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ServiceType? ServiceType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
