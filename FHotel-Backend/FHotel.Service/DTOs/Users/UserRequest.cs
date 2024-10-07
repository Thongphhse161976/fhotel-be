﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Services.DTOs.Users
{
    public class UserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Image { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Sex { get; set; }
        public Guid? HotelId { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
    public class LoginMem
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
