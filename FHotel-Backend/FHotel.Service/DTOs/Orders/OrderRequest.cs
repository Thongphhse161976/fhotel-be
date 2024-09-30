using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Orders
{
    public class OrderRequest
    {
        public Guid? ReservationId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string? OrderStatus { get; set; }

    }
}
