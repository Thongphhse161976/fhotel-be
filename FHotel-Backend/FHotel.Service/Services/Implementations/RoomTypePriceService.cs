using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.RoomTypePrices;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class RoomTypePriceService : IRoomTypePriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomTypePriceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomTypePriceResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomTypePrice>().GetAll()
                                            .ProjectTo<RoomTypePriceResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomTypePriceResponse> Get(Guid id)
        {
            try
            {
                RoomTypePrice roomTypePrice = null;
                roomTypePrice = await _unitOfWork.Repository<RoomTypePrice>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomTypePriceId == id)
                    .FirstOrDefaultAsync();

                if (roomTypePrice == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<RoomTypePrice, RoomTypePriceResponse>(roomTypePrice);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomTypePriceResponse> Create(RoomTypePriceRequest request)
        {
            try
            {
                var roomTypePrice = _mapper.Map<RoomTypePriceRequest, RoomTypePrice>(request);
                roomTypePrice.RoomTypePriceId = Guid.NewGuid();
                await _unitOfWork.Repository<RoomTypePrice>().InsertAsync(roomTypePrice);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomTypePrice, RoomTypePriceResponse>(roomTypePrice);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomTypePriceResponse> Delete(Guid id)
        {
            try
            {
                RoomTypePrice roomTypePrice = null;
                roomTypePrice = _unitOfWork.Repository<RoomTypePrice>()
                    .Find(p => p.RoomTypePriceId == id);
                if (roomTypePrice == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(roomTypePrice.RoomTypePriceId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomTypePrice, RoomTypePriceResponse>(roomTypePrice);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomTypePriceResponse> Update(Guid id, RoomTypePriceRequest request)
        {
            try
            {
                RoomTypePrice roomTypePrice = _unitOfWork.Repository<RoomTypePrice>()
                            .Find(x => x.RoomTypePriceId == id);
                if (roomTypePrice == null)
                {
                    throw new Exception();
                }
                roomTypePrice = _mapper.Map(request, roomTypePrice);

                await _unitOfWork.Repository<RoomTypePrice>().UpdateDetached(roomTypePrice);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomTypePrice, RoomTypePriceResponse>(roomTypePrice);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
