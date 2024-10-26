using FHotel.Repository.Models;
using FHotel.Services.DTOs.Users;
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
        public int? BankAccountNumber { get; set; }
        public string? BankName { get; set; }

        public virtual UserResponse? User { get; set; }
    }
}
