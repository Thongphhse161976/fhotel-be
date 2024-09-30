using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Services
{
    public class ServiceRequest
    {
    
        public string? ServiceName { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public Guid? ServiceTypeId { get; set; }

     
    }
}
