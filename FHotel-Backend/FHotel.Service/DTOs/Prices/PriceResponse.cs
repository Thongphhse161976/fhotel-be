using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Prices
{
    public class PriceResponse
    {
        public Guid PriceId { get; set; }
        public Guid? HotelId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
   
    }
}
