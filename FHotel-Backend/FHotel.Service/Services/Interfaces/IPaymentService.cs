using FHotel.Service.DTOs.Amenities;
using FHotel.Service.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<List<PaymentResponse>> GetAll();

        public Task<PaymentResponse> Get(Guid id);

        public Task<PaymentResponse> Create(PaymentRequest request);

        public Task<PaymentResponse> Delete(Guid id);

        public Task<PaymentResponse> Update(Guid id, PaymentRequest request);

        public Task<List<PaymentResponse>> GetAllPaymentByBillId(Guid id);
    }
}
