using FHotel.Repository.Models;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.RoomFacilities;
using FHotel.Services.DTOs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.OrderDetails
{
    public class OrderDetailResponse
    {
        public Guid OrderDetailId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ServiceId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }

        public virtual ServiceResponse? Service { get; set; }
        public virtual OrderResponse? Order { get; set; }
    }
}
