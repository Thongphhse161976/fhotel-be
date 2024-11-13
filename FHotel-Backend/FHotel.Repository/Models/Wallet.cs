using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid WalletId { get; set; }
        public Guid? UserId { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public decimal? Balance { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
