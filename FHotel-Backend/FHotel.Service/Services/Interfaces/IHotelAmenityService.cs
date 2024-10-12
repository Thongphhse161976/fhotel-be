using FHotel.Service.DTOs.HotelAmenities;
using FHotel.Services.DTOs.Cities;
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

        public Task<HotelAmenityResponse> Create(HotelAmenityCreateRequest request);

        public Task<HotelAmenityResponse> Delete(Guid id);

        public Task<HotelAmenityResponse> Update(Guid id, HotelAmenityUpdateRequest request);

        public Task<string> UploadImage(IFormFile file);
       

    }
}
