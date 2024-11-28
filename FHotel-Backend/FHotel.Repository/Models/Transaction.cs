using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? WalletId { get; set; }
        public Guid? EscrowWalletId { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual EscrowWallet? EscrowWallet { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
