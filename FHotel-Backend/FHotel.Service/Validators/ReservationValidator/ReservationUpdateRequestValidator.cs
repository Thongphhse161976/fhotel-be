﻿using FHotel.Service.DTOs.Reservations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.ReservationValidator
{
    public class ReservationUpdateRequestValidator: AbstractValidator<ReservationUpdateRequest>
    {
        public ReservationUpdateRequestValidator()
        {

            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("Customer ID is required.");

            RuleFor(x => x.CheckInDate)
                .NotNull().WithMessage("Check-in date is required.")
                .GreaterThan(DateTime.Now).WithMessage("Check-in date must be in the future.")
                .When(x => x.CheckInDate.HasValue); // Only validate if CheckInDate is provided

            RuleFor(x => x.CheckOutDate)
                .NotNull().WithMessage("Check-out date is required.")
                .GreaterThan(x => x.CheckInDate).WithMessage("Check-out date must be later than check-in date.")
                .When(x => x.CheckOutDate.HasValue && x.CheckInDate.HasValue); // Validate if both dates are provided

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than zero.")
                .When(x => x.TotalAmount.HasValue); // Validate if TotalAmount is provided

            RuleFor(x => x.ReservationStatus)
                .NotEmpty().WithMessage("Reservation status is required.")
                .Must(status => status == "Pending" || status == "Confirmed" || status == "Cancelled")
                .WithMessage("Reservation status must be either 'Pending', 'Confirmed', or 'Cancelled'.");
            // Allow ActualCheckInTime to be equal to CheckInDate
            //RuleFor(x => x.ActualCheckInTime)
            //    .GreaterThanOrEqualTo(x => x.CheckInDate).When(x => x.ActualCheckInTime.HasValue && x.CheckInDate.HasValue)
            //    .WithMessage("Actual check-in time must be later than the check-in date.");
            // Allow ActualCheckOutDate to be equal to CheckOutDate
            RuleFor(x => x.ActualCheckOutDate)
                .GreaterThanOrEqualTo(x => x.ActualCheckInTime).When(x => x.ActualCheckInTime.HasValue)
                .WithMessage("Actual check-out date must be later than the actual check-in time.");
        }
    }
}
