using FHotel.Service.DTOs.CancellationPolicies;
using FHotel.Service.DTOs.RevenuePolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface ICancellationPolicyService
    {
        public Task<List<CancellationPolicyResponse>> GetAll();

        public Task<CancellationPolicyResponse> Get(Guid id);

        public Task<CancellationPolicyResponse> Create(CancellationPolicyRequest request);

        public Task<CancellationPolicyResponse> Delete(Guid id);

        public Task<CancellationPolicyResponse> Update(Guid id, CancellationPolicyRequest request);

        public Task<List<CancellationPolicyResponse>> GetAllCancellationPolicyByHotelId(Guid id);
    }
}
