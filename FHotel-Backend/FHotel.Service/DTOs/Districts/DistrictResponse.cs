using FHotel.Repository.Models;
using FHotel.Services.DTOs.Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Districts
{
    public class DistrictResponse
    {
        public Guid DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public Guid? CityId { get; set; }

        public virtual CityResponse? City { get; set; }
    }
}
