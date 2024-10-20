﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.DTOs.Types;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class TypePricingService: ITypePricingService
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
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
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
                await _unitOfWork.Repository<Role>().HardDeleteGuid(typePricing.TypePricingId);
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
                                            .Where(x=> x.TypeId == id)
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

        public async Task<TypePricingResponse> GetPricingByTypeAndDistrict(Guid typeId, Guid districtId, int dayOfWeek)
        {
            try
            {
                var pricing = await _unitOfWork.Repository<TypePricing>()
                    .GetAll()
                    .Where(tp => tp.TypeId == typeId && tp.DistrictId == districtId && tp.DayOfWeek == dayOfWeek)
                    .FirstOrDefaultAsync();

                if (pricing == null)
                {
                    throw new Exception("No pricing found for the specified criteria.");
                }

                return _mapper.Map<TypePricing, TypePricingResponse>(pricing);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching pricing: {ex.Message}");
            }
        }
    }
}
