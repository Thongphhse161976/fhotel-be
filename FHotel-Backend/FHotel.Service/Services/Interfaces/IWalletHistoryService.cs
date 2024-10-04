using FHotel.Service.DTOs.WalletHistories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IWalletHistoryService
    {
        public Task<List<WalletHistoryResponse>> GetAll();

        public Task<WalletHistoryResponse> Get(Guid id);

        public Task<WalletHistoryResponse> Create(WalletHistoryRequest request);

        public Task<WalletHistoryResponse> Delete(Guid id);

        public Task<WalletHistoryResponse> Update(Guid id, WalletHistoryRequest request);
    }
}
