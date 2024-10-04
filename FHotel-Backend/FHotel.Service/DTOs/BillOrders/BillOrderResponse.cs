using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillOrders
{
    public class BillOrderResponse
    {
        public Guid BillOrderId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal? Amount { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual Order? Order { get; set; }
    }
}
