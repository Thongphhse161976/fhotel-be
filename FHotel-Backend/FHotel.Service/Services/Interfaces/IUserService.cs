using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserResponse>> GetAll();

        public Task<UserResponse> Get(Guid id);

        public Task<UserResponse> Create(UserRequest request);

        public Task<UserResponse> Delete(Guid id);

        public Task<UserResponse> Update(Guid id, UserRequest request);

        public Task<UserResponse> Login(LoginMem account);
    }
}
