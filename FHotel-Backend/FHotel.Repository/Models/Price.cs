using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Price
    {
        public Price()
        {
            RoomTypePrices = new HashSet<RoomTypePrice>();
        }

        public Guid PriceId { get; set; }
        public Guid? HotelId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<RoomTypePrice> RoomTypePrices { get; set; }
    }
}
