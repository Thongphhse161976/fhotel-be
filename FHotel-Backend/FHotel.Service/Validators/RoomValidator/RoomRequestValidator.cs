using FHotel.Service.DTOs.Reservations;
using FHotel.Services.DTOs.Rooms;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.RoomValidator
{
    public class RoomRequestValidator : AbstractValidator<RoomRequest>
    {
        public RoomRequestValidator()
        {
            // RoomNumber is required and must be greater than zero
            RuleFor(x => x.RoomNumber)
                .NotNull().WithMessage("Room number is required.")
                .GreaterThan(0).WithMessage("Room number must be greater than zero.");

            // RoomTypeId is required
            RuleFor(x => x.RoomTypeId)
                .NotNull().WithMessage("Room type ID is required.");

            // Status must be one of the allowed values
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Room status is required.")
                .Must(status => status == "Available" || status == "Occupied" || status == "Maintenance")
                .WithMessage("Room status must be either 'Available', 'Occupied', or 'Maintenance'.");

        }
    }
}
