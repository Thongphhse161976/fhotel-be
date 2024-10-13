using FHotel.Service.DTOs.HotelStaffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IHotelStaffService
    {
        public Task<List<HotelStaffResponse>> GetAll();

        public Task<HotelStaffResponse> Get(Guid id);

        public Task<HotelStaffResponse> Create(HotelStaffCreateteRequest request);

        public Task<HotelStaffResponse> Delete(Guid id);

        public Task<HotelStaffResponse> Update(Guid id, HotelStaffCreateteRequest request);
    }
}
