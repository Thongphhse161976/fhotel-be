using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Services;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ServiceResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Repository.Models.Service>().GetAll()
                                            .ProjectTo<ServiceResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<ServiceResponse> Get(Guid id)
        {
            try
            {
                Repository.Models.Service service = null;
                service = await _unitOfWork.Repository<Repository.Models.Service>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.ServiceId == id)
                    .FirstOrDefaultAsync();

                if (service == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceResponse> Create(ServiceRequest request)
        {
            try
            {
                var service = _mapper.Map<ServiceRequest, Repository.Models.Service>(request);
                service.ServiceId = Guid.NewGuid();
                await _unitOfWork.Repository<Repository.Models.Service>().InsertAsync(service);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<ServiceResponse> Delete(Guid id)
        {
            try
            {
                Repository.Models.Service service = null;
                service = _unitOfWork.Repository<Repository.Models.Service>()
                    .Find(p => p.ServiceId == id);
                if (service == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Role>().HardDeleteGuid(service.ServiceId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ServiceResponse> Update(Guid id, ServiceRequest request)
        {
            try
            {
                Repository.Models.Service service = _unitOfWork.Repository<Repository.Models.Service>()
                            .Find(x => x.ServiceId == id);
                if (service == null)
                {
                    throw new Exception();
                }
                service = _mapper.Map(request, service);

                await _unitOfWork.Repository<Repository.Models.Service>().UpdateDetached(service);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Repository.Models.Service, ServiceResponse>(service);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
