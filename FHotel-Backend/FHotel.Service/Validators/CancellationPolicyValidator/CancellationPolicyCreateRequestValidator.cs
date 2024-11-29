using FHotel.Service.DTOs.CancellationPolicies;
using FHotel.Service.DTOs.TypePricings;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.CancellationPolicyValidator
{
    public class CancellationPolicyCreateRequestValidator : AbstractValidator<CancellationPolicyRequest>
    {
        public CancellationPolicyCreateRequestValidator()
        {
            
        }
    }
}
