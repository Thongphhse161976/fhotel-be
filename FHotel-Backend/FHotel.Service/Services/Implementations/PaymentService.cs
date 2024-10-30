using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Payments;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Rooms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
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
            if (list == null)
            {
                throw new Exception("Payment not found");
            }
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
                    throw new Exception("Payment not found");
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
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var payment = _mapper.Map<PaymentRequest, Payment>(request);
                payment.PaymentId = Guid.NewGuid();
                payment.CreatedDate = localTime;
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
                await _unitOfWork.Repository<Payment>().HardDeleteGuid(payment.PaymentId);
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
                    throw new Exception("Not Found");
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
        public async Task<List<PaymentResponse>> GetAllPaymentByBillId(Guid id)
        {

            var list = await _unitOfWork.Repository<Payment>().GetAll()
                                            .ProjectTo<PaymentResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.BillId == id)
                                            .ToListAsync();
            return list;
        }

    }
}
