using FHotel.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Wallets
{
    public class WalletResponse
    {
        public Guid WalletId { get; set; }
        public Guid? UserId { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual User? User { get; set; }
    }
}
