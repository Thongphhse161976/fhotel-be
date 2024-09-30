using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Document
    {
        public Guid DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
