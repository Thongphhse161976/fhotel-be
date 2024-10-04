using FHotel.Service.DTOs.LateCheckOutCharges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface ILateCheckOutChargeService
    {
        public Task<List<LateCheckOutChargeResponse>> GetAll();

        public Task<LateCheckOutChargeResponse> Get(Guid id);

        public Task<LateCheckOutChargeResponse> Create(LateCheckOutChargeRequest request);

        public Task<LateCheckOutChargeResponse> Delete(Guid id);

        public Task<LateCheckOutChargeResponse> Update(Guid id, LateCheckOutChargeRequest request);
    }
}
