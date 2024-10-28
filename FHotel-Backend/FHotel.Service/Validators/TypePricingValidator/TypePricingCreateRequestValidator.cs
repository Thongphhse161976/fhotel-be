using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.TypePricingValidator
{
    public class TypePricingCreateRequestValidator : AbstractValidator<TypePricingCreateRequest>
    {
        public TypePricingCreateRequestValidator()
        {
            
        }
    }
}
