using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.ServiceTypes
{
    public class ServiceTypeRequest
    {
        public string? ServiceTypeName { get; set; }
        public bool? IsVisibleToCustomer { get; set; }

    }
}
