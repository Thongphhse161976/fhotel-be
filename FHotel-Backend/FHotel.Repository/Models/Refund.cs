﻿using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Refund
    {
        public Guid RefundId { get; set; }
        public Guid? ReservationId { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? RefundStatus { get; set; }
        public DateTime? RefundDate { get; set; }
        public string? PaymentAccountInformation { get; set; }
        public Guid? RefundPolicyId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual RefundPolicy? RefundPolicy { get; set; }
        public virtual Reservation? Reservation { get; set; }
    }
}
