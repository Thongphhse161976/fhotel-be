using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelVerification
    {
        public Guid HotelVerificationId { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? AssignedManagerId { get; set; }
        public string? VerificationStatus { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual User? AssignedManager { get; set; }
        public virtual Hotel? Hotel { get; set; }
    }
}
