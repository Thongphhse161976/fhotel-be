using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Payments
{
    public class PaymentRequest
    {
        public Guid? ReservationId { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public string? PaymentStatus { get; set; }
     
    }
}
