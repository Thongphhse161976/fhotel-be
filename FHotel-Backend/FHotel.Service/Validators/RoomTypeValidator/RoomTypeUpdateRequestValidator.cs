using FHotel.Service.DTOs.RoomTypes;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.RoomTypeValidator
{
    public class RoomTypeUpdateRequestValidator : AbstractValidator<RoomTypeUpdateRequest>
    {
        public RoomTypeUpdateRequestValidator()
        {

            RuleFor(r => r.TypeId)
                .NotEmpty().WithMessage("Type Id is required.");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(r => r.RoomSize)
                .NotNull().WithMessage("Room size is required.")
                .GreaterThan(0).WithMessage("Room size must be greater than 0.");


            RuleFor(r => r.TotalRooms)
                .NotNull().WithMessage("Total rooms are required.")
                .GreaterThan(0).WithMessage("Total rooms must be greater than 0.");

            RuleFor(r => r.AvailableRooms)
                .NotNull().WithMessage("Available rooms are required.")
                .GreaterThanOrEqualTo(0).WithMessage("Available rooms must be at least 0.")
                .LessThanOrEqualTo(r => r.TotalRooms).WithMessage("Available rooms cannot exceed total rooms.");

            
        }
    }

}
