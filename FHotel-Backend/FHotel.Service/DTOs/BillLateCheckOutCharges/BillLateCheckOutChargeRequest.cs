using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillLateCheckOutCharges
{
    public class BillLateCheckOutChargeRequest
    {
        public Guid? BillId { get; set; }
        public Guid? LateCheckOutChargeId { get; set; }
        public decimal? Amount { get; set; }
    }
}
