using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Refunds;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class RefundService: IRefundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RefundService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RefundResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Refund>().GetAll()
                                            .ProjectTo<RefundResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RefundResponse> Get(Guid id)
        {
            try
            {
                Refund refund = null;
                refund = await _unitOfWork.Repository<Refund>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RefundId == id)
                    .FirstOrDefaultAsync();

                if (refund == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Refund, RefundResponse>(refund);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RefundResponse> Create(RefundRequest request)
        {
            try
            {
                var refund = _mapper.Map<RefundRequest, Refund>(request);
                refund.RefundId = Guid.NewGuid();
                await _unitOfWork.Repository<Refund>().InsertAsync(refund);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Refund, RefundResponse>(refund);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RefundResponse> Delete(Guid id)
        {
            try
            {
                Refund refund = null;
                refund = _unitOfWork.Repository<Refund>()
                    .Find(p => p.RefundId == id);
                if (refund == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Refund>().HardDeleteGuid(refund.RefundId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Refund, RefundResponse>(refund);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RefundResponse> Update(Guid id, RefundRequest request)
        {
            try
            {
                Refund refund = _unitOfWork.Repository<Refund>()
                            .Find(x => x.RefundId == id);
                if (refund == null)
                {
                    throw new Exception();
                }
                refund = _mapper.Map(request, refund);

                await _unitOfWork.Repository<Refund>().UpdateDetached(refund);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Refund, RefundResponse>(refund);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
