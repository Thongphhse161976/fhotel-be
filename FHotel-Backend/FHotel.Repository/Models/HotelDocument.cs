using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class HotelDocument
    {
        public Guid HotelDocumentId { get; set; }
        public Guid? DocumentId { get; set; }
        public Guid? HotelId { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Document? Document { get; set; }
        public virtual Hotel? Hotel { get; set; }
    }
}
