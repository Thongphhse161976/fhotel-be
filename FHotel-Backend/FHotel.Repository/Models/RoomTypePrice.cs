using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class RoomTypePrice
    {
        public Guid RoomTypePriceId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public string? DayOfWeek { get; set; }
        public decimal? PercentageIncrease { get; set; }

        public virtual RoomType? RoomType { get; set; }
    }
}
