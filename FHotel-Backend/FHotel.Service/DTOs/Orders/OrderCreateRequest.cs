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
        public Guid? ReservationId { get; set; }
        public Guid? BillId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? OrderStatus { get; set; }
    }
}
