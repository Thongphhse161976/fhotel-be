using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.LateCheckOutCharges
{
    public class LateCheckOutChargeResponse
    {
        public Guid LateCheckOutChargeId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? LateCheckOutPolicyId { get; set; }
        public decimal? ChargeAmount { get; set; }
        public DateTime? ChargeDate { get; set; }

        public virtual LateCheckOutPolicy? LateCheckOutPolicy { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual Room? Room { get; set; }
    }
}
