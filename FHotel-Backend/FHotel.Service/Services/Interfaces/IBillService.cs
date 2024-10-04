using FHotel.Service.DTOs.Bills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IBillService
    {
        public Task<List<BillResponse>> GetAll();

        public Task<BillResponse> Get(Guid id);

        public Task<BillResponse> Create(BillRequest request);

        public Task<BillResponse> Delete(Guid id);

        public Task<BillResponse> Update(Guid id, BillRequest request);
    }
}
