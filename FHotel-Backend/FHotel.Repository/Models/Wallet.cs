using System;
using System.Collections.Generic;

namespace FHotel.Repository.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            WalletHistories = new HashSet<WalletHistory>();
        }

        public Guid WalletId { get; set; }
        public Guid? UserId { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<WalletHistory> WalletHistories { get; set; }
    }
}
