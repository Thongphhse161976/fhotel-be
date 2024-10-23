using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomImages
{
    public class RoomImageRequest
    {

        public Guid? RoomTypeId { get; set; }
        public string? Image { get; set; }


    }
}
