using FHotel.Service.DTOs.EscrowWallets;
using FHotel.Service.DTOs.Hotels;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IEscrowWalletService
    {
        public Task<List<EscrowWalletResponse>> GetAll();

        public Task<EscrowWalletResponse> Get(Guid id);

        public Task<EscrowWalletResponse> Create(EscrowWalletRequest request);

        public Task<EscrowWalletResponse> Delete(Guid id);

        public Task<EscrowWalletResponse> Update(Guid id, EscrowWalletRequest request);
    }
}
