using FHotel.Service.DTOs.Reservations;
using FHotel.Services.DTOs.ReservationDetails;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.ReservationValidator
{
    public class ReservationDetailRequestValidator: AbstractValidator<ReservationDetailRequest>
    {
        public ReservationDetailRequestValidator()
        {
            // Validate ReservationId: must not be empty
            RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required.");

            // Validate RoomTypeId: must not be empty
            RuleFor(x => x.RoomTypeId)
                .NotEmpty().WithMessage("Room Type ID is required.");

            // Validate NumberOfRooms: must not be empty and should be a positive number
            RuleFor(x => x.NumberOfRooms)
                .NotEmpty().WithMessage("Number of rooms is required.")
                .GreaterThan(0).WithMessage("Number of rooms must be greater than zero.");
        }
    }
}
