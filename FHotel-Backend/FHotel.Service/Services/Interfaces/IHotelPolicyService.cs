using FHotel.Service.DTOs.HotelPolicies;
using FHotel.Service.DTOs.Hotels;
using FHotel.Services.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IHotelPolicyService
    {
        public Task<List<HotelPolicyResponse>> GetAll();

        public Task<HotelPolicyResponse> Get(Guid id);

        public Task<HotelPolicyResponse> Create(HotelPolicyRequest request);

        public Task<HotelPolicyResponse> Delete(Guid id);

        public Task<HotelPolicyResponse> Update(Guid id, HotelPolicyRequest request);
        public Task<List<HotelPolicyResponse>> GetAllHotelPolicyByHotelId(Guid id);
        public Task<List<HotelPolicyResponse>> GetAllHotelPolicyByPolicyId(Guid id);
    }
}
