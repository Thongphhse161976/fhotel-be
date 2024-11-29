using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.CancellationPolicies;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.CancellationPolicyValidator;
using FHotel.Service.Validators.TypePricingValidator;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class CancellationPolicyService : ICancellationPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CancellationPolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CancellationPolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<CancellationPolicy>().GetAll()
                                            .ProjectTo<CancellationPolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<CancellationPolicyResponse> Get(Guid id)
        {
            try
            {
                CancellationPolicy cancellationPolicy = null;
                cancellationPolicy = await _unitOfWork.Repository<CancellationPolicy>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Hotel)
                    .Where(x => x.CancellationPolicyId == id)
                    .FirstOrDefaultAsync();

                if (cancellationPolicy == null)
                {
                    throw new Exception("CancellationPolicy not found");
                }

                return _mapper.Map<CancellationPolicy, CancellationPolicyResponse>(cancellationPolicy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CancellationPolicyResponse> Create(CancellationPolicyRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            var validator = new CancellationPolicyCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            // Check if a cancellation policy already exists for the hotel
            var existingPolicy = (await GetAll())
            .Where(u => u.HotelId == request.HotelId)
            .ToList();
            if (existingPolicy.Any())
            {
                validationResult.Errors.Add(new ValidationFailure("Hotel ID", "Chính sách hoàn tiền của khách sạn đã tồn tại."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
               
                // Map request to entity
                var cancellationPolicy = _mapper.Map<CancellationPolicyRequest, CancellationPolicy>(request);
                cancellationPolicy.CancellationPolicyId = Guid.NewGuid();
                cancellationPolicy.CreatedDate = localTime;

                // Insert the new cancellation policy
                await _unitOfWork.Repository<CancellationPolicy>().InsertAsync(cancellationPolicy);
                await _unitOfWork.CommitAsync();

                // Map entity to response and return
                return _mapper.Map<CancellationPolicy, CancellationPolicyResponse>(cancellationPolicy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<CancellationPolicyResponse> Delete(Guid id)
        {
            try
            {
                CancellationPolicy cancellationPolicy = null;
                cancellationPolicy = _unitOfWork.Repository<CancellationPolicy>()
                    .Find(p => p.CancellationPolicyId == id);
                if (cancellationPolicy == null)
                {
                    throw new Exception("Cancellation Policy not found");
                }
                await _unitOfWork.Repository<CancellationPolicy>().HardDeleteGuid(cancellationPolicy.CancellationPolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<CancellationPolicy, CancellationPolicyResponse>(cancellationPolicy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CancellationPolicyResponse> Update(Guid id, CancellationPolicyRequest request)
        {
            try
            {
                CancellationPolicy cancellationPolicy = _unitOfWork.Repository<CancellationPolicy>()
                            .Find(x => x.CancellationPolicyId == id);
                if (cancellationPolicy == null)
                {
                    throw new Exception();
                }
                cancellationPolicy = _mapper.Map(request, cancellationPolicy);

                await _unitOfWork.Repository<CancellationPolicy>().UpdateDetached(cancellationPolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<CancellationPolicy, CancellationPolicyResponse>(cancellationPolicy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CancellationPolicyResponse>> GetAllCancellationPolicyByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<CancellationPolicy>().GetAll()
                                            .ProjectTo<CancellationPolicyResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.HotelId == id)
                                            .ToListAsync();
            return list;
        }
    }
}
