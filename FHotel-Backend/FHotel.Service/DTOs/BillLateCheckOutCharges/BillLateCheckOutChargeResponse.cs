using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillLateCheckOutCharges
{
    public class BillLateCheckOutChargeResponse
    {
        public Guid BillLateCheckOutChargeId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? LateCheckOutChargeId { get; set; }
        public decimal? Amount { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual LateCheckOutCharge? LateCheckOutCharge { get; set; }
    }
}
