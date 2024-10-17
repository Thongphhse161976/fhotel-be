﻿using FHotel.Repository.Models;
using FHotel.Services.DTOs.RoomTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.RoomTypePrices
{
    public class RoomTypePriceResponse
    {
        public Guid RoomTypePriceId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? DayOfWeek { get; set; }
        public decimal? PercentageIncrease { get; set; }

        public virtual RoomTypeResponse? RoomType { get; set; }
    }
}
