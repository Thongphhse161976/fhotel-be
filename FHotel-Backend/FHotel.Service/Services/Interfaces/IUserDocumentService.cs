using FHotel.Services.DTOs.UserDocuments;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface IUserDocumentService
    {
        public Task<List<UserDocumentResponse>> GetAll();

        public Task<UserDocumentResponse> Get(Guid id);

        public Task<UserDocumentResponse> Create(UserDocumentRequest request);

        public Task<UserDocumentResponse> Delete(Guid id);

        public Task<UserDocumentResponse> Update(Guid id, UserDocumentRequest request);

        public Task<string> UploadImage(IFormFile file);

        public Task<IEnumerable<UserDocumentResponse>> GetAllUserDocumentByReservationId(Guid reservationId);
    }
}
