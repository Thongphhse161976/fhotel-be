using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class BillLateCheckOutCharge
    {
        public Guid BillLateCheckOutChargeId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? LateCheckOutChargeId { get; set; }
        public decimal? Amount { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual LateCheckOutCharge? LateCheckOutCharge { get; set; }
    }
}
