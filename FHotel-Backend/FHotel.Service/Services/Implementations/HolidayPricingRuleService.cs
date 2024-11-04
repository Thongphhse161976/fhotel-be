using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HolidayPricingRules;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class HolidayPricingRuleService : IHolidayPricingRuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HolidayPricingRuleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HolidayPricingRuleResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HolidayPricingRule>().GetAll()
                                            .ProjectTo<HolidayPricingRuleResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HolidayPricingRuleResponse> Get(Guid id)
        {
            try
            {
                HolidayPricingRule holidayPricingRule = null;
                holidayPricingRule = await _unitOfWork.Repository<HolidayPricingRule>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HolidayPricingRuleId == id)
                    .FirstOrDefaultAsync();

                if (holidayPricingRule == null)
                {
                    throw new Exception("HolidayPricingRule not found");
                }

                return _mapper.Map<HolidayPricingRule, HolidayPricingRuleResponse>(holidayPricingRule);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HolidayPricingRuleResponse> Create(HolidayPricingRuleCreateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var holidayPricingRule = _mapper.Map<HolidayPricingRuleCreateRequest, HolidayPricingRule>(request);
                holidayPricingRule.HolidayPricingRuleId = Guid.NewGuid();
                holidayPricingRule.CreatedDate = localTime;
                await _unitOfWork.Repository<HolidayPricingRule>().InsertAsync(holidayPricingRule);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HolidayPricingRule, HolidayPricingRuleResponse>(holidayPricingRule);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HolidayPricingRuleResponse> Delete(Guid id)
        {
            try
            {
                HolidayPricingRule holidayPricingRule = null;
                holidayPricingRule = _unitOfWork.Repository<HolidayPricingRule>()
                    .Find(p => p.HolidayPricingRuleId == id);
                if (holidayPricingRule == null)
                {
                    throw new Exception("HolidayPricingRule not found");
                }
                await _unitOfWork.Repository<HolidayPricingRule>().HardDeleteGuid(holidayPricingRule.HolidayPricingRuleId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HolidayPricingRule, HolidayPricingRuleResponse>(holidayPricingRule);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HolidayPricingRuleResponse> Update(Guid id, HolidayPricingRuleUpdateRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                HolidayPricingRule holidayPricingRule = _unitOfWork.Repository<HolidayPricingRule>()
                            .Find(x => x.HolidayPricingRuleId == id);
                if (holidayPricingRule == null)
                {
                    throw new Exception();
                }
                holidayPricingRule = _mapper.Map(request, holidayPricingRule);
                holidayPricingRule.UpdatedDate = localTime;
                await _unitOfWork.Repository<HolidayPricingRule>().UpdateDetached(holidayPricingRule);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HolidayPricingRule, HolidayPricingRuleResponse>(holidayPricingRule);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
