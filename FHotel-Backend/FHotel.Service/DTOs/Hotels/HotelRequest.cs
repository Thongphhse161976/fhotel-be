using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Hotels
{
    public class HotelRequest
    {
        public string? HotelName { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerEmail { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? Star { get; set; }
        public string? BusinessLicenseNumber { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? OwnerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

    }
}
