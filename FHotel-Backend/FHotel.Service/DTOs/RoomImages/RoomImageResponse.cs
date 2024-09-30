using FHotel.Repository.Models;
using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomImages
{
    public class RoomImageResponse
    {
        public Guid RoomImageId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? Image { get; set; }

        public virtual RoomTypeResponse? RoomType { get; set; }
    }
}
