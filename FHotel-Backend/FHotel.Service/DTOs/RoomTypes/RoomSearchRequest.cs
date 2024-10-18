using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomTypes
{
    public class RoomSearchRequest
    {
        public string RoomTypeName { get; set; }   // e.g., Single, Double
        public int Quantity { get; set; }          // e.g., 2 rooms, 1 room
    }

}
