using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.Validators.PaymentValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Payments;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
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
        private readonly IReservationService _reservationService;
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IReservationService reservationService, IPaymentMethodService paymentMethodService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reservationService = reservationService;
            _paymentMethodService = paymentMethodService;
        }

        public async Task<List<PaymentResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Payment>().GetAll()
                                            .ProjectTo<PaymentResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            // Check if list is null or empty
            if (list == null || !list.Any())
            {
                throw new Exception("No payments found.");
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
            // Create the validator instance
            var validator = new PaymentRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Check for validation errors
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            request.PaymentStatus = "Pending";
            try
            {
                await _reservationService.Get((Guid)request.ReservationId);
                await _paymentMethodService.Get((Guid)request.PaymentMethodId);
                var payment = _mapper.Map<PaymentRequest, Payment>(request);
                payment.PaymentId = Guid.NewGuid();
                payment.PaymentDate = localTime; 
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
            // Create the validator instance
            var validator = new PaymentUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Check for validation errors
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                await _reservationService.Get((Guid)request.ReservationId);
                await _paymentMethodService.Get((Guid)request.PaymentMethodId);
                Payment payment = _unitOfWork.Repository<Payment>()
                            .Find(x => x.PaymentId == id);
                if (payment == null)
                {
                    throw new Exception("Payment not found");
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
