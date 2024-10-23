using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class RoomStayHistoryService: IRoomStayHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomStayHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomStayHistoryResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomStayHistory>().GetAll()
                                            .ProjectTo<RoomStayHistoryResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomStayHistoryResponse> Get(Guid id)
        {
            try
            {
                RoomStayHistory roomStayHistory = null;
                roomStayHistory = await _unitOfWork.Repository<RoomStayHistory>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomStayHistoryId == id)
                    .FirstOrDefaultAsync();

                if (roomStayHistory == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomStayHistoryResponse> Create(RoomStayHistoryRequest request)
        {
            try
            {
                var roomStayHistory = _mapper.Map<RoomStayHistoryRequest, RoomStayHistory>(request);
                roomStayHistory.RoomStayHistoryId = Guid.NewGuid();
                await _unitOfWork.Repository<RoomStayHistory>().InsertAsync(roomStayHistory);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomStayHistoryResponse> Delete(Guid id)
        {
            try
            {
                RoomStayHistory roomStayHistory = null;
                roomStayHistory = _unitOfWork.Repository<RoomStayHistory>()
                    .Find(p => p.RoomStayHistoryId == id);
                if (roomStayHistory == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<RoomStayHistory>().HardDeleteGuid(roomStayHistory.RoomStayHistoryId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomStayHistoryResponse> Update(Guid id, RoomStayHistoryRequest request)
        {
            try
            {
                RoomStayHistory roomStayHistory = _unitOfWork.Repository<RoomStayHistory>()
                            .Find(x => x.RoomStayHistoryId == id);
                if (roomStayHistory == null)
                {
                    throw new Exception();
                }
                roomStayHistory = _mapper.Map(request, roomStayHistory);

                await _unitOfWork.Repository<RoomStayHistory>().UpdateDetached(roomStayHistory);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomStayHistory, RoomStayHistoryResponse>(roomStayHistory);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
