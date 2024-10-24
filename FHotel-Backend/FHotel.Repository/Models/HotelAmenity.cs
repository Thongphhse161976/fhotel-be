﻿using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelAmenity
    {
        public Guid HotelAmenityId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? AmenityId { get; set; }

        public virtual Amenity? Amenity { get; set; }
        public virtual Hotel? Hotel { get; set; }
    }
}
