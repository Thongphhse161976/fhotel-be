using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillTransactionImages
{
    public class BillTransactionImageRequest
    {
        public Guid? BillId { get; set; }
        public string? Image { get; set; }
        public DateTime? UploadedDate { get; set; }
    }
}
