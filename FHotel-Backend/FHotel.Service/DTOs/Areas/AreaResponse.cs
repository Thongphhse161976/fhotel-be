using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Areas
{
    public class AreaResponse
    {
        public Guid AreaId { get; set; }
        public string? AreaName { get; set; }
        public Guid? DistrictId { get; set; }
        public string? AddressPattern { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual DistrictResponse District { get; set; } = null!;
    }
}
