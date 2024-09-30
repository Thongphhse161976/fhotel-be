using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.DamagedFactilities;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class DamagedFacilityService : IDamagedFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public DamagedFacilityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DamagedFacilityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<DamagedFacility>().GetAll()
                                            .ProjectTo<DamagedFacilityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<DamagedFacilityResponse> Get(Guid id)
        {
            try
            {
                DamagedFacility damagedFacility = null;
                damagedFacility = await _unitOfWork.Repository<DamagedFacility>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.DamagedFacilityId == id)
                    .FirstOrDefaultAsync();

                if (damagedFacility == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<DamagedFacility, DamagedFacilityResponse>(damagedFacility);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DamagedFacilityResponse> Create(DamagedFacilityRequest request)
        {
            try
            {
                var damagedFacility = _mapper.Map<DamagedFacilityRequest, DamagedFacility>(request);
                damagedFacility.DamagedFacilityId = Guid.NewGuid();
                await _unitOfWork.Repository<DamagedFacility>().InsertAsync(damagedFacility);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<DamagedFacility, DamagedFacilityResponse>(damagedFacility);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DamagedFacilityResponse> Delete(Guid id)
        {
            try
            {
                DamagedFacility damagedFacility = null;
                damagedFacility = _unitOfWork.Repository<DamagedFacility>()
                    .Find(p => p.DamagedFacilityId == id);
                if (damagedFacility == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(damagedFacility.DamagedFacilityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<DamagedFacility, DamagedFacilityResponse>(damagedFacility);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DamagedFacilityResponse> Update(Guid id, DamagedFacilityRequest request)
        {
            try
            {
                DamagedFacility damagedFacility = _unitOfWork.Repository<DamagedFacility>()
                            .Find(x => x.DamagedFacilityId == id);
                if (damagedFacility == null)
                {
                    throw new Exception();
                }
                damagedFacility = _mapper.Map(request, damagedFacility);

                await _unitOfWork.Repository<DamagedFacility>().UpdateDetached(damagedFacility);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<DamagedFacility, DamagedFacilityResponse>(damagedFacility);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
