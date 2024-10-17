using FHotel.Repository.Models;
using FHotel.Services.DTOs.ServiceTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Services
{
    public class ServiceResponse
    {
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public string? Image { get; set; }
        public bool? IsActive { get; set; }

        public virtual ServiceTypeResponse? ServiceType { get; set; }
    }
}
