using FHotel.Service.DTOs.Hotels;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelValidator
{
     public class HotelUpdateRequestValidator : AbstractValidator<HotelUpdateRequest>
    {
        public HotelUpdateRequestValidator()
        {
            RuleFor(h => h.HotelName)
                .NotEmpty().WithMessage("Hotel name is required.")
                .MaximumLength(100).WithMessage("Hotel name must not exceed 100 characters.");

            RuleFor(h => h.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

            RuleFor(h => h.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number is invalid.");

            RuleFor(h => h.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(h => h.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(h => h.Image)
                .NotEmpty().WithMessage("Image is required.");
                

            RuleFor(h => h.Star)
                .InclusiveBetween(1, 5).WithMessage("Star rating must be between 1 and 5.");

            RuleFor(h => h.CityId)
                .NotNull().WithMessage("City is required.");

            RuleFor(h => h.OwnerId)
                .NotNull().WithMessage("Owner is required.");

            RuleFor(h => h.IsActive)
                .NotNull().WithMessage("IsActive field is required.");
        }
    }
}
