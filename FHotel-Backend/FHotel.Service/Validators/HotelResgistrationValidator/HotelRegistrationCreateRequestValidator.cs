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

            // Description is required
            RuleFor(x => x.Description)
                 .NotEmpty().WithMessage("Description is required.");

            
        }
    }
}
