using FHotel.Service.DTOs.Facilities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.FacilityValidator
{
    public class FacilityRequestValidator: AbstractValidator<FacilityRequest>
    {
        public FacilityRequestValidator()
        {
            RuleFor(r => r.FacilityName)
                .NotEmpty().WithMessage("Facility name is required.");
            RuleFor(r => r.Price)
                .NotNull().WithMessage("Facility price is required.");
        }
    }
}
