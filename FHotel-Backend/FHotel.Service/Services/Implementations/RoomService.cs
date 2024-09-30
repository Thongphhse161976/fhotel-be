using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Room>().GetAll()
                                            .ProjectTo<RoomResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomResponse> Get(Guid id)
        {
            try
            {
                Room room = null;
                room = await _unitOfWork.Repository<Room>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomId == id)
                    .FirstOrDefaultAsync();

                if (room == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Room, RoomResponse>(room);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomResponse> Create(RoomRequest request)
        {
            try
            {
                var room = _mapper.Map<RoomRequest, Room>(request);
                room.RoomId = Guid.NewGuid();
                await _unitOfWork.Repository<Room>().InsertAsync(room);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomResponse> Delete(Guid id)
        {
            try
            {
                Room room = null;
                room = _unitOfWork.Repository<Room>()
                    .Find(p => p.RoomId == id);
                if (room == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(room.RoomId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Room, RoomResponse>(room);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomResponse> Update(Guid id, RoomRequest request)
        {
            try
            {
                Room room = _unitOfWork.Repository<Room>()
                            .Find(x => x.RoomId == id);
                if (room == null)
                {
                    throw new Exception();
                }
                room = _mapper.Map(request, room);

                await _unitOfWork.Repository<Room>().UpdateDetached(room);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Room, RoomResponse>(room);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
