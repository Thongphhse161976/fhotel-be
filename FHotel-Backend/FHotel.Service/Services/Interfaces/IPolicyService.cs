using FHotel.Service.DTOs.Hotels;
using FHotel.Service.DTOs.Policies;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IPolicyService
    {
        public Task<List<PolicyResponse>> GetAll();

        public Task<PolicyResponse> Get(Guid id);

        public Task<PolicyResponse> Create(PolicyRequest request);

        public Task<PolicyResponse> Delete(Guid id);

        public Task<PolicyResponse> Update(Guid id, PolicyRequest request);
    }
}
