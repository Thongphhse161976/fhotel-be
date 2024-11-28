using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Policies;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class PolicyService : IPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Policy>().GetAll()
                                            .ProjectTo<PolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<PolicyResponse> Get(Guid id)
        {
            try
            {
                Policy policy = null;
                policy = await _unitOfWork.Repository<Policy>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.PolicyId == id)
                    .FirstOrDefaultAsync();

                if (policy == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Policy, PolicyResponse>(policy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PolicyResponse> Create(PolicyRequest request)
        {
            try
            {
                var policy = _mapper.Map<PolicyRequest, Policy>(request);
                policy.PolicyId = Guid.NewGuid();
                await _unitOfWork.Repository<Policy>().InsertAsync(policy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Policy, PolicyResponse>(policy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PolicyResponse> Delete(Guid id)
        {
            try
            {
                Policy policy = null;
                policy = _unitOfWork.Repository<Policy>()
                    .Find(p => p.PolicyId == id);
                if (policy == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Policy>().HardDeleteGuid(policy.PolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Policy, PolicyResponse>(policy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PolicyResponse> Update(Guid id, PolicyRequest request)
        {
            try
            {
                Policy policy = _unitOfWork.Repository<Policy>()
                            .Find(x => x.PolicyId == id);
                if (policy == null)
                {
                    throw new Exception();
                }
                policy = _mapper.Map(request, policy);

                await _unitOfWork.Repository<Policy>().UpdateDetached(policy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Policy, PolicyResponse>(policy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
