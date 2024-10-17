using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Amenities
{
    public class AmenityResponse
    {
        public Guid AmenityId { get; set; }
        public string? AmenityName { get; set; }
        public string? Image { get; set; }
    }
}
