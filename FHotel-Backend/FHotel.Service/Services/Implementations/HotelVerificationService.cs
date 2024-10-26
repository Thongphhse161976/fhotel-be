using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelVerifications;
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
    public class HotelVerificationService: IHotelVerificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public HotelVerificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<HotelVerificationResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelVerification>().GetAll()
                                            .ProjectTo<HotelVerificationResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<HotelVerificationResponse> Get(Guid id)
        {
            try
            {
                HotelVerification hotelVerification = null;
                hotelVerification = await _unitOfWork.Repository<HotelVerification>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.HotelVerificationId == id)
                    .FirstOrDefaultAsync();

                if (hotelVerification == null)
                {
                    throw new Exception("Hotel verifictaion not found");
                }

                return _mapper.Map<HotelVerification, HotelVerificationResponse>(hotelVerification);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelVerificationResponse> Create(HotelVerificationRequest request)
        {
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try 
            {
                var hotelVerification = _mapper.Map<HotelVerificationRequest, HotelVerification>(request);
                hotelVerification.HotelVerificationId = Guid.NewGuid();
                hotelVerification.CreatedDate = localTime; 
                hotelVerification.VerificationStatus = "Pending";
                await _unitOfWork.Repository<HotelVerification>().InsertAsync(hotelVerification);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelVerification, HotelVerificationResponse>(hotelVerification);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<HotelVerificationResponse> Delete(Guid id)
        {
            try
            {
                HotelVerification hotelVerification = null;
                hotelVerification = _unitOfWork.Repository<HotelVerification>()
                    .Find(p => p.HotelVerificationId == id);
                if (hotelVerification == null)
                {
                    throw new Exception("Hotel verifictaion not found");
                }
                await _unitOfWork.Repository<HotelVerification>().HardDeleteGuid(hotelVerification.HotelVerificationId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelVerification, HotelVerificationResponse>(hotelVerification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelVerificationResponse> Update(Guid id, HotelVerificationRequest request)
        {
            try
            {
                HotelVerification hotelVerification = _unitOfWork.Repository<HotelVerification>()
                            .Find(x => x.HotelVerificationId == id);
                if (hotelVerification == null)
                {
                    throw new Exception();
                }
                hotelVerification = _mapper.Map(request, hotelVerification);

                await _unitOfWork.Repository<HotelVerification>().UpdateDetached(hotelVerification);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<HotelVerification, HotelVerificationResponse>(hotelVerification);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<HotelVerificationResponse>> GetAllByHotelId(Guid id)
        {

            var list = await _unitOfWork.Repository<HotelVerification>().GetAll()
                                            .ProjectTo<HotelVerificationResponse>(_mapper.ConfigurationProvider)
                                            .Where(h=> h.HotelId == id)
                                            .ToListAsync();
            return list;
        }


        public async Task<List<HotelVerificationResponse>> GetAllByAssignManagerId(Guid id)
        {

            var list = await _unitOfWork.Repository<HotelVerification>().GetAll()
                                            .ProjectTo<HotelVerificationResponse>(_mapper.ConfigurationProvider)
                                            .Where(h => h.AssignedManagerId == id)
                                            .ToListAsync();
            return list;
        }
    }
}
