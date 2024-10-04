using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.LateCheckOutPolicies;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class LateCheckOutPolicyService:ILateCheckOutPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public LateCheckOutPolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LateCheckOutPolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<LateCheckOutPolicy>().GetAll()
                                            .ProjectTo<LateCheckOutPolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<LateCheckOutPolicyResponse> Get(Guid id)
        {
            try
            {
                LateCheckOutPolicy lateCheckOutPolicy = null;
                lateCheckOutPolicy = await _unitOfWork.Repository<LateCheckOutPolicy>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.LateCheckOutPolicyId == id)
                    .FirstOrDefaultAsync();

                if (lateCheckOutPolicy == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<LateCheckOutPolicy, LateCheckOutPolicyResponse>(lateCheckOutPolicy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<LateCheckOutPolicyResponse> Create(LateCheckOutPolicyRequest request)
        {
            try
            {
                var lateCheckOutPolicy = _mapper.Map<LateCheckOutPolicyRequest, LateCheckOutPolicy>(request);
                lateCheckOutPolicy.LateCheckOutPolicyId = Guid.NewGuid();
                await _unitOfWork.Repository<LateCheckOutPolicy>().InsertAsync(lateCheckOutPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<LateCheckOutPolicy, LateCheckOutPolicyResponse>(lateCheckOutPolicy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<LateCheckOutPolicyResponse> Delete(Guid id)
        {
            try
            {
                LateCheckOutPolicy lateCheckOutPolicy = null;
                lateCheckOutPolicy = _unitOfWork.Repository<LateCheckOutPolicy>()
                    .Find(p => p.LateCheckOutPolicyId == id);
                if (lateCheckOutPolicy == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(lateCheckOutPolicy.LateCheckOutPolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<LateCheckOutPolicy, LateCheckOutPolicyResponse>(lateCheckOutPolicy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LateCheckOutPolicyResponse> Update(Guid id, LateCheckOutPolicyRequest request)
        {
            try
            {
                LateCheckOutPolicy lateCheckOutPolicy = _unitOfWork.Repository<LateCheckOutPolicy>()
                            .Find(x => x.LateCheckOutPolicyId == id);
                if (lateCheckOutPolicy == null)
                {
                    throw new Exception();
                }
                lateCheckOutPolicy = _mapper.Map(request, lateCheckOutPolicy);

                await _unitOfWork.Repository<LateCheckOutPolicy>().UpdateDetached(lateCheckOutPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<LateCheckOutPolicy, LateCheckOutPolicyResponse>(lateCheckOutPolicy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
