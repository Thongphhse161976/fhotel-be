using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.HotelAmenities
{
    public class HotelAmenityResponse
    {
        public Guid HotelAmenityId { get; set; }
        public Guid? HotelId { get; set; }
        public string? Image { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
    }
}
