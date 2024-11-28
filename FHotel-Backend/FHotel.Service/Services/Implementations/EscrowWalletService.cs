using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.EscrowWallets;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class EscrowWalletService : IEscrowWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public EscrowWalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            try
            {
                EscrowWallet escrowWallet = _unitOfWork.Repository<EscrowWallet>()
                            .Find(x => x.EscrowWalletId == id);
                if (escrowWallet == null)
                {
                    throw new Exception();
                }
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
    }
}
