using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.RefundPolicies;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class RefundPolicyService: IRefundPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RefundPolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RefundPolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RefundPolicy>().GetAll()
                                            .ProjectTo<RefundPolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RefundPolicyResponse> Get(Guid id)
        {
            try
            {
                RefundPolicy refundPolicy = null;
                refundPolicy = await _unitOfWork.Repository<RefundPolicy>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RefundPolicyId == id)
                    .FirstOrDefaultAsync();

                if (refundPolicy == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RefundPolicy, RefundPolicyResponse>(refundPolicy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RefundPolicyResponse> Create(RefundPolicyRequest request)
        {
            try
            {
                var refundPolicy = _mapper.Map<RefundPolicyRequest, RefundPolicy>(request);
                refundPolicy.RefundPolicyId = Guid.NewGuid();
                await _unitOfWork.Repository<RefundPolicy>().InsertAsync(refundPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RefundPolicy, RefundPolicyResponse>(refundPolicy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RefundPolicyResponse> Delete(Guid id)
        {
            try
            {
                RefundPolicy refundPolicy = null;
                refundPolicy = _unitOfWork.Repository<RefundPolicy>()
                    .Find(p => p.RefundPolicyId == id);
                if (refundPolicy == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(refundPolicy.RefundPolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RefundPolicy, RefundPolicyResponse>(refundPolicy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RefundPolicyResponse> Update(Guid id, RefundPolicyRequest request)
        {
            try
            {
                RefundPolicy refundPolicy = _unitOfWork.Repository<RefundPolicy>()
                            .Find(x => x.RefundPolicyId == id);
                if (refundPolicy == null)
                {
                    throw new Exception();
                }
                refundPolicy = _mapper.Map(request, refundPolicy);

                await _unitOfWork.Repository<RefundPolicy>().UpdateDetached(refundPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RefundPolicy, RefundPolicyResponse>(refundPolicy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
