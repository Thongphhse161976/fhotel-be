using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Facilities
{
    public class FacilityResponse
    {
        public Guid FacilityId { get; set; }
        public string? FacilityName { get; set; }
    }
}
