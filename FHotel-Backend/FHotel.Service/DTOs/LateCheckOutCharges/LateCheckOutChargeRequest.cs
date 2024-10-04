using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.LateCheckOutCharges
{
    public class LateCheckOutChargeRequest
    {
        public Guid? ReservationId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? LateCheckOutPolicyId { get; set; }
        public decimal? ChargeAmount { get; set; }
        public DateTime? ChargeDate { get; set; }
    }
}
