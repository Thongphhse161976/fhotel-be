using FHotel.Repository.Models;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelVerifications
{
    public class HotelVerificationResponse
    {
        public Guid HotelVerificationId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? AssignedManagerId { get; set; }
        public string? VerificationStatus { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual UserResponse? AssignedManager { get; set; }
        public virtual HotelResponse? Hotel { get; set; }
    }
}
