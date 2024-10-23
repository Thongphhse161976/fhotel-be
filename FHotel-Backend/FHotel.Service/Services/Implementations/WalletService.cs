using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Wallets;
using FHotel.Service.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class WalletService: IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WalletResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Wallet>().GetAll()
                                            .ProjectTo<WalletResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<WalletResponse> Get(Guid id)
        {
            try
            {
                Wallet wallet = null;
                wallet = await _unitOfWork.Repository<Wallet>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.WalletId == id)
                    .FirstOrDefaultAsync();

                if (wallet == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Wallet, WalletResponse>(wallet);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<WalletResponse> Create(WalletRequest request)
        {
            try
            {
                var wallet = _mapper.Map<WalletRequest, Wallet>(request);
                wallet.WalletId = Guid.NewGuid();
                if (wallet.UserId == Guid.Empty)
                {
                    throw new ValidationException("UserId cannot be empty. Please provide a valid UserId.");
                }
                var userExists = await _unitOfWork.Repository<User>().FindAsync(u => u.UserId == wallet.UserId);
                if (userExists == null)
                {
                    throw new ValidationException("The specified user does not exist.");
                }
                await _unitOfWork.Repository<Wallet>().InsertAsync(wallet);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Wallet, WalletResponse>(wallet);
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific exceptions
                var innerMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
            catch (ValidationException vEx)
            {
                // Handle validation-specific exceptions
                throw new Exception(vEx.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<WalletResponse> Delete(Guid id)
        {
            try
            {
                Wallet wallet = null;
                wallet = _unitOfWork.Repository<Wallet>()
                    .Find(p => p.WalletId == id);
                if (wallet == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Wallet>().HardDeleteGuid(wallet.WalletId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Wallet, WalletResponse>(wallet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WalletResponse> Update(Guid id, WalletRequest request)
        {
            try
            {
                Wallet wallet = _unitOfWork.Repository<Wallet>()
                            .Find(x => x.WalletId == id);
                if (wallet == null)
                {
                    throw new Exception();
                }
                wallet = _mapper.Map(request, wallet);

                await _unitOfWork.Repository<Wallet>().UpdateDetached(wallet);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Wallet, WalletResponse>(wallet);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
