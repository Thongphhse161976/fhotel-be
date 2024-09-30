using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IServiceService
    {
        public Task<List<ServiceResponse>> GetAll();

        public Task<ServiceResponse> Get(Guid id);

        public Task<ServiceResponse> Create(ServiceRequest request);

        public Task<ServiceResponse> Delete(Guid id);

        public Task<ServiceResponse> Update(Guid id, ServiceRequest request);
    }
}
