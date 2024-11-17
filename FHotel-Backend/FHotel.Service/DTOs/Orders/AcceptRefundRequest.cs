using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Orders
{
    public class AcceptRefundRequest
    {
        public Guid OrderId { get; set; }
    }
}
