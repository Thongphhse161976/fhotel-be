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

            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Owner name is required.")
                .MaximumLength(50).WithMessage("Owner name cannot exceed 50 characters.");

            RuleFor(x => x.OwnerEmail)
                .NotEmpty().WithMessage("Owner email is required.")
                .EmailAddress().WithMessage("Invalid email format for owner.");

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
                .NotEmpty().WithMessage("Description is required.");

           

            RuleFor(h => h.Star)
                .InclusiveBetween(1, 5).WithMessage("Star rating must be between 1 and 5.");

            RuleFor(h => h.DistrictId)
                .NotNull().WithMessage("District is required.");


            RuleFor(x => x.BusinessLicenseNumber)
                .NotEmpty().WithMessage("Business license number is required.")
                .MaximumLength(20).WithMessage("Business license number cannot exceed 20 characters.");

            RuleFor(x => x.TaxIdentificationNumber)
                .NotEmpty().WithMessage("Tax identification number is required.")
                .MaximumLength(15).WithMessage("Tax identification number cannot exceed 15 characters.");
        }
    }
}
