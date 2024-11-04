using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Holidays
{
    public class HolidayRequest
    {
        public DateTime? HolidayDate { get; set; }
        public string? Description { get; set; }
    }
}
