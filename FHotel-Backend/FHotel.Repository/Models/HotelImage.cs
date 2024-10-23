using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelImage
    {
        public Guid HotelImageId { get; set; }
        public Guid? HotelId { get; set; }
        public string? Image { get; set; }

        public virtual Hotel? Hotel { get; set; }
    }
}
