using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Payments;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PaymentResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Payment>().GetAll()
                                            .ProjectTo<PaymentResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<PaymentResponse> Get(Guid id)
        {
            try
            {
                Payment payment = null;
                payment = await _unitOfWork.Repository<Payment>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.PaymentId == id)
                    .FirstOrDefaultAsync();

                if (payment == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Payment, PaymentResponse>(payment);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentResponse> Create(PaymentRequest request)
        {
            try
            {
                var payment = _mapper.Map<PaymentRequest, Payment>(request);
                payment.PaymentId = Guid.NewGuid();
                await _unitOfWork.Repository<Payment>().InsertAsync(payment);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Payment, PaymentResponse>(payment);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<PaymentResponse> Delete(Guid id)
        {
            try
            {
                Payment payment = null;
                payment = _unitOfWork.Repository<Payment>()
                    .Find(p => p.PaymentId == id);
                if (payment == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(payment.PaymentId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Payment, PaymentResponse>(payment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaymentResponse> Update(Guid id, PaymentRequest request)
        {
            try
            {
                Payment payment = _unitOfWork.Repository<Payment>()
                            .Find(x => x.PaymentId == id);
                if (payment == null)
                {
                    throw new Exception();
                }
                payment = _mapper.Map(request, payment);

                await _unitOfWork.Repository<Payment>().UpdateDetached(payment);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Payment, PaymentResponse>(payment);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
