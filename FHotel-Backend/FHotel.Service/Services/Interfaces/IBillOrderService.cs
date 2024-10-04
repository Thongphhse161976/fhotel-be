using FHotel.Service.DTOs.BillOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IBillOrderService
    {
        public Task<List<BillOrderResponse>> GetAll();

        public Task<BillOrderResponse> Get(Guid id);

        public Task<BillOrderResponse> Create(BillOrderRequest request);

        public Task<BillOrderResponse> Delete(Guid id);

        public Task<BillOrderResponse> Update(Guid id, BillOrderRequest request);
    }
}
