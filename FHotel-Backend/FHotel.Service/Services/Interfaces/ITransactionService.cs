using FHotel.Service.DTOs.Amenities;
using FHotel.Service.DTOs.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface ITransactionService
    {
        public Task<List<TransactionResponse>> GetAll();

        public Task<TransactionResponse> Get(Guid id);

        public Task<TransactionResponse> Create(TransactionRequest request);

        public Task<TransactionResponse> Delete(Guid id);

        public Task<TransactionResponse> Update(Guid id, TransactionRequest request);

        public Task<List<TransactionResponse>> GetAllTransactionByWalletId(Guid id);
        public Task<List<TransactionResponse>> GetAllTransactionByBillId(Guid id);
        public Task<TransactionResponse> GetTransactionByWalletAndBillId(Guid walletId, Guid billId);
        public Task<List<TransactionResponse>> GetAllTransactionByEscrowWallet();
    }
}
