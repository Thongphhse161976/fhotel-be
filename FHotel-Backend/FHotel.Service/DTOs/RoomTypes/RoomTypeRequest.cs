using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomTypes
{
    public class RoomTypeRequest
    {
        public string TypeName { get; set; } // The name of the room type
        public int Quantity { get; set; }     // The quantity of rooms requested
    }
}
