using FHotel.Service.DTOs.HotelAmenities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelAmenityValidator
{
    public class HotelAmenityUpdateRequestValidator : AbstractValidator<HotelAmenityUpdateRequest>
    {
        public HotelAmenityUpdateRequestValidator()
        {
            RuleFor(x => x.Image)
                 .NotEmpty().WithMessage("Image is required.");
        }
    }
}
