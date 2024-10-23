using FHotel.Service.DTOs.Amenities;
using FHotel.Services.DTOs.HotelDocuments;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IHotelDocumentService
    {
        public Task<List<HotelDocumentResponse>> GetAll();

        public Task<HotelDocumentResponse> Get(Guid id);

        public Task<HotelDocumentResponse> Create(HotelDocumentRequest request);

        public Task<HotelDocumentResponse> Delete(Guid id);

        public Task<HotelDocumentResponse> Update(Guid id, HotelDocumentRequest request);

        public Task<string> UploadImage(IFormFile file);

        public Task<IEnumerable<HotelDocumentResponse>> GetAllHotelDocumentByHotelId(Guid hotelId);
    }
}