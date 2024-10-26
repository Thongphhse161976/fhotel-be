using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Areas;
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
    public class AreaService: IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AreaResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Area>().GetAll()
                                            .ProjectTo<AreaResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<AreaResponse> Get(Guid id)
        {
            try
            {
                Area area = null;
                area = await _unitOfWork.Repository<Area>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.AreaId == id)
                    .FirstOrDefaultAsync();

                if (area == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Area, AreaResponse>(area);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AreaResponse> Create(AreaRequest request)
        {
            try
            {
                var area = _mapper.Map<AreaRequest, Area>(request);
                area.AreaId = Guid.NewGuid();
                await _unitOfWork.Repository<Area>().InsertAsync(area);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Area, AreaResponse>(area);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AreaResponse> Delete(Guid id)
        {
            try
            {
                Area area = null;
                area = _unitOfWork.Repository<Area>()
                    .Find(p => p.AreaId == id);
                if (area == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Area>().HardDeleteGuid(area.AreaId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Area, AreaResponse>(area);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AreaResponse> Update(Guid id, AreaRequest request)
        {
            try
            {
                Area area = _unitOfWork.Repository<Area>()
                            .Find(x => x.AreaId == id);
                if (area == null)
                {
                    throw new Exception();
                }
                area = _mapper.Map(request, area);

                await _unitOfWork.Repository<Area>().UpdateDetached(area);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Area, AreaResponse>(area);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
