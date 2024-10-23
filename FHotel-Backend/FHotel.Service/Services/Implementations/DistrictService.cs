using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Cities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class DistrictService: IDistrictService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public DistrictService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DistrictResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<District>().GetAll()
                                            .ProjectTo<DistrictResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<DistrictResponse> Get(Guid id)
        {
            try
            {
                District district = null;
                district = await _unitOfWork.Repository<District>().GetAll()
                     .AsNoTracking()
                     .Include(x => x.City)
                    .Where(x => x.DistrictId == id)
                    .FirstOrDefaultAsync();

                if (district == null)
                {
                    throw new Exception("District not found");
                }

                return _mapper.Map<District, DistrictResponse>(district);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DistrictResponse> Create(DistrictRequest request)
        {
            try
            {
                var district = _mapper.Map<DistrictRequest, District>(request);
                district.DistrictId = Guid.NewGuid();
                await _unitOfWork.Repository<District>().InsertAsync(district);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<District, DistrictResponse>(district);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DistrictResponse> Delete(Guid id)
        {
            try
            {
                District district = null;
                district = _unitOfWork.Repository<District>()
                    .Find(p => p.DistrictId == id);
                if (district == null)
                {
                    throw new Exception("District not found");
                }
                await _unitOfWork.Repository<District>().HardDeleteGuid(district.DistrictId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<District, DistrictResponse>(district);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DistrictResponse> Update(Guid id, DistrictRequest request)
        {
            try
            {
                District district = _unitOfWork.Repository<District>()
                            .Find(x => x.DistrictId == id);
                if (district == null)
                {
                    throw new Exception();
                }
                district = _mapper.Map(request, district);

                await _unitOfWork.Repository<District>().UpdateDetached(district);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<District, DistrictResponse>(district);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DistrictResponse>> GetAllByCityId(Guid id)
        {

            var list = await _unitOfWork.Repository<District>().GetAll()
                                            .ProjectTo<DistrictResponse>(_mapper.ConfigurationProvider)
                                            .Where(d => d.CityId == id)
                                            .ToListAsync();
            return list;
        }
    }
}
