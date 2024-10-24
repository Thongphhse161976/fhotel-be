using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.VnPayConfigs
{
    public class VnPaymentRequestModel
    {
        public Guid OrderId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
