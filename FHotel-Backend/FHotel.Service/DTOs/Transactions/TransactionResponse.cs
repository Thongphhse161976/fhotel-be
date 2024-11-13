using FHotel.Repository.Models;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.Transactions
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public Guid? BillId { get; set; }
        public Guid? WalletId { get; set; }
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }

        public virtual BillResponse? Bill { get; set; }
        public virtual WalletResponse? Wallet { get; set; }
    }
}
