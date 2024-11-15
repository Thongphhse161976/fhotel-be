using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.UserValidator
{
    public class UserRegisterRequestValidator: AbstractValidator<UserCreateRequest>
    {
        public UserRegisterRequestValidator()
        {
            // Validate FirstName: must not be empty
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Bạn chưa nhập tên.");
            // Validate Email: must not be empty and should be in a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Bạn chưa nhập Email.")
                .EmailAddress().WithMessage("Email chưa đúng định dạng.");

            // Validate Password: must not be empty
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Bạn chưa nhập mật khẩu.");
            // Validate IdentificationNumber: must be 9 or 12 digits
            RuleFor(x => x.IdentificationNumber)
                .NotEmpty().WithMessage("Bạn chưa nhập số căn cước.")
                .Matches(@"^\d{12}$").WithMessage("Số căn cước phải gồm 12 chữ số.");
            // Validate PhoneNumber: must be 10 or 11 digits
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Bạn chưa nhập số điện thoại.")
                .Matches(@"^\d{10}$").WithMessage("Số điện thoại phải đúng 10 kí tự.");
            // Validate Address: must not be empty
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Bạn chưa nhập địa chỉ.");
            
        }
    }
}
