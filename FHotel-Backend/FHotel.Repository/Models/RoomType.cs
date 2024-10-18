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
            Rooms = new HashSet<Room>();
        }

        public Guid RoomTypeId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? TypeId { get; set; }
        public string? Description { get; set; }
        public decimal? RoomSize { get; set; }
        public int? TotalRooms { get; set; }
        public int? AvailableRooms { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Type? Type { get; set; }
        public virtual ReservationDetail? ReservationDetail { get; set; }
        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }
        public virtual ICollection<RoomImage> RoomImages { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
