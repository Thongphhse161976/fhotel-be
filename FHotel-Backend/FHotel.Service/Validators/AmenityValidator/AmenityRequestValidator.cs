using FHotel.Service.DTOs.Amenities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.AmenityValidator
{
    public class AmenityRequestValidator: AbstractValidator<AmenityRequest>
    {
        public AmenityRequestValidator()
        {
            RuleFor(x=> x.AmenityName)
                .NotEmpty().WithMessage("Vui lòng nhập tên");
            RuleFor(x => x.Image)
                 .NotEmpty().WithMessage("Vui lòng nhập hình ảnh");
        }
    }
}
