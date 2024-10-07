using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelRegistations;
using FHotel.Service.Validators.HotelResgistrationValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public async Task<HotelRegistrationResponse> Create(HotelRegistrationCreateRequest request)
        {
            // Validate the create request
            var validator = new HotelRegistrationCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var hotelRegistration = _mapper.Map<HotelRegistrationCreateRequest, HotelRegistration>(request);
                hotelRegistration.HotelRegistrationId = Guid.NewGuid();
                hotelRegistration.RegistrationDate = localTime;
                hotelRegistration.RegistrationStatus = "Pending";
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
                    throw new Exception("Not found");
                }
                await _unitOfWork.Repository<HotelRegistration>().HardDeleteGuid(hotelRegistration.HotelRegistrationId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
            }
            catch (Exception eu)
            {
                throw new Exception(eu.Message);
            }
        }

        public async Task<HotelRegistrationResponse> Update(Guid id, HotelRegistrationUpdateRequest request)
        {
            // Validate the update request
            var validator = new HotelRegistrationUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            // Proceed with hotel registration update
            var hotelRegistration = _unitOfWork.Repository<HotelRegistration>()
                .Find(x => x.HotelRegistrationId == id); // Use '==' for comparison

            if (hotelRegistration == null)
            {
                throw new Exception("Hotel registration not found.");
            }

            // Update fields
            hotelRegistration.OwnerId = request.OwnerId;
            hotelRegistration.NumberOfHotels = request.NumberOfHotels;
            hotelRegistration.Description = request.Description;
            hotelRegistration.RegistrationStatus = request.RegistrationStatus;

            await _unitOfWork.Repository<HotelRegistration>().UpdateDetached(hotelRegistration);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<HotelRegistration, HotelRegistrationResponse>(hotelRegistration);
        }


    }
}
