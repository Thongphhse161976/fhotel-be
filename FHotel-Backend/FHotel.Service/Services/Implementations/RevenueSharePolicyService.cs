using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.RevenueSharePolicies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class RevenueSharePolicyService : IRevenueSharePolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RevenueSharePolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RevenueSharePolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RevenueSharePolicy>().GetAll()
                                            .ProjectTo<RevenueSharePolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RevenueSharePolicyResponse> Get(Guid id)
        {
            try
            {
                RevenueSharePolicy revenueSharePolicy = null;
                revenueSharePolicy = await _unitOfWork.Repository<RevenueSharePolicy>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RevenueSharePolicyId == id)
                    .FirstOrDefaultAsync();

                if (revenueSharePolicy == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RevenueSharePolicy, RevenueSharePolicyResponse>(revenueSharePolicy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RevenueSharePolicyResponse> Create(RevenueSharePolicyRequest request)
        {
            try
            {
                var revenueSharePolicy = _mapper.Map<RevenueSharePolicyRequest, RevenueSharePolicy>(request);
                revenueSharePolicy.RevenueSharePolicyId = Guid.NewGuid();
                await _unitOfWork.Repository<RevenueSharePolicy>().InsertAsync(revenueSharePolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RevenueSharePolicy, RevenueSharePolicyResponse>(revenueSharePolicy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RevenueSharePolicyResponse> Delete(Guid id)
        {
            try
            {
                RevenueSharePolicy revenueSharePolicy = null;
                revenueSharePolicy = _unitOfWork.Repository<RevenueSharePolicy>()
                    .Find(p => p.RevenueSharePolicyId == id);
                if (revenueSharePolicy == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<RevenueSharePolicy>().HardDeleteGuid(revenueSharePolicy.RevenueSharePolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RevenueSharePolicy, RevenueSharePolicyResponse>(revenueSharePolicy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RevenueSharePolicyResponse> Update(Guid id, RevenueSharePolicyRequest request)
        {
            try
            {
                RevenueSharePolicy revenueSharePolicy = _unitOfWork.Repository<RevenueSharePolicy>()
                            .Find(x => x.RevenueSharePolicyId == id);
                if (revenueSharePolicy == null)
                {
                    throw new Exception();
                }
                revenueSharePolicy = _mapper.Map(request, revenueSharePolicy);

                await _unitOfWork.Repository<RevenueSharePolicy>().UpdateDetached(revenueSharePolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RevenueSharePolicy, RevenueSharePolicyResponse>(revenueSharePolicy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
