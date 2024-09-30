using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IDocumentService
    {
        public Task<List<DocumentResponse>> GetAll();

        public Task<DocumentResponse> Get(Guid id);

        public Task<DocumentResponse> Create(DocumentRequest request);

        public Task<DocumentResponse> Delete(Guid id);

        public Task<DocumentResponse> Update(Guid id, DocumentRequest request);
    }
}
