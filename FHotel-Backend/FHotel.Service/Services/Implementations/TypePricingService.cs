using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.Services.Interfaces;
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
        public TypePricingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
    }
}
