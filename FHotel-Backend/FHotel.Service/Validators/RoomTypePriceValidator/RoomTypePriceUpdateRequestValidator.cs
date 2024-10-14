using FHotel.Service.DTOs.RoomTypePrices;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.RoomTypePriceValidator
{
    public class RoomTypePriceUpdateRequestValidator : AbstractValidator<RoomTypePriceUpdateRequest>
    {
        public RoomTypePriceUpdateRequestValidator()
        {

            RuleFor(r => r.DayOfWeek)
                .NotEmpty().WithMessage("Day of the week is required.")
                .Must(IsValidDayOfWeek).WithMessage("Day of the week must be one of the following: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday.");

            RuleFor(r => r.PercentageIncrease)
                .NotNull().WithMessage("Price is required.")
                .InclusiveBetween(0, 100).WithMessage("Price must be greater than 0.");
        }

        private bool IsValidDayOfWeek(string? dayOfWeek)
        {
            var validDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            return validDays.Contains(dayOfWeek);
        }
    }
}
