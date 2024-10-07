using FHotel.Service.DTOs.HotelRegistations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelResgistrationValidator
{
    public class HotelRegistrationCreateRequestValidator : AbstractValidator<HotelRegistrationCreateRequest>
    {
        public HotelRegistrationCreateRequestValidator()
        {
            // NumberOfHotels must be greater than or equal to 1
            RuleFor(x => x.NumberOfHotels)
                .GreaterThan(0).WithMessage("NumberOfHotels must be at least 1.");

            // Description should not exceed 500 characters
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            // RegistrationStatus must be present
            RuleFor(x => x.RegistrationStatus)
                .NotEmpty().WithMessage("RegistrationStatus is required.")
                .MaximumLength(50).WithMessage("RegistrationStatus cannot exceed 50 characters.");
        }
    }
}
