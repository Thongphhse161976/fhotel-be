using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.UserValidator
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            // Validate FirstName: must not be empty
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vui lòng nhập tên.");
           
            // Validate Email: must not be empty and should be in a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Vui lòng nhập Email.")
                .EmailAddress().WithMessage("Địa chỉ Email không đúng định dạng.");

            // Validate Password: must not be empty
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc.");
            // Validate IdentificationNumber: must be 9 or 12 digits
            RuleFor(x => x.IdentificationNumber)
                .NotEmpty().WithMessage("Vui lòng nhập số căn cước.")
                .Matches(@"^\d{12}$").WithMessage("Số căn cước phải gồm 12 chữ số.");
            // Validate PhoneNumber: must be 10 or 11 digits
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Vui lòng nhập số điện thoại.")
                .Matches(@"^\d{10}$").WithMessage("Số điện thoại phải đúng 10 kí tự.");
            // Validate Address: must not be empty
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Vui lòng nhập địa chỉ.");
          
            // Validate RoleId: must not be empty
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required and must be selected.");
            
        }
    }
}
