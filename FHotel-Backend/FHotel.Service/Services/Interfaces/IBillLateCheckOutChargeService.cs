using FHotel.Service.DTOs.BillLateCheckOutCharges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IBillLateCheckOutChargeService
    {
        public Task<List<BillLateCheckOutChargeResponse>> GetAll();

        public Task<BillLateCheckOutChargeResponse> Get(Guid id);

        public Task<BillLateCheckOutChargeResponse> Create(BillLateCheckOutChargeRequest request);

        public Task<BillLateCheckOutChargeResponse> Delete(Guid id);

        public Task<BillLateCheckOutChargeResponse> Update(Guid id, BillLateCheckOutChargeRequest request);
    }
}
