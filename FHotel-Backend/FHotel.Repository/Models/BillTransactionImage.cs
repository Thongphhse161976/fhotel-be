using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class BillTransactionImage
    {
        public Guid BillTransactionImageId { get; set; }
        public Guid? BillId { get; set; }
        public string? Image { get; set; }
        public DateTime? UploadedDate { get; set; }

        public virtual Bill? Bill { get; set; }
    }
}
