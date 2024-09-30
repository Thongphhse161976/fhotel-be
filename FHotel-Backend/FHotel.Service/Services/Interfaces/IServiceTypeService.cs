using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.ServiceTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IServiceTypeService
    {
        public Task<List<ServiceTypeResponse>> GetAll();

        public Task<ServiceTypeResponse> Get(Guid id);

        public Task<ServiceTypeResponse> Create(ServiceTypeRequest request);

        public Task<ServiceTypeResponse> Delete(Guid id);

        public Task<ServiceTypeResponse> Update(Guid id, ServiceTypeRequest request);
    }
}
