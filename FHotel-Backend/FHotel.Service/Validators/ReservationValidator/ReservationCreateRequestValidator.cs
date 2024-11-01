using FHotel.Service.DTOs.Reservations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.ReservationValidator
{
    public class ReservationCreateRequestValidator: AbstractValidator<ReservationCreateRequest>
    {
        public ReservationCreateRequestValidator()
        {
            // Validate CustomerId: it should not be null
            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Vui lòng nhập Id khách hàng.");

            // Validate CheckInDate: it should not be null, should be today or later
            RuleFor(x => x.CheckInDate)
                .NotNull().WithMessage("Vui lòng nhập ngày nhận phòng.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Ngày nhận phòng phải là hôm nay hoặc tương lai.");

            // Validate CheckOutDate: it should not be null, and should be after CheckInDate
            RuleFor(x => x.CheckOutDate)
                .NotNull().WithMessage("Vui lòng nhập ngày trả phòng.")
                .GreaterThan(x => x.CheckInDate).WithMessage("Ngày trả phòng phải sau ngày nhận phòng.");

            // Validate TotalAmount: it should not be null or negative
            RuleFor(x => x.TotalAmount)
                .NotNull().WithMessage("Vui lòng nhập tổng số tiền.")
                .GreaterThan(0).WithMessage("Tổng số tiền phải lớn hơn 0.");
        }
    }
}
