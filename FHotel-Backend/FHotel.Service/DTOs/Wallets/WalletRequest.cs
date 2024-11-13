using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Wallets
{
    public class WalletRequest
    {
        public Guid? UserId { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public decimal? Balance { get; set; }
    }
}
