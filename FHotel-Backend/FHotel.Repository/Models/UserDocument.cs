using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class UserDocument
    {
        public Guid UserDocumentId { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? ReservationId { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Document? Document { get; set; }
        public virtual Reservation? Reservation { get; set; }
    }
}
