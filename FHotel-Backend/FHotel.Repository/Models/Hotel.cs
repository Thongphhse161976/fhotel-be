using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            HotelAmenities = new HashSet<HotelAmenity>();
            HotelDocuments = new HashSet<HotelDocument>();
            HotelImages = new HashSet<HotelImage>();
            HotelStaffs = new HashSet<HotelStaff>();
            HotelVerifications = new HashSet<HotelVerification>();
            RoomTypes = new HashSet<RoomType>();
        }

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
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? VerifyStatus { get; set; }
        public bool? IsActive { get; set; }

        public virtual District? District { get; set; }
        public virtual User? Owner { get; set; }
        public virtual ICollection<HotelAmenity> HotelAmenities { get; set; }
        public virtual ICollection<HotelDocument> HotelDocuments { get; set; }
        public virtual ICollection<HotelImage> HotelImages { get; set; }
        public virtual ICollection<HotelStaff> HotelStaffs { get; set; }
        public virtual ICollection<HotelVerification> HotelVerifications { get; set; }
        public virtual ICollection<RoomType> RoomTypes { get; set; }
    }
}
