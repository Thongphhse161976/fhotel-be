using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class WalletHistory
    {
        public Guid WalletHistoryId { get; set; }
        public Guid? WalletId { get; set; }
        public decimal? Note { get; set; }
        public DateTime? TransactionDate { get; set; }

        public virtual Wallet? Wallet { get; set; }
    }
}
