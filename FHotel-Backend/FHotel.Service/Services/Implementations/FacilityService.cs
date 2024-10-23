using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Facilities;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.AmenityValidator;
using FHotel.Service.Validators.FacilityValidator;
using FHotel.Services.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class FacilityService: IFacilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public FacilityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FacilityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Facility>().GetAll()
                                            .ProjectTo<FacilityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<FacilityResponse> Get(Guid id)
        {
            try
            {
                Facility facility = null;
                facility = await _unitOfWork.Repository<Facility>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.FacilityId == id)
                    .FirstOrDefaultAsync();

                if (facility == null)
                {
                    throw new Exception("Facility not found");
                }

                return _mapper.Map<Facility, FacilityResponse>(facility);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<FacilityResponse> Create(FacilityRequest request)
        {
            var validator = new FacilityRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                
                var facility = _mapper.Map<FacilityRequest, Facility>(request);
                facility.FacilityId = Guid.NewGuid();
                await _unitOfWork.Repository<Facility>().InsertAsync(facility);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Facility, FacilityResponse>(facility);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<FacilityResponse> Delete(Guid id)
        {
            try
            {
                Facility facility = null;
                facility = _unitOfWork.Repository<Facility>()
                    .Find(p => p.FacilityId == id);
                if (facility == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Facility>().HardDeleteGuid(facility.FacilityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Facility, FacilityResponse>(facility);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FacilityResponse> Update(Guid id, FacilityRequest request)
        {
            var validator = new FacilityRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                
                Facility facility = _unitOfWork.Repository<Facility>()
                            .Find(x => x.FacilityId == id);
                if (facility == null)
                {
                    throw new Exception();
                }
                facility = _mapper.Map(request, facility);

                await _unitOfWork.Repository<Facility>().UpdateDetached(facility);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Facility, FacilityResponse>(facility);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
