using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Hotels;
using FHotel.Service.DTOs.HotelVerifications;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.Services.Interfaces;
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
        private IHotelService _hotelService;
        public HotelVerificationService(IUnitOfWork unitOfWork, IMapper mapper, IHotelService hotelService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hotelService = hotelService;
        }

        public async Task<List<HotelVerificationResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<HotelVerification>().GetAll()
                                            .Include(x => x.Hotel)
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
                        .Include (x => x.Hotel)
                            .ThenInclude(x => x.District)
                                .ThenInclude(x => x.City)
                     .AsNoTracking()
                    .Where(x => x.HotelVerificationId == id)
                    .FirstOrDefaultAsync();

                if (hotelVerification == null)
                {
                    throw new Exception("Hotel verification not found");
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
            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                HotelVerification hotelVerification = _unitOfWork.Repository<HotelVerification>()
                            .Find(x => x.HotelVerificationId == id);
                if (hotelVerification == null)
                {
                    throw new Exception();
                }
                hotelVerification = _mapper.Map(request, hotelVerification);
                hotelVerification.UpdatedDate = localTime;
                hotelVerification.VerificationDate = localTime;
                var hotel = await  _hotelService.Get(hotelVerification.HotelId.Value);
                var updateHotel = new HotelUpdateRequest()
                {
                    HotelId = hotel.HotelId,
                    HotelName = hotel.HotelName,
                    OwnerEmail = hotel.OwnerEmail,
                    OwnerId = hotel.OwnerId,
                    OwnerIdentificationNumber = hotel.OwnerIdentificationNumber,
                    OwnerName = hotel.OwnerName,
                    OwnerPhoneNumber = hotel.OwnerPhoneNumber,
                    Address = hotel.Address,
                    Phone = hotel.Phone,
                    Email = hotel.Email,
                    Description = hotel.Description,
                    DistrictId = hotel.DistrictId,
                    UpdatedDate = hotel.UpdatedDate,
                    IsActive = hotel.IsActive,
                    CreatedDate = hotel.CreatedDate
                };
                if (hotel != null)
                {
                    if (hotelVerification.VerificationStatus == "Verified")
                    {
                        updateHotel.VerifyStatus = "Verified";
                    }   
                    if (hotelVerification.VerificationStatus == "Pending")
                    {
                        updateHotel.VerifyStatus = "Pending";
                    }
                    if (hotelVerification.VerificationStatus == "Rejected")
                    {
                        updateHotel.VerifyStatus = "Rejected";
                    }
                }
                
                await _unitOfWork.Repository<HotelVerification>().UpdateDetached(hotelVerification);
                await _unitOfWork.CommitAsync();
                await _hotelService.Update(hotel.HotelId, updateHotel);
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
