using FHotel.Services.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<List<RoleResponse>> GetAll();

        public Task<RoleResponse> Get(Guid id);

        public Task<RoleResponse> Create(RoleRequest request);

        public Task<RoleResponse> Delete(Guid id);

        public Task<RoleResponse> Update(Guid id, RoleRequest request);
    }
}
