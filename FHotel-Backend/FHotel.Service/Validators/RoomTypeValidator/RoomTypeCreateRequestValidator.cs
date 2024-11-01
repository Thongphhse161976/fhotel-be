using FHotel.Service.DTOs.RoomTypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.RoomTypeValidator
{
    public class RoomTypeCreateRequestValidator : AbstractValidator<RoomTypeCreateRequest>
    {
        public RoomTypeCreateRequestValidator()
        {
            RuleFor(r => r.TypeId)
                .NotEmpty().WithMessage("Vui lòng nhập loại.");

            RuleFor(r => r.Description)
            .NotEmpty().WithMessage("Vui lòng nhập mô tả.");
            RuleFor(r => r.HotelId)
            .NotEmpty().WithMessage("Vui lòng nhập Id khách sạn.");

            RuleFor(r => r.RoomSize)
                .NotNull().WithMessage("Vui lòng nhập diện tích phòngd.")
                .GreaterThan(0).WithMessage("Diện tích phòng phải lớn hơn 0.");

            

            RuleFor(r => r.TotalRooms)
                .NotNull().WithMessage("Vui lòng nhập tổng số phòng.")
                .GreaterThan(0).WithMessage("Tổng số phòng phải lớn hơn 0.");

            

        }
    }

}
