using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Hotels
{
    public class HotelUpdateRequest
    {
        public Guid HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerEmail { get; set; }
        public string? OwnerIdentificationNumber { get; set; }
        public string? OwnerPhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? OwnerId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? VerifyStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
