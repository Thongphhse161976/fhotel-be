using FHotel.Service.DTOs.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.ServiceValidator
{
    public class ServiceUpdateRequestValidator : AbstractValidator<ServiceUpdateRequest>
    {
        public ServiceUpdateRequestValidator()
        {
            RuleFor(s => s.ServiceName)
                .NotEmpty().WithMessage("Service name is required.")
                .MaximumLength(100).WithMessage("Service name must not exceed 100 characters.");

            RuleFor(s => s.Price)
                .NotNull().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(s => s.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(s => s.ServiceTypeId)
                .NotNull().WithMessage("Service type is required.");

            RuleFor(s => s.Image)
                .NotEmpty().WithMessage("Image is required.");
        }
    }

}
