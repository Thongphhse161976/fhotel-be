using FHotel.Service.DTOs.LateCheckOutPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface ILateCheckOutPolicyService
    {
        public Task<List<LateCheckOutPolicyResponse>> GetAll();

        public Task<LateCheckOutPolicyResponse> Get(Guid id);

        public Task<LateCheckOutPolicyResponse> Create(LateCheckOutPolicyRequest request);

        public Task<LateCheckOutPolicyResponse> Delete(Guid id);

        public Task<LateCheckOutPolicyResponse> Update(Guid id, LateCheckOutPolicyRequest request);
    }
}
