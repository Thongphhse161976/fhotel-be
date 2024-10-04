using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.WalletHistories
{
    public class WalletHistoryRequest
    {
        public Guid? WalletId { get; set; }
        public decimal? Note { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
