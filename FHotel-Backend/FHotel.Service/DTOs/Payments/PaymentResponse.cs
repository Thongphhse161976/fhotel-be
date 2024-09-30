using FHotel.Repository.Models;
using FHotel.Services.DTOs.PaymentMethods;
using FHotel.Services.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Payments
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public string? PaymentStatus { get; set; }

        public virtual PaymentMethodResponse? PaymentMethod { get; set; }
        public virtual ReservationResponse? Reservation { get; set; }
    }
}
