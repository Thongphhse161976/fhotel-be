using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Bills
{
    public class BillRequest
    {
        public Guid? ReservationId { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? BillStatus { get; set; }
    }
}
