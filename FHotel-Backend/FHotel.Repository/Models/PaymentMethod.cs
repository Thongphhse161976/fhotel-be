using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Bills = new HashSet<Bill>();
            Reservations = new HashSet<Reservation>();
        }

        public Guid PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
