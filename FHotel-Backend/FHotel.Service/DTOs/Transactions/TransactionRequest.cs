using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Transactions
{
    public class TransactionRequest
    {
        public Guid? BillId { get; set; }
        public Guid? WalletId { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
