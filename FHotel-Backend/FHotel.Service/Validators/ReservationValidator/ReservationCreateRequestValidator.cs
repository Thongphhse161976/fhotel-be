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
                .NotNull().WithMessage("Customer ID is required.");

            // Validate CheckInDate: it should not be null, should be today or later
            RuleFor(x => x.CheckInDate)
                .NotNull().WithMessage("Check-in date is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date must be today or in the future.");

            // Validate CheckOutDate: it should not be null, and should be after CheckInDate
            RuleFor(x => x.CheckOutDate)
                .NotNull().WithMessage("Check-out date is required.")
                .GreaterThan(x => x.CheckInDate).WithMessage("Check-out date must be after check-in date.");

            // Validate TotalAmount: it should not be null or negative
            RuleFor(x => x.TotalAmount)
                .NotNull().WithMessage("Total amount is required.")
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.");
        }
    }
}
