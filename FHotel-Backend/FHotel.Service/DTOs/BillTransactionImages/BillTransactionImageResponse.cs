using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.BillTransactionImages
{
    public class BillTransactionImageResponse
    {
        public Guid BillTransactionImageId { get; set; }
        public Guid? BillId { get; set; }
        public string? Image { get; set; }
        public DateTime? UploadedDate { get; set; }

        public virtual BillResponse? Bill { get; set; }
    }
}
