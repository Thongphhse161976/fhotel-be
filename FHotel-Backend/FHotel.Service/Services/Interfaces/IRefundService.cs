using FHotel.Service.DTOs.Refunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IRefundService
    {
        public Task<List<RefundResponse>> GetAll();

        public Task<RefundResponse> Get(Guid id);

        public Task<RefundResponse> Create(RefundRequest request);

        public Task<RefundResponse> Delete(Guid id);

        public Task<RefundResponse> Update(Guid id, RefundRequest request);
    }
}
