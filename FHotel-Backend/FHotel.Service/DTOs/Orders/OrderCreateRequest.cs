using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Orders
{
    public class OrderCreateRequest
    {
        public Guid OrderId { get; set; }
        public Guid? ReservationId { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? OrderStatus { get; set; }
    }
}
