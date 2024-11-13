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

        public Task<BillResponse> GetBillByReservation(Guid id);

        public Task<List<BillResponse>> GetAllBillByStaffId(Guid staffId);

        public Task<List<BillResponse>> GetAllByOwnerId(Guid id);
        //public Task CheckAndProcessBillsAsync();
        //public Task ProcessBillAsync(BillResponse bill);

        //public Task<BillResponse[]> GetEligibleBillsAsync();

        //public bool Is60SecondsAfterBill(BillResponse bill);
    }
}
