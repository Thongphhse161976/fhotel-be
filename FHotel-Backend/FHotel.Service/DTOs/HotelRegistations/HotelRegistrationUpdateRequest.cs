using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelRegistations
{
    public class HotelRegistrationUpdateRequest
    {
        public Guid HotelRegistrationId { get; set; }
        public Guid? OwnerId { get; set; }
        public int? NumberOfHotels { get; set; }
        public string? Description { get; set; }
        public string? RegistrationStatus { get; set; }
    }
}
