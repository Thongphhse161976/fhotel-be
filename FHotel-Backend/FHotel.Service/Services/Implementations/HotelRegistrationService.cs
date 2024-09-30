using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class HotelRegistrationService : IHotelRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelRegistrationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelRegistrationResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelRegistration>().GetAll()
                                            .ProjectTo<HotelRegistrationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelRegistrationResponse> Get(Guid id)
        {
            try
            {
                HotelRegistration hotelRegistration = null;
                hotelRegistration = await _unitOfWork.Repository<HotelRegistration>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelRegistrationId == id)
                    .FirstOrDefaultAsync();

                if (hotelRegistration == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelRegistrationResponse> Create(HotelRegistrationRequest request)
        {
            try
            {
                var hotelRegistration = _mapper.Map<HotelRegistrationRequest, HotelRegistration>(request);
                hotelRegistration.HotelRegistrationId = Guid.NewGuid();
                await _unitOfWork.Repository<HotelRegistration>().InsertAsync(hotelRegistration);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelRegistrationResponse> Delete(Guid id)
        {
            try
            {
                HotelRegistration hotelRegistration = null;
                hotelRegistration = _unitOfWork.Repository<HotelRegistration>()
                    .Find(p => p.HotelRegistrationId == id);
                if (hotelRegistration == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(hotelRegistration.HotelRegistrationId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelRegistrationResponse> Update(Guid id, HotelRegistrationRequest request)
        {
            try
            {
                HotelRegistration hotelRegistration = _unitOfWork.Repository<HotelRegistration>()
                            .Find(x => x.HotelRegistrationId == id);
                if (hotelRegistration == null)
                {
                    throw new Exception();
                }
                hotelRegistration = _mapper.Map(request, hotelRegistration);

                await _unitOfWork.Repository<HotelRegistration>().UpdateDetached(hotelRegistration);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
