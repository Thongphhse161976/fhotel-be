using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Timetable;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class TimetableService : ITimetableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public TimetableService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TimetableResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Timetable>().GetAll()
                                            .ProjectTo<TimetableResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<TimetableResponse> Get(Guid id)
        {
            try
            {
                Timetable timetable = null;
                timetable = await _unitOfWork.Repository<Timetable>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.TimetableId == id)
                    .FirstOrDefaultAsync();

                if (timetable == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Timetable, TimetableResponse>(timetable);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TimetableResponse> Create(TimetableRequest request)
        {
            try
            {
                var timetable = _mapper.Map<TimetableRequest, Timetable>(request);
                timetable.TimetableId = Guid.NewGuid();
                await _unitOfWork.Repository<Timetable>().InsertAsync(timetable);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Timetable, TimetableResponse>(timetable);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<TimetableResponse> Delete(Guid id)
        {
            try
            {
                Timetable timetable = null;
                timetable = _unitOfWork.Repository<Timetable>()
                    .Find(p => p.TimetableId == id);
                if (timetable == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(timetable.TimetableId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Timetable, TimetableResponse>(timetable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TimetableResponse> Update(Guid id, TimetableRequest request)
        {
            try
            {
                Timetable timetable = _unitOfWork.Repository<Timetable>()
                            .Find(x => x.TimetableId == id);
                if (timetable == null)
                {
                    throw new Exception();
                }
                timetable = _mapper.Map(request, timetable);

                await _unitOfWork.Repository<Timetable>().UpdateDetached(timetable);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Timetable, TimetableResponse>(timetable);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
