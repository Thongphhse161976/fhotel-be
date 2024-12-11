using FHotel.Services.DTOs.Hotels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelValidator
{
    public class HotelRequestValidate : AbstractValidator<HotelRequest>
    {
        public HotelRequestValidate()
        {
            RuleFor(x => x.HotelName)
            .NotEmpty().WithMessage("Vui lòng nhập tên khách sạn.")
            .MaximumLength(50).WithMessage("Tên khách sạn không được quá 50 kí tự.");

            // Validate OwnerName
            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Vui lòng nhập chủ khách sạn.");
            // Validate Address
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Vui lòng nhập địa chỉ khách sạn.")
                .MaximumLength(100).WithMessage("Đại chỉ không vượt quá 100 kí tự");

            // Validate Phone
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Vui lòng nhập sdt khách sạn.")
                .Matches(@"^\d{10}$").WithMessage("Sdt phải đúng 10 kí tự.");

            // Validate Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Vui lòng nhập địa chỉ email khách sạn.")
                .EmailAddress().WithMessage("Sai định dạng email.");

            // Validate Description
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Vui lòng nhập mô tả khách sạn.");


            // Validate DistrictId
            RuleFor(x => x.DistrictId)
                .NotEmpty().WithMessage("Vui lòng nhập quận.");
        }
    }
}
