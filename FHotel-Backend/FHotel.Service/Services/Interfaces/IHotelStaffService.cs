using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Services.DTOs.RoomTypes;
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

        public Task<HotelStaffResponse> Create(Guid hotelId, Guid userId);

        public Task<HotelStaffResponse> Delete(Guid id);

        public Task<HotelStaffResponse> Update(Guid id, HotelStaffCreateRequest request);

        public Task<IEnumerable<HotelStaffResponse>> GetAllStaffByHotelId(Guid hotelId);

    }
}
