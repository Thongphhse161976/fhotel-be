﻿using FHotel.Service.DTOs.HotelRegistations;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.Services.Interfaces
{
    public interface IHotelRegistrationService
    {
        public Task<List<HotelRegistrationResponse>> GetAll();

        public Task<HotelRegistrationResponse> Get(Guid id);

        public Task<HotelRegistrationResponse> Create(HotelRegistrationCreateRequest request);

        public Task<HotelRegistrationResponse> Delete(Guid id);

        public Task<HotelRegistrationResponse> Update(Guid id, HotelRegistrationUpdateRequest request);

        public Task ApproveHotelRegistration(string email);

        public Task<HotelRegistrationResponse> GetByOwnerEmail(String? email);

        public Task SendEmail(string toEmail, UserResponse user);
    }
}
