using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Districts
{
    public class DistrictRequest
    {
        public string? DistrictName { get; set; }
        public Guid? CityId { get; set; }
    }
}
