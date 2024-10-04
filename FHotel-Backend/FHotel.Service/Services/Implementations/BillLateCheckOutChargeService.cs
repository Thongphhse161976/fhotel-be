using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.BillLateCheckOutCharges;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class BillLateCheckOutChargeService: IBillLateCheckOutChargeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BillLateCheckOutChargeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BillLateCheckOutChargeResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<BillLateCheckOutCharge>().GetAll()
                                            .ProjectTo<BillLateCheckOutChargeResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<BillLateCheckOutChargeResponse> Get(Guid id)
        {
            try
            {
                BillLateCheckOutCharge billLateCheckOutCharge = null;
                billLateCheckOutCharge = await _unitOfWork.Repository<BillLateCheckOutCharge>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.BillLateCheckOutChargeId == id)
                    .FirstOrDefaultAsync();

                if (billLateCheckOutCharge == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<BillLateCheckOutCharge, BillLateCheckOutChargeResponse>(billLateCheckOutCharge);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillLateCheckOutChargeResponse> Create(BillLateCheckOutChargeRequest request)
        {
            try
            {
                var billLateCheckOutCharge = _mapper.Map<BillLateCheckOutChargeRequest, BillLateCheckOutCharge>(request);
                billLateCheckOutCharge.BillLateCheckOutChargeId = Guid.NewGuid();
                await _unitOfWork.Repository<BillLateCheckOutCharge>().InsertAsync(billLateCheckOutCharge);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillLateCheckOutCharge, BillLateCheckOutChargeResponse>(billLateCheckOutCharge);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillLateCheckOutChargeResponse> Delete(Guid id)
        {
            try
            {
                BillLateCheckOutCharge billLateCheckOutCharge = null;
                billLateCheckOutCharge = _unitOfWork.Repository<BillLateCheckOutCharge>()
                    .Find(p => p.BillLateCheckOutChargeId == id);
                if (billLateCheckOutCharge == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(billLateCheckOutCharge.BillLateCheckOutChargeId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<BillLateCheckOutCharge, BillLateCheckOutChargeResponse>(billLateCheckOutCharge);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BillLateCheckOutChargeResponse> Update(Guid id, BillLateCheckOutChargeRequest request)
        {
            try
            {
                BillLateCheckOutCharge billLateCheckOutCharge = _unitOfWork.Repository<BillLateCheckOutCharge>()
                            .Find(x => x.BillLateCheckOutChargeId == id);
                if (billLateCheckOutCharge == null)
                {
                    throw new Exception();
                }
                billLateCheckOutCharge = _mapper.Map(request, billLateCheckOutCharge);

                await _unitOfWork.Repository<BillLateCheckOutCharge>().UpdateDetached(billLateCheckOutCharge);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillLateCheckOutCharge, BillLateCheckOutChargeResponse>(billLateCheckOutCharge);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
