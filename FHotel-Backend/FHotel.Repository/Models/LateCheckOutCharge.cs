using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class LateCheckOutCharge
    {
        public LateCheckOutCharge()
        {
            BillLateCheckOutCharges = new HashSet<BillLateCheckOutCharge>();
        }

        public Guid LateCheckOutChargeId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? LateCheckOutPolicyId { get; set; }
        public decimal? ChargeAmount { get; set; }
        public DateTime? ChargeDate { get; set; }

        public virtual LateCheckOutPolicy? LateCheckOutPolicy { get; set; }
        public virtual Reservation? Reservation { get; set; }
        public virtual Room? Room { get; set; }
        public virtual ICollection<BillLateCheckOutCharge> BillLateCheckOutCharges { get; set; }
    }
}
