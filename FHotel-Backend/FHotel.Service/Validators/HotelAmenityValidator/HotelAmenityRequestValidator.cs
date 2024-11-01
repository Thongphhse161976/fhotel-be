using FHotel.Services.DTOs.HotelAmenities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelAmenityValidator
{
    public class HotelAmenityRequestValidator : AbstractValidator<HotelAmenityRequest>
    { 
        public HotelAmenityRequestValidator()
        {
            RuleFor(x => x.HotelId)
                 .NotEmpty().WithMessage("Vui lòng nhập khách sạn.");
            RuleFor(x => x.AmenityId)
                 .NotEmpty().WithMessage("Vui lòn nhập tiện nghi.");
        }
    }
}
