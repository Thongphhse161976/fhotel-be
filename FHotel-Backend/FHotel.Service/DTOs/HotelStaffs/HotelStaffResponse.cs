using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelStaffs
{
    public class HotelStaffResponse
    {
        public Guid HotelStaffId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? UserId { get; set; }

        public virtual HotelResponse? Hotel { get; set; }
        public virtual UserResponse? User { get; set; }
    }
}
