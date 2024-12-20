﻿using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.RoomTypes
{
    public class RoomSearchRequest
    {

        public Guid TypeId { get; set; }   // e.g., Single, Double
        public int Quantity { get; set; }          // e.g., 2 rooms, 1 room
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }


}
