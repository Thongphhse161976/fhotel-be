using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillOrders
{
    public class BillOrderRequest
    {
        public Guid? BillId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal? Amount { get; set; }
    }
}
