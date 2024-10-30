using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.OrderDetails
{
    public class OrderDetailRequest
    {
        public Guid? OrderId { get; set; }
        public Guid? ServiceId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }

    }
}
