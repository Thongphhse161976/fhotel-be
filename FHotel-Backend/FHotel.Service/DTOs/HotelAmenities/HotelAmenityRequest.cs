using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.HotelAmenities
{
    public class HotelAmenityRequest
    {
        public Guid? HotelId { get; set; }
        public string? Image { get; set; }

    }
}
