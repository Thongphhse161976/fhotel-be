using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.PaymentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        public Task<List<PaymentMethodResponse>> GetAll();

        public Task<PaymentMethodResponse> Get(Guid id);

        public Task<PaymentMethodResponse> Create(PaymentMethodRequest request);

        public Task<PaymentMethodResponse> Delete(Guid id);

        public Task<PaymentMethodResponse> Update(Guid id, PaymentMethodRequest request);
    }
}
