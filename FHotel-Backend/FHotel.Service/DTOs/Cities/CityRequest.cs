using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Cities
{
    public class CityRequest
    {
        public string? CityName { get; set; }
        public string? PostalCode { get; set; }
    }
}
