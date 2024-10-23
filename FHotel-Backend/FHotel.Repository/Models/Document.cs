using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Document
    {
        public Document()
        {
            HotelDocuments = new HashSet<HotelDocument>();
        }

        public Guid DocumentId { get; set; }
        public string? DocumentName { get; set; }

        public virtual ICollection<HotelDocument> HotelDocuments { get; set; }
        public virtual ICollection<UserDocument> UserDocuments { get; set; }

    }
}
