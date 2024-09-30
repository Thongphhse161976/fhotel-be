using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomTypeResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomType>().GetAll()
                                            .ProjectTo<RoomTypeResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomTypeResponse> Get(Guid id)
        {
            try
            {
                RoomType roomType = null;
                roomType = await _unitOfWork.Repository<RoomType>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomTypeId == id)
                    .FirstOrDefaultAsync();

                if (roomType == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomTypeResponse> Create(RoomTypeRequest request)
        {
            try
            {
                var roomType = _mapper.Map<RoomTypeRequest, RoomType>(request);
                roomType.RoomTypeId = Guid.NewGuid();
                await _unitOfWork.Repository<RoomType>().InsertAsync(roomType);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomTypeResponse> Delete(Guid id)
        {
            try
            {
                RoomType roomType = null;
                roomType = _unitOfWork.Repository<RoomType>()
                    .Find(p => p.RoomTypeId == id);
                if (roomType == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(roomType.RoomTypeId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomTypeResponse> Update(Guid id, RoomTypeRequest request)
        {
            try
            {
                RoomType roomType = _unitOfWork.Repository<RoomType>()
                            .Find(x => x.RoomTypeId == id);
                if (roomType == null)
                {
                    throw new Exception();
                }
                roomType = _mapper.Map(request, roomType);

                await _unitOfWork.Repository<RoomType>().UpdateDetached(roomType);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomType, RoomTypeResponse>(roomType);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
