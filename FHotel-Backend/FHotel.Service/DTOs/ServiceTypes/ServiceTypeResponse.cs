using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.ServiceTypes
{
    public class ServiceTypeResponse
    {
        public Guid ServiceTypeId { get; set; }
        public string? ServiceTypeName { get; set; }
        public bool? IsVisibleToCustomer { get; set; }

    }
}
