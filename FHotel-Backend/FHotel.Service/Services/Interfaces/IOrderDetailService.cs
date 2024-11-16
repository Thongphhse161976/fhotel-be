using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.UserDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IOrderDetailService
    {
        public Task<List<OrderDetailResponse>> GetAll();

        public Task<OrderDetailResponse> Get(Guid id);

        public Task<OrderDetailResponse> Create(OrderDetailRequest request);

        public Task<OrderDetailResponse> Delete(Guid id);

        public Task<OrderDetailResponse> Update(Guid id, OrderDetailRequest request);
        public Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailByOrder(Guid orderId);
        public Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailByReservation(Guid reservationId);
        public Task<IEnumerable<OrderDetailResponse>> GetAllOrderDetailByUser(Guid userId);
        public Task<IEnumerable<OrderDetailResponse>> GetAllRefund();

    }
}
