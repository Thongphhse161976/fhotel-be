using FHotel.Service.DTOs.BillTransactionImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IBillTransactionImageService
    {
        public Task<List<BillTransactionImageResponse>> GetAll();

        public Task<BillTransactionImageResponse> Get(Guid id);

        public Task<BillTransactionImageResponse> Create(BillTransactionImageRequest request);

        public Task<BillTransactionImageResponse> Delete(Guid id);

        public Task<BillTransactionImageResponse> Update(Guid id, BillTransactionImageRequest request);
    }
}
