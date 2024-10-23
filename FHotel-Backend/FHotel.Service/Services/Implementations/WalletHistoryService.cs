using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.WalletHistories;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class WalletHistoryService: IWalletHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public WalletHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WalletHistoryResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<WalletHistory>().GetAll()
                                            .ProjectTo<WalletHistoryResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<WalletHistoryResponse> Get(Guid id)
        {
            try
            {
                WalletHistory walletHistory = null;
                walletHistory = await _unitOfWork.Repository<WalletHistory>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.WalletHistoryId == id)
                    .FirstOrDefaultAsync();

                if (walletHistory == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<WalletHistory, WalletHistoryResponse>(walletHistory);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<WalletHistoryResponse> Create(WalletHistoryRequest request)
        {
            try
            {
                var walletHistory = _mapper.Map<WalletHistoryRequest, WalletHistory>(request);
                walletHistory.WalletHistoryId = Guid.NewGuid();
                await _unitOfWork.Repository<WalletHistory>().InsertAsync(walletHistory);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<WalletHistory, WalletHistoryResponse>(walletHistory);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<WalletHistoryResponse> Delete(Guid id)
        {
            try
            {
                WalletHistory walletHistory = null;
                walletHistory = _unitOfWork.Repository<WalletHistory>()
                    .Find(p => p.WalletHistoryId == id);
                if (walletHistory == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<WalletHistory>().HardDeleteGuid(walletHistory.WalletHistoryId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<WalletHistory, WalletHistoryResponse>(walletHistory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WalletHistoryResponse> Update(Guid id, WalletHistoryRequest request)
        {
            try
            {
                WalletHistory walletHistory = _unitOfWork.Repository<WalletHistory>()
                            .Find(x => x.WalletHistoryId == id);
                if (walletHistory == null)
                {
                    throw new Exception();
                }
                walletHistory = _mapper.Map(request, walletHistory);

                await _unitOfWork.Repository<WalletHistory>().UpdateDetached(walletHistory);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<WalletHistory, WalletHistoryResponse>(walletHistory);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
