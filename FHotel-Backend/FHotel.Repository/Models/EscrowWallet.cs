using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class EscrowWallet
    {
        public EscrowWallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public Guid EscrowWalletId { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
