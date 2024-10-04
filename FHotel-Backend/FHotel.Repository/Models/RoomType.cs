using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            RoomFacilities = new HashSet<RoomFacility>();
            RoomImages = new HashSet<RoomImage>();
            RoomTypePrices = new HashSet<RoomTypePrice>();
            Rooms = new HashSet<Room>();
        }

        public Guid RoomTypeId { get; set; }
        public Guid? HotelId { get; set; }
        public string? TypeName { get; set; }
        public string? Description { get; set; }
        public decimal? RoomSize { get; set; }
        public string? Image { get; set; }
        public decimal? BasePrice { get; set; }
        public int? MaxOccupancy { get; set; }
        public int? TotalRooms { get; set; }
        public int? AvailableRooms { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual ReservationDetail? ReservationDetail { get; set; }
        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }
        public virtual ICollection<RoomImage> RoomImages { get; set; }
        public virtual ICollection<RoomTypePrice> RoomTypePrices { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
