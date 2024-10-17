using FHotel.Service.DTOs.Amenities;
using FHotel.Services.DTOs.HotelAmenities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IHotelAmenityService
    {
        public Task<List<HotelAmenityResponse>> GetAll();

        public Task<HotelAmenityResponse> Get(Guid id);

        public Task<HotelAmenityResponse> Create(HotelAmenityRequest request);

        public Task<HotelAmenityResponse> Delete(Guid id);

        public Task<HotelAmenityResponse> Update(Guid id, HotelAmenityRequest request);

        public Task<string> UploadImage(IFormFile file);

        public Task<IEnumerable<AmenityResponse>> GetAllAmenityByHotelId(Guid hotelId);
    }
}
