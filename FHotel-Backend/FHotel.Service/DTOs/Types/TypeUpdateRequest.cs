using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Types
{
    public class TypeUpdateRequest
    {
        public string? TypeName { get; set; }
        public int? MaxOccupancy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
