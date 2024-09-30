using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<List<PaymentResponse>> GetAll();

        public Task<PaymentResponse> Get(Guid id);

        public Task<PaymentResponse> Create(PaymentRequest request);

        public Task<PaymentResponse> Delete(Guid id);

        public Task<PaymentResponse> Update(Guid id, PaymentRequest request);
    }
}
