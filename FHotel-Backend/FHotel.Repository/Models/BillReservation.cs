using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class BillReservation
    {
        public Guid BillReservationId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual Reservation? Reservation { get; set; }
    }
}
