using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.LateCheckOutCharges;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class LateCheckOutChargeService: ILateCheckOutChargeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public LateCheckOutChargeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LateCheckOutChargeResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<LateCheckOutCharge>().GetAll()
                                            .ProjectTo<LateCheckOutChargeResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<LateCheckOutChargeResponse> Get(Guid id)
        {
            try
            {
                LateCheckOutCharge lateCheckOutCharge = null;
                lateCheckOutCharge = await _unitOfWork.Repository<LateCheckOutCharge>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.LateCheckOutChargeId == id)
                    .FirstOrDefaultAsync();

                if (lateCheckOutCharge == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<LateCheckOutCharge, LateCheckOutChargeResponse>(lateCheckOutCharge);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<LateCheckOutChargeResponse> Create(LateCheckOutChargeRequest request)
        {
            try
            {
                var lateCheckOutCharge = _mapper.Map<LateCheckOutChargeRequest, LateCheckOutCharge>(request);
                lateCheckOutCharge.LateCheckOutChargeId = Guid.NewGuid();
                await _unitOfWork.Repository<LateCheckOutCharge>().InsertAsync(lateCheckOutCharge);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<LateCheckOutCharge, LateCheckOutChargeResponse>(lateCheckOutCharge);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<LateCheckOutChargeResponse> Delete(Guid id)
        {
            try
            {
                LateCheckOutCharge lateCheckOutCharge = null;
                lateCheckOutCharge = _unitOfWork.Repository<LateCheckOutCharge>()
                    .Find(p => p.LateCheckOutChargeId == id);
                if (lateCheckOutCharge == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<LateCheckOutCharge>().HardDeleteGuid(lateCheckOutCharge.LateCheckOutChargeId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<LateCheckOutCharge, LateCheckOutChargeResponse>(lateCheckOutCharge);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LateCheckOutChargeResponse> Update(Guid id, LateCheckOutChargeRequest request)
        {
            try
            {
                LateCheckOutCharge lateCheckOutCharge = _unitOfWork.Repository<LateCheckOutCharge>()
                            .Find(x => x.LateCheckOutChargeId == id);
                if (lateCheckOutCharge == null)
                {
                    throw new Exception();
                }
                lateCheckOutCharge = _mapper.Map(request, lateCheckOutCharge);

                await _unitOfWork.Repository<LateCheckOutCharge>().UpdateDetached(lateCheckOutCharge);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<LateCheckOutCharge, LateCheckOutChargeResponse>(lateCheckOutCharge);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
