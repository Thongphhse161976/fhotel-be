using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.PaymentMethods
{
    public class PaymentMethodResponse
    {
        public Guid PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }

    }
}
