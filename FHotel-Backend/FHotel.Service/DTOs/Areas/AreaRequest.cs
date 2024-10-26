using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Areas
{
    public class AreaRequest
    {
        public string? AreaName { get; set; }
        public Guid? DistrictId { get; set; }
        public string? AddressPattern { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
