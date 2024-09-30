using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Hotel>().GetAll()
                                            .ProjectTo<HotelResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelResponse> Get(Guid id)
        {
            try
            {
                Hotel hotel = null;
                hotel = await _unitOfWork.Repository<Hotel>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelId == id)
                    .FirstOrDefaultAsync();

                if (hotel == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelResponse> Create(HotelRequest request)
        {
            try
            {
                var hotel = _mapper.Map<HotelRequest, Hotel>(request);
                hotel.HotelId = Guid.NewGuid();
                await _unitOfWork.Repository<Hotel>().InsertAsync(hotel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelResponse> Delete(Guid id)
        {
            try
            {
                Hotel hotel = null;
                hotel = _unitOfWork.Repository<Hotel>()
                    .Find(p => p.HotelId == id);
                if (hotel == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(hotel.HotelId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelResponse> Update(Guid id, HotelRequest request)
        {
            try
            {
                Hotel hotel = _unitOfWork.Repository<Hotel>()
                            .Find(x => x.HotelId == id);
                if (hotel == null)
                {
                    throw new Exception();
                }
                hotel = _mapper.Map(request, hotel);

                await _unitOfWork.Repository<Hotel>().UpdateDetached(hotel);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
