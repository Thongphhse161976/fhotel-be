using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.HotelRegistations;
using FHotel.Service.DTOs.Hotels;
using FHotel.Service.Validators.HotelResgistrationValidator;
using FHotel.Service.Validators.HotelValidator;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
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

        public async Task<HotelResponse> Create(HotelCreateRequest request)
        {
            // Validate the create request
            var validator = new HotelCreateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }



            // Set the UTC offset for UTC+7
            TimeSpan utcOffset = TimeSpan.FromHours(7);

            // Get the current UTC time
            DateTime utcNow = DateTime.UtcNow;

            // Convert the UTC time to UTC+7
            DateTime localTime = utcNow + utcOffset;
            try
            {
                var hotel = _mapper.Map<HotelCreateRequest, Hotel>(request);
                hotel.HotelId = Guid.NewGuid();
                hotel.CreatedDate = localTime;
                hotel.IsActive = true;
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
                await _unitOfWork.Repository<Hotel>().HardDeleteGuid(hotel.HotelId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Hotel, HotelResponse>(hotel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelResponse> Update(Guid id, HotelUpdateRequest request)
        {
            // Validate the update request
            var validator = new HotelUpdateRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                // Combine validation errors into a single message
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }
            try
            {
                Hotel hotel = _unitOfWork.Repository<Hotel>()
                            .Find(x => x.HotelId == id);
                if (hotel == null)
                {
                    throw new Exception("Not Found");
                }
                hotel = _mapper.Map(request, hotel);

                // Set the UTC offset for UTC+7
                TimeSpan utcOffset = TimeSpan.FromHours(7);

                // Get the current UTC time
                DateTime utcNow = DateTime.UtcNow;

                // Convert the UTC time to UTC+7
                DateTime localTime = utcNow + utcOffset;

                // Update fields
                hotel.HotelName = request.HotelName ?? hotel.HotelName;
                hotel.Address = request.Address ?? hotel.Address;
                hotel.Phone = request.Phone ?? hotel.Phone;
                hotel.Email = request.Email ?? hotel.Email;
                hotel.Description = request.Description ?? hotel.Description;
                hotel.Star = request.Star ?? hotel.Star;
                hotel.CityId = request.CityId ?? hotel.CityId;
                hotel.OwnerId = request.OwnerId ?? hotel.OwnerId;
                hotel.UpdatedDate = localTime; // Ensure you update this field automatically
                hotel.IsActive = request.IsActive ?? hotel.IsActive;


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
