using FHotel.Service.DTOs.RefundPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IRefundPolicyService
    {
        public Task<List<RefundPolicyResponse>> GetAll();

        public Task<RefundPolicyResponse> Get(Guid id);

        public Task<RefundPolicyResponse> Create(RefundPolicyRequest request);

        public Task<RefundPolicyResponse> Delete(Guid id);

        public Task<RefundPolicyResponse> Update(Guid id, RefundPolicyRequest request);
    }
}
