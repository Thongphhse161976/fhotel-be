﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Reservations
{
    public class ReservationCreateRequest
    {
        public string? Code { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ReservationStatus { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int? NumberOfRooms { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public string? PaymentStatus { get; set; }
        public bool? IsPrePaid { get; set; }
        public Guid? PaymentMethodId { get; set; }
    }
}
