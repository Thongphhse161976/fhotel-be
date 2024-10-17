using FHotel.Services.DTOs.RoomFacilities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.RoomFacilityValidator
{
    public class RoomFacilityRequestValidator: AbstractValidator<RoomFacilityRequest>
    {
        public RoomFacilityRequestValidator()
        {
            RuleFor(x => x.RoomTypeId)
                 .NotEmpty().WithMessage("Room type id is required.");
            RuleFor(x => x.FacilityId)
                 .NotEmpty().WithMessage("Facility id is required.");
        }
    }
}
