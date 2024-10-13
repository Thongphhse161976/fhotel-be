using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelStaff
    {
        public Guid HotelStaffId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? UserId { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual User? User { get; set; }
    }
}
