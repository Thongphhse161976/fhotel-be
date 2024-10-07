using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelRegistations
{
    public class HotelRegistrationCreateRequest
    {
        public int? NumberOfHotels { get; set; }
        public string? Description { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RegistrationStatus { get; set; }
    }
}
