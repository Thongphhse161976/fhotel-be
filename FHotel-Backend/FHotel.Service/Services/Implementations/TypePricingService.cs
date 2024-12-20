﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.DTOs.Types;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.TypePricingValidator;
using FHotel.Service.Validators.UserValidator;
using FHotel.Services.Services.Interfaces;
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
    public class TypePricingService : ITypePricingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IRoomTypeService _roomTypeService;
        public TypePricingService(IUnitOfWork unitOfWork, IMapper mapper, IRoomTypeService roomTypeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roomTypeService = roomTypeService;
        }

        public async Task<List<TypePricingResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<TypePricing>().GetAll()
                                            .ProjectTo<TypePricingResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<TypePricingResponse> Get(Guid id)
        {
            try
            {
                TypePricing typePricing = null;
                typePricing = await _unitOfWork.Repository<TypePricing>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.Type)
                     .Include(x => x.District)
                    .Where(x => x.TypePricingId == id)
                    .FirstOrDefaultAsync();

                if (typePricing == null)
                {
                    throw new Exception("TypePricing not found");
                }

                return _mapper.Map<TypePricing, TypePricingResponse>(typePricing);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TypePricingResponse> Create(TypePricingCreateRequest request)
        {
            var validator = new TypePricingCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // Use GetAll with a LINQ filter to check for duplicates
            // Modified validation for existingPricing to include DayOfWeek
            var existingPricing = (await GetAll())
                .Where(u => u.DistrictId == request.DistrictId &&
                            u.TypeId == request.TypeId &&
                            ((u.From <= request.To && u.To >= request.From) ||
                             (request.From <= u.To && request.To >= u.From)))
                .ToList();


            if (existingPricing.Any())
            {
                validationResult.Errors.Add(new ValidationFailure("DistrictId and TypeId", "Giá khoảng thời gian này đã tồn tại."));
            }

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);
            DateTime localTime = DateTime.UtcNow + utcOffset;

            try
            {
                var typePricing = _mapper.Map<TypePricingCreateRequest, TypePricing>(request);
                typePricing.TypePricingId = Guid.NewGuid();
                typePricing.CreatedDate = localTime;
                await _unitOfWork.Repository<TypePricing>().InsertAsync(typePricing);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<TypePricing, TypePricingResponse>(typePricing);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<TypePricingResponse> Delete(Guid id)
        {
            try
            {
                TypePricing typePricing = null;
                typePricing = _unitOfWork.Repository<TypePricing>()
                    .Find(p => p.TypePricingId == id);
                if (typePricing == null)
                {
                    throw new Exception("TypePricing not found");
                }
                await _unitOfWork.Repository<TypePricing>().HardDeleteGuid(typePricing.TypePricingId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<TypePricing, TypePricingResponse>(typePricing);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TypePricingResponse> Update(Guid id, TypePricingUpdateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                TypePricing typePricing = _unitOfWork.Repository<TypePricing>()
                            .Find(x => x.TypePricingId == id);
                if (typePricing == null)
                {
                    throw new Exception();
                }
                typePricing = _mapper.Map(request, typePricing);
                typePricing.UpdatedDate = localTime;
                await _unitOfWork.Repository<TypePricing>().UpdateDetached(typePricing);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<TypePricing, TypePricingResponse>(typePricing);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<TypePricingResponse>> GetAllByTypeId(Guid id)
        {
            var list = await _unitOfWork.Repository<TypePricing>().GetAll()
                                            .ProjectTo<TypePricingResponse>(_mapper.ConfigurationProvider)
                                            .Where(x => x.TypeId == id)
                                            .ToListAsync();
            if (list == null)
            {
                throw new Exception("TypePricing is empty");
            }
            return list;
        }

        public async Task<List<TypePricingResponse>> GetAllByRoomTypeId(Guid roomTypeId)
        {
            var roomType = await _roomTypeService.Get(roomTypeId);

            var typePricings = await _unitOfWork.Repository<TypePricing>()
                                        .GetAll()
                                        .Where(tp => tp.TypeId == roomType.TypeId && tp.DistrictId == roomType.Hotel.DistrictId)
                                        .ProjectTo<TypePricingResponse>(_mapper.ConfigurationProvider)
                                        .ToListAsync();

            if (typePricings == null || !typePricings.Any())
            {
                throw new Exception("No TypePricing found for the given RoomTypeId.");
            }

            return typePricings;
        }

        public async Task<TypePricingResponse> GetPricingByTypeAndDistrict(Guid typeId, Guid districtId, DateTime currentDate)
        {
            try
            {
                // Fetch pricing for the given date range
                var pricing = await _unitOfWork.Repository<TypePricing>()
                    .GetAll()
                    .Where(tp => tp.TypeId == typeId
                                 && tp.DistrictId == districtId
                                 && tp.From <= currentDate
                                 && tp.To >= currentDate)  // Ensure the pricing is within the current date range
                    .FirstOrDefaultAsync();

                if (pricing == null)
                {
                    throw new Exception($"No pricing found for the specified criteria on {currentDate.ToShortDateString()}.");
                }

                // Adjust price based on the day of the week
                int dayOfWeek = (int)currentDate.DayOfWeek == 0 ? 7 : (int)currentDate.DayOfWeek; // 7 for Sunday (adjusted logic)
                decimal adjustedPrice = pricing.Price ?? 0;

                // Apply weekend increase if it's Saturday or Sunday
                if (dayOfWeek == 6 || dayOfWeek == 7)
                {
                    decimal percentageIncrease = pricing.PercentageIncrease ?? 0;
                    adjustedPrice *= (1 + (percentageIncrease / 100));
                }

                pricing.Price = adjustedPrice;

                return _mapper.Map<TypePricing, TypePricingResponse>(pricing);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching pricing: {ex.Message}");
            }
        }





        public async Task<decimal> GetTodayPricingByRoomType(Guid roomTypeId)
        {
            try
            {
                // Step 1: Fetch Room Type details
                var roomType = await _roomTypeService.Get(roomTypeId);
                //if (roomType == null || roomType.IsActive != true)
                if (roomType == null)
                    {
                    throw new ArgumentException("Invalid or inactive room type.");
                }

                // Step 2: Get the district ID from the room type
                var districtId = roomType.Hotel.DistrictId;

                // Step 3: Fetch today's pricing for the room type and district
                var todayPricing = await GetPricingByTypeAndDistrict(roomType.TypeId.Value, districtId.Value, DateTime.Now);

                if (todayPricing == null)
                {
                    throw new Exception("No pricing available for today.");
                }

                // Step 4: Return the price
                return todayPricing.Price ?? throw new Exception("Price for today is not set.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching today's pricing: {ex.Message}");
            }
        }


        public async Task<decimal> GetPricingByRoomTypeAndDate(Guid roomTypeId, DateTime date)
        {
            try
            {
                // Step 1: Fetch Room Type details
                var roomType = await _roomTypeService.Get(roomTypeId);
                if (roomType == null || roomType.IsActive != true)
                {
                    throw new ArgumentException("Invalid or inactive room type.");
                }

                // Step 2: Get the district ID from the room type
                var districtId = roomType.Hotel.DistrictId;

                // Step 3: Fetch pricing for the room type and district on the specified date
                var pricing = await GetPricingByTypeAndDistrict(roomType.TypeId.Value, districtId.Value, date);

                if (pricing == null)
                {
                    throw new Exception($"No pricing available for the specified date: {date.ToShortDateString()}.");
                }

                // Step 4: Apply weekend price increase (if applicable)
                decimal adjustedPrice = pricing.Price ?? 0;

                // Get the day of the week (1 = Monday, ..., 7 = Sunday)
                int dayOfWeek = (int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;

                // If the date is Saturday (6) or Sunday (7), apply the percentage increase
                if (dayOfWeek == 6 || dayOfWeek == 7)  // Saturday or Sunday
                {
                    adjustedPrice *= (1 + (pricing.PercentageIncrease ?? 0) / 100);
                }

                return adjustedPrice;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching pricing for the specified date: {ex.Message}");
            }
        }





    }
}