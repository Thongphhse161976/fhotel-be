using FHotel.Repository.Models;
using FHotel.Services.DTOs.PaymentMethods;
using FHotel.Services.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Orders
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? OrderStatus { get; set; }

        public virtual ReservationResponse? Reservation { get; set; }
    }
}
