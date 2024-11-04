using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Holidays;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class HolidayService : IHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HolidayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HolidayResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Holiday>().GetAll()
                                            .ProjectTo<HolidayResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HolidayResponse> Get(Guid id)
        {
            try
            {
                Holiday holiday = null;
                holiday = await _unitOfWork.Repository<Holiday>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HolidayId == id)
                    .FirstOrDefaultAsync();

                if (holiday == null)
                {
                    throw new Exception("Holiday not find");
                }

                return _mapper.Map<Holiday, HolidayResponse>(holiday);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HolidayResponse> Create(HolidayRequest request)
        {
            try
            {
                var holiday = _mapper.Map<HolidayRequest, Holiday>(request);
                holiday.HolidayId = Guid.NewGuid();
                await _unitOfWork.Repository<Holiday>().InsertAsync(holiday);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Holiday, HolidayResponse>(holiday);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HolidayResponse> Delete(Guid id)
        {
            try
            {
                Holiday holiday = null;
                holiday = _unitOfWork.Repository<Holiday>()
                    .Find(p => p.HolidayId == id);
                if (holiday == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Holiday>().HardDeleteGuid(holiday.HolidayId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Holiday, HolidayResponse>(holiday);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HolidayResponse> Update(Guid id, HolidayRequest request)
        {
            try
            {
                Holiday holiday = _unitOfWork.Repository<Holiday>()
                            .Find(x => x.HolidayId == id);
                if (holiday == null)
                {
                    throw new Exception();
                }
                holiday = _mapper.Map(request, holiday);

                await _unitOfWork.Repository<Holiday>().UpdateDetached(holiday);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Holiday, HolidayResponse>(holiday);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
