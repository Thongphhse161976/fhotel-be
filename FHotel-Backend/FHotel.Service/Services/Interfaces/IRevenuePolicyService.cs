using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.RevenuePolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IRevenuePolicyService
    {
        public Task<List<RevenuePolicyResponse>> GetAll();

        public Task<RevenuePolicyResponse> Get(Guid id);

        public Task<RevenuePolicyResponse> Create(RevenuePolicyRequest request);

        public Task<RevenuePolicyResponse> Delete(Guid id);

        public Task<RevenuePolicyResponse> Update(Guid id, RevenuePolicyRequest request);

        public Task<List<RevenuePolicyResponse>> GetAllRevenuePolicyByHotelId(Guid id);
    }
}
