using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Facilities;
using FHotel.Service.Validators.RoomFacilityValidator;
using FHotel.Services.DTOs.RoomFacilities;
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
    public class RoomFacilityService : IRoomFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public RoomFacilityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RoomFacilityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<RoomFacility>().GetAll()
                                            .ProjectTo<RoomFacilityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<RoomFacilityResponse> Get(Guid id)
        {
            try
            {
                RoomFacility roomFacility = null;
                roomFacility = await _unitOfWork.Repository<RoomFacility>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.RoomFacilityId == id)
                    .FirstOrDefaultAsync();

                if (roomFacility == null)
                {
                    throw new Exception("Room facility not found");
                }

                return _mapper.Map<RoomFacility, RoomFacilityResponse>(roomFacility);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomFacilityResponse> Create(RoomFacilityRequest request)
        {
            var validator = new RoomFacilityRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var roomFacility = _mapper.Map<RoomFacilityRequest, RoomFacility>(request);
                roomFacility.RoomFacilityId = Guid.NewGuid();
                await _unitOfWork.Repository<RoomFacility>().InsertAsync(roomFacility);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomFacility, RoomFacilityResponse>(roomFacility);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<RoomFacilityResponse> Delete(Guid id)
        {
            try
            {
                RoomFacility roomFacility = null;
                roomFacility = _unitOfWork.Repository<RoomFacility>()
                    .Find(p => p.RoomFacilityId == id);
                if (roomFacility == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<RoomFacility>().HardDeleteGuid(roomFacility.RoomFacilityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<RoomFacility, RoomFacilityResponse>(roomFacility);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomFacilityResponse> Update(Guid id, RoomFacilityRequest request)
        {
            var validator = new RoomFacilityRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                RoomFacility roomFacility = _unitOfWork.Repository<RoomFacility>()
                            .Find(x => x.RoomFacilityId == id);
                if (roomFacility == null)
                {
                    throw new Exception();
                }
                roomFacility = _mapper.Map(request, roomFacility);

                await _unitOfWork.Repository<RoomFacility>().UpdateDetached(roomFacility);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<RoomFacility, RoomFacilityResponse>(roomFacility);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<RoomFacilityResponse>> GetAllRoomFacilityByRoomTypeId(Guid id)
        {
            try
            {
                var list = await _unitOfWork.Repository<RoomFacility>().GetAll()
                                            .ProjectTo<RoomFacilityResponse>(_mapper.ConfigurationProvider)
                                            .Where(x=> x.RoomTypeId == id)
                                            .ToListAsync();
                return list;
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
