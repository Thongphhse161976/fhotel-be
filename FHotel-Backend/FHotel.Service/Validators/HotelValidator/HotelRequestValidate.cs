using FHotel.Services.DTOs.Hotels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelValidator
{
    public class HotelRequestValidate : AbstractValidator<HotelRequest>
    {
        public HotelRequestValidate()
        {
            RuleFor(x => x.HotelName)
            .NotEmpty().WithMessage("HotelName is required.")
            .MaximumLength(50).WithMessage("HotelName cannot exceed 50 characters.");

            // Validate OwnerName
            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Owner id is required.");
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
                .NotEmpty().WithMessage("Description is required.");


            // Validate DistrictId
            RuleFor(x => x.DistrictId)
                .NotEmpty().WithMessage("District is required.");
        }
    }
}
