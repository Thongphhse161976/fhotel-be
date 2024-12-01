﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.DTOs.EscrowWallets
{
    public class EscrowWalletResponse
    {
        public Guid EscrowWalletId { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}