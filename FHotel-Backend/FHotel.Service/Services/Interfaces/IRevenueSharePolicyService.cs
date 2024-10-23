using FHotel.Services.DTOs.RevenueSharePolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IRevenueSharePolicyService
    {
        public Task<List<RevenueSharePolicyResponse>> GetAll();

        public Task<RevenueSharePolicyResponse> Get(Guid id);

        public Task<RevenueSharePolicyResponse> Create(RevenueSharePolicyRequest request);

        public Task<RevenueSharePolicyResponse> Delete(Guid id);

        public Task<RevenueSharePolicyResponse> Update(Guid id, RevenueSharePolicyRequest request);
    }
}
