using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;

using FHotel.Services.DTOs.Countries;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CountryResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Country>().GetAll()
                                            .ProjectTo<CountryResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<CountryResponse> Get(Guid id)
        {
            try
            {
                Country country = null;
                country = await _unitOfWork.Repository<Country>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.CountryId == id)
                    .FirstOrDefaultAsync();

                if (country == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Country, CountryResponse>(country);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CountryResponse> Create(CountryRequest request)
        {
            try
            {
                var country = _mapper.Map<CountryRequest, Country>(request);
                country.CountryId = Guid.NewGuid();
                await _unitOfWork.Repository<Country>().InsertAsync(country);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Country, CountryResponse>(country);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CountryResponse> Delete(Guid id)
        {
            try
            {
                Country country = null;
                country = _unitOfWork.Repository<Country>()
                    .Find(p => p.CountryId == id);
                if (country == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Country>().HardDeleteGuid(country.CountryId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Country, CountryResponse>(country);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CountryResponse> Update(Guid id, CountryRequest request)
        {
            try
            {
                Country country = _unitOfWork.Repository<Country>()
                            .Find(x => x.CountryId == id);
                if (country == null)
                {
                    throw new Exception();
                }
                country = _mapper.Map(request, country);

                await _unitOfWork.Repository<Country>().UpdateDetached(country);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Country, CountryResponse>(country);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
