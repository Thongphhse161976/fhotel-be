using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.RevenuePolicies;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.RevenuePolicyValidator;
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
    public class RevenuePolicyService : IRevenuePolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RevenuePolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RevenuePolicyResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RevenuePolicy>().GetAll()
                                            .ProjectTo<RevenuePolicyResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RevenuePolicyResponse> Get(Guid id)
        {
            try
            {
                RevenuePolicy revenuePolicy = null;
                revenuePolicy = await _unitOfWork.Repository<RevenuePolicy>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Hotel)
                    .Where(x => x.RevenuePolicyId == id)
                    .FirstOrDefaultAsync();

                if (revenuePolicy == null)
                {
                    throw new Exception("RevenuePolicy not found");
                }

                return _mapper.Map<RevenuePolicy, RevenuePolicyResponse>(revenuePolicy);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RevenuePolicyResponse> Create(RevenuePolicyRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            var validator = new RevenuePolicyCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Use GetAll with a LINQ filter to check for duplicates
            // Modified validation for existingPricing to include DayOfWeek
            var existingPolicy = (await GetAll())
                .Where(u => u.HotelId == request.HotelId &&
                            ((u.EffectiveDate <= request.ExpiryDate && u.ExpiryDate >= request.EffectiveDate) ||
                             (request.EffectiveDate <= u.ExpiryDate && request.ExpiryDate >= u.EffectiveDate)))
                .ToList();


            if (existingPolicy.Any())
            {
                validationResult.Errors.Add(new ValidationFailure("Date", "Chính sách khoảng thời gian này đã tồn tại."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var revenuePolicy = _mapper.Map<RevenuePolicyRequest, RevenuePolicy>(request);
                revenuePolicy.RevenuePolicyId = Guid.NewGuid();
                revenuePolicy.CreatedDate = localTime;
                await _unitOfWork.Repository<RevenuePolicy>().InsertAsync(revenuePolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RevenuePolicy, RevenuePolicyResponse>(revenuePolicy);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RevenuePolicyResponse> Delete(Guid id)
        {
            try
            {
                RevenuePolicy revenuePolicy = null;
                revenuePolicy = _unitOfWork.Repository<RevenuePolicy>()
                    .Find(p => p.RevenuePolicyId == id);
                if (revenuePolicy == null)
                {
                    throw new Exception("Revenue Policy not found");
                }
                await _unitOfWork.Repository<RevenuePolicy>().HardDeleteGuid(revenuePolicy.RevenuePolicyId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RevenuePolicy, RevenuePolicyResponse>(revenuePolicy);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RevenuePolicyResponse> Update(Guid id, RevenuePolicyRequest request)
        {
            try
            {
                RevenuePolicy revenuePolicy = _unitOfWork.Repository<RevenuePolicy>()
                            .Find(x => x.RevenuePolicyId == id);
                if (revenuePolicy == null)
                {
                    throw new Exception();
                }
                revenuePolicy = _mapper.Map(request, revenuePolicy);

                await _unitOfWork.Repository<RevenuePolicy>().UpdateDetached(revenuePolicy);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RevenuePolicy, RevenuePolicyResponse>(revenuePolicy);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RevenuePolicyResponse>> GetAllRevenuePolicyByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<RevenuePolicy>().GetAll()
                                            .ProjectTo<RevenuePolicyResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.HotelId == id)
                                            .ToListAsync();
            return list;
        }
        
        public async Task<List<RevenuePolicyResponse>> GetAllRevenuePolicyByOwnerId(Guid id)
        {

            var list = await _unitOfWork.Repository<RevenuePolicy>().GetAll()
                                            .ProjectTo<RevenuePolicyResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.Hotel.OwnerId == id)
                                            .ToListAsync();
            return list;
        }
    }
}
