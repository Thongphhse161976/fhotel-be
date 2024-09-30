using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class HotelAmenityService : IHotelAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelAmenityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelAmenityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelAmenity>().GetAll()
                                            .ProjectTo<HotelAmenityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelAmenityResponse> Get(Guid id)
        {
            try
            {
                HotelAmenity hotelAmenity = null;
                hotelAmenity = await _unitOfWork.Repository<HotelAmenity>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelAmenityId == id)
                    .FirstOrDefaultAsync();

                if (hotelAmenity == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelAmenityResponse> Create(HotelAmenityRequest request)
        {
            try
            {
                var hotelAmenity = _mapper.Map<HotelAmenityRequest, HotelAmenity>(request);
                hotelAmenity.HotelAmenityId = Guid.NewGuid();
                await _unitOfWork.Repository<HotelAmenity>().InsertAsync(hotelAmenity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelAmenityResponse> Delete(Guid id)
        {
            try
            {
                HotelAmenity hotelAmenity = null;
                hotelAmenity = _unitOfWork.Repository<HotelAmenity>()
                    .Find(p => p.HotelAmenityId == id);
                if (hotelAmenity == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(hotelAmenity.HotelAmenityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelAmenityResponse> Update(Guid id, HotelAmenityRequest request)
        {
            try
            {
                HotelAmenity hotelAmenity = _unitOfWork.Repository<HotelAmenity>()
                            .Find(x => x.HotelAmenityId == id);
                if (hotelAmenity == null)
                {
                    throw new Exception();
                }
                hotelAmenity = _mapper.Map(request, hotelAmenity);

                await _unitOfWork.Repository<HotelAmenity>().UpdateDetached(hotelAmenity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelAmenity, HotelAmenityResponse>(hotelAmenity);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
