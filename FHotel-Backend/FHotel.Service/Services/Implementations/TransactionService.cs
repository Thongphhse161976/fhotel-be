using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Payments;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TransactionResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Transaction>().GetAll()
                                            .ProjectTo<TransactionResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            if (list == null)
            {
                throw new Exception("Transaction not found");
            }
            return list;
        }

        public async Task<TransactionResponse> Get(Guid id)
        {
            try
            {
                Transaction transaction = null;
                transaction = await _unitOfWork.Repository<Transaction>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.TransactionId == id)
                    .FirstOrDefaultAsync();

                if (transaction == null)
                {
                    throw new Exception("Transaction not found");
                }

                return _mapper.Map<Transaction, TransactionResponse>(transaction);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionResponse> Create(TransactionRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var transaction = _mapper.Map<TransactionRequest, Transaction>(request);
                transaction.TransactionId = Guid.NewGuid();
                transaction.TransactionDate = localTime;
                await _unitOfWork.Repository<Transaction>().InsertAsync(transaction);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Transaction, TransactionResponse>(transaction);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TransactionResponse> Delete(Guid id)
        {
            try
            {
                Transaction transaction = null;
                transaction = _unitOfWork.Repository<Transaction>()
                    .Find(p => p.TransactionId == id);
                if (transaction == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Transaction>().HardDeleteGuid(transaction.TransactionId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Transaction, TransactionResponse>(transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TransactionResponse> Update(Guid id, TransactionRequest request)
        {
            try
            {
                Transaction transaction = _unitOfWork.Repository<Transaction>()
                            .Find(x => x.TransactionId == id);
                if (transaction == null)
                {
                    throw new Exception("Not Found");
                }
                transaction = _mapper.Map(request, transaction);

                await _unitOfWork.Repository<Transaction>().UpdateDetached(transaction);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Transaction, TransactionResponse>(transaction);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

      

        public async Task<List<TransactionResponse>> GetAllTransactionByWalletId(Guid id)
        {

            var list = await _unitOfWork.Repository<Transaction>().GetAll()
                                            .ProjectTo<TransactionResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.WalletId == id)
                                            .ToListAsync();
            return list;
        }


        public async Task<List<TransactionResponse>> GetAllTransactionByBillId(Guid id)
        {

            var list = await _unitOfWork.Repository<Transaction>().GetAll()
                                            .ProjectTo<TransactionResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.BillId == id)
                                            .ToListAsync();
            return list;
        }

        public async Task<TransactionResponse> GetTransactionByWalletAndBillId(Guid walletId, Guid billId)
        {
            return await _unitOfWork.Repository<Transaction>().GetAll()
                 .ProjectTo<TransactionResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.WalletId == walletId && t.BillId == billId);
        }

    }
}
