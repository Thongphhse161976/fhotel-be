using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Services.DTOs.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Payments
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual BillResponse? Bill { get; set; }
        public virtual PaymentMethodResponse? PaymentMethod { get; set; }
    }
}
