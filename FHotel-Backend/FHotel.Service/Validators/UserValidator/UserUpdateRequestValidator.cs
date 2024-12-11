using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.UserValidator
{
    public class UserUpdateRequestValidator: AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            // Validate FirstName: must not be empty if provided
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên không được để trống.");


            // Validate Email: must be in a valid format if provided
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Sai định dạng email.");

            // Validate Password: must not be empty if provided
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.");

            // Validate IdentificationNumber: must be 9 or 12 digits if provided
            RuleFor(x => x.IdentificationNumber)
                .NotEmpty().WithMessage("Bạn chưa nhập số căn cước.")
                .Matches(@"^\d{12}$").WithMessage("Số căn cước phải gồm 12 chữ số.");

            // Validate PhoneNumber: must be 10 or 11 digits if provided
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Bạn chưa nhập số điện thoại.")
                .Matches(@"^\d{10}$").WithMessage("Số điện thoại phải đúng 10 kí tự.");

            // Validate Address: must not be empty if provided
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Bạn chưa nhập địa chỉ.");


            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive is required and must be selected if provided.");

            // Validate RoleId: must not be empty if provided
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required and must be selected if provided.");
        }
    }
}
