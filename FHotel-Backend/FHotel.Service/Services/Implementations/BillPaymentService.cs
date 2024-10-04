using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.BillPayments;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class BillPaymentService: IBillPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BillPaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BillPaymentResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<BillPayment>().GetAll()
                                            .ProjectTo<BillPaymentResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<BillPaymentResponse> Get(Guid id)
        {
            try
            {
                BillPayment billPayment = null;
                billPayment = await _unitOfWork.Repository<BillPayment>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.BillPaymentId == id)
                    .FirstOrDefaultAsync();

                if (billPayment == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<BillPayment, BillPaymentResponse>(billPayment);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillPaymentResponse> Create(BillPaymentRequest request)
        {
            try
            {
                var billPayment = _mapper.Map<BillPaymentRequest, BillPayment>(request);
                billPayment.BillPaymentId = Guid.NewGuid();
                await _unitOfWork.Repository<BillPayment>().InsertAsync(billPayment);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillPayment, BillPaymentResponse>(billPayment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BillPaymentResponse> Delete(Guid id)
        {
            try
            {
                BillPayment billPayment = null;
                billPayment = _unitOfWork.Repository<BillPayment>()
                    .Find(p => p.BillPaymentId == id);
                if (billPayment == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(billPayment.BillPaymentId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<BillPayment, BillPaymentResponse>(billPayment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BillPaymentResponse> Update(Guid id, BillPaymentRequest request)
        {
            try
            {
                BillPayment billPayment = _unitOfWork.Repository<BillPayment>()
                            .Find(x => x.BillPaymentId == id);
                if (billPayment == null)
                {
                    throw new Exception();
                }
                billPayment = _mapper.Map(request, billPayment);

                await _unitOfWork.Repository<BillPayment>().UpdateDetached(billPayment);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<BillPayment, BillPaymentResponse>(billPayment);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
