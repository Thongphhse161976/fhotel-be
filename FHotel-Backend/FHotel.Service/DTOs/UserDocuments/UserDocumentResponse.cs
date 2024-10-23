using FHotel.Repository.Models;
using FHotel.Services.DTOs.Documents;
using FHotel.Services.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.UserDocuments
{
    public class UserDocumentResponse
    {
        public Guid? UserDocumentId { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? ReservationId { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual DocumentResponse? Document { get; set; }
        public virtual ReservationResponse? Reservation { get; set; }
    }
}
