using FHotel.Service.DTOs.Services;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Services;
using FHotel.Services.DTOs.Users;
using Microsoft.AspNetCore.Http;
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

        public Task<ServiceResponse> Create(ServiceCreateRequest request);

        public Task<ServiceResponse> Delete(Guid id);

        public Task<ServiceResponse> Update(Guid id, ServiceUpdateRequest request);

        public Task<string> UploadImage(IFormFile file);

        public Task<List<ServiceResponse>> GetAllServiceByServiceTypeId(Guid serviceTypeId);


    }
}
