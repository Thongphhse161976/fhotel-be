using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class WalletHistory
    {
        public Guid WalletHistoryId { get; set; }
        public Guid? WalletId { get; set; }
        public decimal? Note { get; set; }
        public decimal? Amount { get; set; }
        public Guid? Payee { get; set; }
        public Guid? Payer { get; set; }
        public DateTime? TransactionDate { get; set; }

        public virtual User? PayeeNavigation { get; set; }
        public virtual User? PayerNavigation { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
