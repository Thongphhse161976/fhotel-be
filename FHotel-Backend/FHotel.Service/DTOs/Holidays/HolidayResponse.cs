using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Holidays
{
    public class HolidayResponse
    {
        public Guid HolidayId { get; set; }
        public DateTime? HolidayDate { get; set; }
        public string? Description { get; set; }
    }
}
