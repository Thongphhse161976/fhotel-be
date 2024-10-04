using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelRegistration
    {
        public Guid HotelRegistrationId { get; set; }
        public Guid? OwnerId { get; set; }
        public int? NumberOfHotels { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? RegistrationStatus { get; set; }

        public virtual User? Owner { get; set; }
    }
}
