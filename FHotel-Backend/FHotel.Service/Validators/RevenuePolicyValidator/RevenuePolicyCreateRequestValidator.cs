using FHotel.Service.DTOs.CancellationPolicies;
using FHotel.Service.DTOs.RevenuePolicies;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.RevenuePolicyValidator
{
    public class RevenuePolicyCreateRequestValidator : AbstractValidator<RevenuePolicyRequest>
    {
        public RevenuePolicyCreateRequestValidator()
        {
            
        }
    }
}
