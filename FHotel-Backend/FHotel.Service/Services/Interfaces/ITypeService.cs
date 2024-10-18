using FHotel.Service.DTOs.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Services.Interfaces
{
    public interface ITypeService
    {
        public Task<List<TypeResponse>> GetAll();

        public Task<TypeResponse> Get(Guid id);

        public Task<TypeResponse> Create(TypeCreateRequest request);

        public Task<TypeResponse> Delete(Guid id);

        public Task<TypeResponse> Update(Guid id, TypeUpdateRequest request);
    }
}
