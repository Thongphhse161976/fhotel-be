using FluentValidation;
using FHotel.Service.DTOs.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelValidator
{
    public class HotelCreateRequestValidator : AbstractValidator<HotelCreateRequest>
    {
        public HotelCreateRequestValidator() {
            RuleFor(x => x.HotelName)
            .NotEmpty().WithMessage("HotelName is required.")
            .MaximumLength(50).WithMessage("HotelName cannot exceed 50 characters.");

            // Validate Address
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(100).WithMessage("Address cannot exceed 100 characters.");

            // Validate Phone
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");

            // Validate Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // Validate Description
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            // Validate Image
            RuleFor(x => x.Image)
                 .NotEmpty().WithMessage("Image is required.");


            // Validate CityId
            RuleFor(x => x.CityId)
                .NotNull().WithMessage("CityId is required.");

            // Validate OwnerId
            RuleFor(x => x.OwnerId)
                .NotNull().WithMessage("OwnerId is required.");


        }
    }
}
