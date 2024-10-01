using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            HotelAmenities = new HashSet<HotelAmenity>();
            RoomTypes = new HashSet<RoomType>();
        }

        public Guid HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? Star { get; set; }
        public Guid? CityId { get; set; }
        public Guid? OwnerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual City? City { get; set; }
        public virtual User? Owner { get; set; }
        public virtual ICollection<HotelAmenity> HotelAmenities { get; set; }
        public virtual ICollection<RoomType> RoomTypes { get; set; }
    }
}
