using FHotel.Service.DTOs.BillPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IBillPaymentService
    {
        public Task<List<BillPaymentResponse>> GetAll();

        public Task<BillPaymentResponse> Get(Guid id);

        public Task<BillPaymentResponse> Create(BillPaymentRequest request);

        public Task<BillPaymentResponse> Delete(Guid id);

        public Task<BillPaymentResponse> Update(Guid id, BillPaymentRequest request);
    }
}
