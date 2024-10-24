using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Reservations = new HashSet<Reservation>();
        }

        public Guid PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
