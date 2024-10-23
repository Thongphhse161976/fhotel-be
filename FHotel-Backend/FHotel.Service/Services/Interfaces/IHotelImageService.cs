using FHotel.Services.DTOs.HotelImages;
using FHotel.Services.DTOs.RoomImages;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Interfaces
{
    public interface IHotelImageService
    {
        public Task<List<HotelImageResponse>> GetAll();

        public Task<HotelImageResponse> Get(Guid id);

        public Task<HotelImageResponse> Create(HotelImageRequest request);

        public Task<HotelImageResponse> Delete(Guid id);

        public Task<HotelImageResponse> Update(Guid id, HotelImageRequest request);
        public Task<IEnumerable<HotelImageResponse>> GetAllHotelImageByHotelId(Guid hotelId);
        public Task<string> UploadImage(IFormFile file);
    }
}
