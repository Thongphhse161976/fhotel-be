using AutoMapper;
using AutoMapper.QueryableExtensions;
using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Documents;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DocumentResponse>> GetAll()
        {

            var list = await _unitOfWork.Repository<Document>().GetAll()
                                            .ProjectTo<DocumentResponse>(_mapper.ConfigurationProvider)
                                            .ToListAsync();
            return list;
        }

        public async Task<DocumentResponse> Get(Guid id)
        {
            try
            {
                Document document = null;
                document = await _unitOfWork.Repository<Document>().GetAll()
                     .AsNoTracking()
                    .Where(x => x.DocumentId == id)
                    .FirstOrDefaultAsync();

                if (document == null)
                {
                    throw new Exception("khong tim thay");
                }

                return _mapper.Map<Document, DocumentResponse>(document);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DocumentResponse> Create(DocumentRequest request)
        {
            try
            {
                var document = _mapper.Map<DocumentRequest, Document>(request);
                document.DocumentId = Guid.NewGuid();
                await _unitOfWork.Repository<Document>().InsertAsync(document);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Document, DocumentResponse>(document);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<DocumentResponse> Delete(Guid id)
        {
            try
            {
                Document document = null;
                document = _unitOfWork.Repository<Document>()
                    .Find(p => p.DocumentId == id);
                if (document == null)
                {
                    throw new Exception("Bi trung id");
                }
                await _unitOfWork.Repository<Document>().HardDeleteGuid(document.DocumentId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<Document, DocumentResponse>(document);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<DocumentResponse> Update(Guid id, DocumentRequest request)
        {
            try
            {
                Document document = _unitOfWork.Repository<Document>()
                            .Find(x => x.DocumentId == id);
                if (document == null)
                {
                    throw new Exception();
                }
                document = _mapper.Map(request, document);

                await _unitOfWork.Repository<Document>().UpdateDetached(document);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<Document, DocumentResponse>(document);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
