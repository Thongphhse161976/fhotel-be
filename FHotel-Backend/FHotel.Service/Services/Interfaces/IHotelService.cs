using FHotel.Service.DTOs.Hotels;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.Hotels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IHotelService 
    {
        public Task<List<HotelResponse>> GetAll();

        public Task<HotelResponse> Get(Guid id);

        public Task<HotelResponse> Create(HotelCreateRequest request);

        public Task<HotelResponse> Delete(Guid id);

        public Task<HotelResponse> Update(Guid id, HotelUpdateRequest request);

        public Task<string> UploadImage(IFormFile file);

        public Task<List<HotelAmenityResponse>> GetHotelAmenityByHotel(Guid id);

        public Task<HotelResponse> CreateMore(HotelRequest request);

        public Task<List<HotelResponse>> GetHotelsWithAvailableRoomTypesInRangeAsync(DateTime checkIn, DateTime checkOut);
    }
}
