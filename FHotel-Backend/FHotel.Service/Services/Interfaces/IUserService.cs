using FHotel.Service.DTOs.Users;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.Users;
using Microsoft.AspNetCore.Http;
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

        public Task<UserResponse> GetByEmail(string email);

        public Task<UserResponse> Create(UserCreateRequest request);

        public Task<UserResponse> Delete(Guid id);

        public Task<UserResponse> Update(Guid id, UserUpdateRequest request);

        public Task<UserResponse> Login(UserLoginRequest account);

        public Task<string> UploadImage(IFormFile file);

        public Task<UserResponse> Register(UserCreateRequest request);

        public Task SendActivationEmail(string toEmail);

        public Task ActivateUser(string email);

        public Task<List<HotelResponse>> GetHotelByUser(Guid id);

        public Task SendSMS(string phonenumber);
    }
}
