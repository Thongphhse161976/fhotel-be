using FHotel.Service.DTOs.RoomTypes;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.RoomTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoomTypeService
    {
        public Task<List<RoomTypeResponse>> GetAll();

        public Task<RoomTypeResponse> Get(Guid id);

        public Task<RoomTypeResponse> Create(RoomTypeCreateRequest request);

        public Task<RoomTypeResponse> Delete(Guid id);

        public Task<RoomTypeResponse> Update(Guid id, RoomTypeUpdateRequest request);


        public Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypeByHotelId(Guid hotelId);

        public  Task<IEnumerable<HotelResponse>> SearchHotelsWithRoomTypes(List<RoomSearchRequest> searchRequests, string? cityName);

    }
}
