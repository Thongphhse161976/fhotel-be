using FHotel.Service.DTOs.HotelRegistations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelResgistrationValidator
{
    public class HotelRegistrationUpdateRequestValidator : AbstractValidator<HotelRegistrationUpdateRequest>
    {
        public HotelRegistrationUpdateRequestValidator()
        {
            // HotelRegistrationId must be present for update
            RuleFor(x => x.HotelRegistrationId)
                .NotEmpty().WithMessage("HotelRegistrationId is required.");

            // OwnerId is required
            RuleFor(x => x.OwnerId)
                .NotNull().WithMessage("OwnerId is required.");

            // NumberOfHotels must be greater than or equal to 1
            RuleFor(x => x.NumberOfHotels)
                .GreaterThan(0).WithMessage("NumberOfHotels must be at least 1.");

            // Description should not exceed 500 characters
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

           
        }
    }
}
