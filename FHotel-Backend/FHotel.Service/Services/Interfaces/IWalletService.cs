using FHotel.Service.DTOs.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IWalletService
    {
        public Task<List<WalletResponse>> GetAll();

        public Task<WalletResponse> Get(Guid id);

        public Task<WalletResponse> Create(WalletRequest request);

        public Task<WalletResponse> Delete(Guid id);

        public Task<WalletResponse> Update(Guid id, WalletRequest request);

        public Task<WalletResponse> GetWalletByUser(Guid id);
    }
}
