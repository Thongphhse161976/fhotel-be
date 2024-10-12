using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.UserValidator
{
    public class UserUpdateStatusRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateStatusRequestValidator()
        {
          
        }
    }
}
