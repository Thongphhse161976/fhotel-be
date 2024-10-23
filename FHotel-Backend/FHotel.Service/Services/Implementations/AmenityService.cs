using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Amenities;
using FHotel.Service.Services.Interfaces;
using FHotel.Service.Validators.AmenityValidator;
using FHotel.Service.Validators.UserValidator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Implementations
{
    public class AmenityService: IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public AmenityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AmenityResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Amenity>().GetAll()
                                            .ProjectTo<AmenityResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<AmenityResponse> Get(Guid id)
        {
            try
            {
                Amenity amenity = null;
                amenity = await _unitOfWork.Repository<Amenity>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.AmenityId == id)
                    .FirstOrDefaultAsync();

                if (amenity == null)
                {
                    throw new Exception("Amenity not found");
                }

                return _mapper.Map<Amenity, AmenityResponse>(amenity);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AmenityResponse> Create(AmenityRequest request)
        {
            var validator = new AmenityRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                var amenity = _mapper.Map<AmenityRequest, Amenity>(request);
                amenity.AmenityId = Guid.NewGuid();
                await _unitOfWork.Repository<Amenity>().InsertAsync(amenity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Amenity, AmenityResponse>(amenity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<AmenityResponse> Delete(Guid id)
        {
            try
            {
                Amenity amenity = null;
                amenity = _unitOfWork.Repository<Amenity>()
                    .Find(p => p.AmenityId == id);
                if (amenity == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Amenity>().HardDeleteGuid(amenity.AmenityId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Amenity, AmenityResponse>(amenity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AmenityResponse> Update(Guid id, AmenityRequest request)
        {
            var validator = new AmenityRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            // If there are any validation errors, throw a ValidationException
            if (validationResult.Errors.Any())
            {
                throw new ValidationException(validationResult.Errors);
            }
            try
            {
                Amenity amenity = _unitOfWork.Repository<Amenity>()
                            .Find(x => x.AmenityId == id);
                if (amenity == null)
                {
                    throw new Exception();
                }
                amenity = _mapper.Map(request, amenity);

                await _unitOfWork.Repository<Amenity>().UpdateDetached(amenity);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Amenity, AmenityResponse>(amenity);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
