using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.HotelRegistations
{
    public class HotelRegistrationResponse
    {
        public Guid HotelRegistrationId { get; set; }
        public Guid? OwnerId { get; set; }
        public int? NumberOfHotels { get; set; }
        public string? Description { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RegistrationStatus { get; set; }

        public virtual User? Owner { get; set; }
    }
}
