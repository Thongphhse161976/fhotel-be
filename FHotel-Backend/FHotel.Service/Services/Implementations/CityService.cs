using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<City>().GetAll()
                                            .ProjectTo<CityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<CityResponse> Get(Guid id)
        {
            try
            {
                City city = null;
                city = await _unitOfWork.Repository<City>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.CityId == id)
                    .FirstOrDefaultAsync();

                if (city == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<City, CityResponse>(city);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CityResponse> Create(CityRequest request)
        {
            try
            {
                var city = _mapper.Map<CityRequest, City>(request);
                city.CityId = Guid.NewGuid();
                await _unitOfWork.Repository<City>().InsertAsync(city);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<City, CityResponse>(city);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CityResponse> Delete(Guid id)
        {
            try
            {
                City city = null;
                city = _unitOfWork.Repository<City>()
                    .Find(p => p.CityId == id);
                if (city == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<City>().HardDeleteGuid(city.CityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<City, CityResponse>(city);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CityResponse> Update(Guid id, CityRequest request)
        {
            try
            {
                City city = _unitOfWork.Repository<City>()
                            .Find(x => x.CityId == id);
                if (city == null)
                {
                    throw new Exception();
                }
                city = _mapper.Map(request, city);

                await _unitOfWork.Repository<City>().UpdateDetached(city);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<City, CityResponse>(city);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
