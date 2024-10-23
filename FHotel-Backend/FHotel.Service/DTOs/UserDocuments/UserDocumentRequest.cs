using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.UserDocuments
{
    public class UserDocumentRequest
    {
        public Guid? DocumentId { get; set; }
        public Guid? ReservationId { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
