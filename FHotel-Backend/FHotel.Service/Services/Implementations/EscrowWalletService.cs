using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.EscrowWallets;
using FHotel.Service.DTOs.Transactions;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class EscrowWalletService : IEscrowWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReservationService _reservationService;
        private readonly ITransactionService _transactionService;
        private IMapper _mapper;
        public EscrowWalletService(IUnitOfWork unitOfWork, IMapper mapper, IReservationService reservationService, ITransactionService transactionService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reservationService = reservationService;
            _transactionService = transactionService;
        }

        public async Task<List<EscrowWalletResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<EscrowWallet>().GetAll()
                                            .ProjectTo<EscrowWalletResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<EscrowWalletResponse> Get(Guid id)
        {
            try
            {
                EscrowWallet escrowWallet = null;
                escrowWallet = await _unitOfWork.Repository<EscrowWallet>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.EscrowWalletId == id)
                    .FirstOrDefaultAsync();

                if (escrowWallet == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<EscrowWallet, EscrowWalletResponse>(escrowWallet);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<EscrowWalletResponse> Create(EscrowWalletRequest request)
        {
            try
            {
                var escrowWallet = _mapper.Map<EscrowWalletRequest, EscrowWallet>(request);
                escrowWallet.EscrowWalletId = Guid.NewGuid();
                await _unitOfWork.Repository<EscrowWallet>().InsertAsync(escrowWallet);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<EscrowWallet, EscrowWalletResponse>(escrowWallet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<EscrowWalletResponse> Delete(Guid id)
        {
            try
            {
                EscrowWallet escrowWallet = null;
                escrowWallet = _unitOfWork.Repository<EscrowWallet>()
                    .Find(p => p.EscrowWalletId == id);
                if (escrowWallet == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<EscrowWallet>().HardDeleteGuid(escrowWallet.EscrowWalletId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<EscrowWallet, EscrowWalletResponse>(escrowWallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EscrowWalletResponse> Update(Guid id, EscrowWalletRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {

                EscrowWallet escrowWallet = _unitOfWork.Repository<EscrowWallet>()
                            .Find(x => x.EscrowWalletId == id);
                if (escrowWallet == null)
                {
                    throw new Exception();
                }
                escrowWallet.UpdatedDate = localTime;
                escrowWallet = _mapper.Map(request, escrowWallet);

                await _unitOfWork.Repository<EscrowWallet>().UpdateDetached(escrowWallet);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<EscrowWallet, EscrowWalletResponse>(escrowWallet);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


public async Task<EscrowWalletResponse> IncreaseBalance(Guid reservationId, decimal amount)
    {
        // Set the UTC offset for UTC+7
        TimeSpan utcOffset = TimeSpan.FromHours(7);

        // Get the current UTC time
        DateTime utcNow = DateTime.UtcNow;

        // Convert the UTC time to UTC+7
        DateTime localTime = utcNow + utcOffset;

        try
        {
            var escrowWallets = await GetAll();
            var escrowWalletId = escrowWallets.FirstOrDefault()?.EscrowWalletId;

            // Check if no wallet is found
            if (escrowWalletId == null)
            {
                throw new Exception("No escrow wallet found.");
            }

            var escrowWalletResponse = await Get(escrowWalletId.Value);
            var reservationResponse = await _reservationService.Get(reservationId);

            // Create transaction object
            var createTransaction = new TransactionRequest()
            {
                Amount = amount,
                EscrowWalletId = escrowWalletId.Value,
                TransactionDate = localTime,
                Description = $@"Nhận {amount:F0}₫ từ đặt phòng {reservationResponse.Code} lúc {localTime}",
            };

                // Perform the transaction creation
                await _transactionService.Create(createTransaction);

            // Update escrow wallet
            var updateEscrowWallet = new EscrowWalletRequest()
            {
                Balance = escrowWalletResponse.Balance + amount,
                CreatedDate = escrowWalletResponse.CreatedDate,
                UpdatedDate = localTime
            };
            var escrowWalletResponseUpdate = await Update(escrowWalletId.Value, updateEscrowWallet);
             return escrowWalletResponseUpdate;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the request: " + ex.Message);
        }
    }
        public async Task<EscrowWalletResponse> DescreaseBalance(Guid reservationId, decimal amount)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;

            try
            {
                var escrowWallets = await GetAll();
                var escrowWalletId = escrowWallets.FirstOrDefault()?.EscrowWalletId;

                // Check if no wallet is found
                if (escrowWalletId == null)
                {
                    throw new Exception("No escrow wallet found.");
                }

                var escrowWalletResponse = await Get(escrowWalletId.Value);
                var reservationResponse = await _reservationService.Get(reservationId);

                // Create transaction object
                var createTransaction = new TransactionRequest()
                {
                    Amount = amount,
                    EscrowWalletId = escrowWalletId.Value,
                    TransactionDate = localTime,
                    Description = $@"Trừ {amount:F0}₫ từ đặt phòng {reservationResponse.Code} lúc {localTime}",
                };

                // Perform the transaction creation
                await _transactionService.Create(createTransaction);

                // Update escrow wallet
                var updateEscrowWallet = new EscrowWalletRequest()
                {
                    Balance = escrowWalletResponse.Balance - amount,
                    CreatedDate = escrowWalletResponse.CreatedDate,
                    UpdatedDate = localTime
                };
                var escrowWalletResponseUpdate = await Update(escrowWalletId.Value, updateEscrowWallet);
                return escrowWalletResponseUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request: " + ex.Message);
            }
        }

    }
}
