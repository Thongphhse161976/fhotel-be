using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.ServiceTypes;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FHotel.Services.Services.Implementations
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ServiceTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ServiceTypeResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<ServiceType>().GetAll()
                                            .ProjectTo<ServiceTypeResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<ServiceTypeResponse> Get(Guid id)
        {
            try
            {
                ServiceType serviceType = null;
                serviceType = await _unitOfWork.Repository<ServiceType>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.ServiceTypeId == id)
                    .FirstOrDefaultAsync();

                if (serviceType == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<ServiceType, ServiceTypeResponse>(serviceType);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceTypeResponse> Create(ServiceTypeRequest request)
        {
            try
            {
                var serviceType = _mapper.Map<ServiceTypeRequest, ServiceType>(request);
                serviceType.ServiceTypeId = Guid.NewGuid();
                await _unitOfWork.Repository<ServiceType>().InsertAsync(serviceType);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<ServiceType, ServiceTypeResponse>(serviceType);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceTypeResponse> Delete(Guid id)
        {
            try
            {
                ServiceType serviceType = null;
                serviceType = _unitOfWork.Repository<ServiceType>()
                    .Find(p => p.ServiceTypeId == id);
                if (serviceType == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<ServiceType>().HardDeleteGuid(serviceType.ServiceTypeId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<ServiceType, ServiceTypeResponse>(serviceType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceTypeResponse> Update(Guid id, ServiceTypeRequest request)
        {
            try
            {
                ServiceType serviceType = _unitOfWork.Repository<ServiceType>()
                            .Find(x => x.ServiceTypeId == id);
                if (serviceType == null)
                {
                    throw new Exception();
                }
                serviceType = _mapper.Map(request, serviceType);

                await _unitOfWork.Repository<ServiceType>().UpdateDetached(serviceType);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<ServiceType, ServiceTypeResponse>(serviceType);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

   
}
