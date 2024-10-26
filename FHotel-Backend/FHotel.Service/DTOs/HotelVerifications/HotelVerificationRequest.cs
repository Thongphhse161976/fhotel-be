using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.HotelVerifications
{
    public class HotelVerificationRequest
    {
        public Guid? HotelId { get; set; }
        public Guid? AssignedManagerId { get; set; }
        public string? VerificationStatus { get; set; }
        public DateTime? VerificationDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
