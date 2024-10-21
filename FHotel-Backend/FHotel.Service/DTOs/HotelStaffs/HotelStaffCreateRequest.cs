using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelStaffs
{
    public class HotelStaffCreateRequest
    {
        public Guid? HotelId { get; set; }
        public Guid UserId { get; set; } // The user ID of the staff member
    }

}
