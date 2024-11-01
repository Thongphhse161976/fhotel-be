using FluentValidation;
using FHotel.Service.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelValidator
{
    public class HotelCreateRequestValidator : AbstractValidator<HotelCreateRequest>
    {
        public HotelCreateRequestValidator() {
            RuleFor(x => x.HotelName)
            .NotEmpty().WithMessage("Vui lòng nhập tên khách sạn.")
            .MaximumLength(50).WithMessage("HotelName cannot exceed 50 characters.");

            // Validate OwnerName
            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Vui lòng nhập tên chủ khách sạn.")
                .MaximumLength(50).WithMessage("Owner name cannot exceed 50 characters.");

            // Validate OwnerEmail
            RuleFor(x => x.OwnerEmail)
                .NotEmpty().WithMessage("Vui lòng nhập Email chủ khách sạn.")
                .EmailAddress().WithMessage("Invalid email format for owner.");

            RuleFor(x => x.OwnerPhoneNumber)
               .NotEmpty().WithMessage("Vui lòng nhập số điện thoại chủ khách sạn.")
                 .Matches(@"^\d{10}$").WithMessage("Số điện thoại phải đúng 10 kí tự.");

            RuleFor(x => x.OwnerIdentificationNumber)
            .NotEmpty().WithMessage("Vui lòng nhập số căn cước chủ khách sạn.")
            .Matches(@"^\d{12}$").WithMessage("Số căn cước phải gồm 12 chữ số.");


            // Validate Address
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Vui lòng nhập địa chỉ khách sạn.")
                .MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");

            // Validate Phone
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Vui lòng nhập số điện thoại khách sạn.")
                .Matches(@"^\d{10}$").WithMessage("Số điện thoại phải đúng 10 kí tự.");

            // Validate Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Vui lòng nhập Email khách sạn.")
                .EmailAddress().WithMessage("Invalid email format.");

            // Validate Description
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Vui lòng nhập mô tả khách sạn.");


            // Validate DistrictId
            RuleFor(x => x.DistrictId)
                .NotEmpty().WithMessage("Vui lòng nhập quận.");

           
        }
    }
}
