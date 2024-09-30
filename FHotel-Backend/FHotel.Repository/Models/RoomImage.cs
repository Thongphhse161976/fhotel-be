using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomImage
    {
        public Guid RoomImageId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? Image { get; set; }

        public virtual RoomType? RoomType { get; set; }
    }
}
