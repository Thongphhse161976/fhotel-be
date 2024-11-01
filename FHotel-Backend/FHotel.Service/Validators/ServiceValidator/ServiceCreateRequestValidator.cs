using FHotel.Service.DTOs.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.ServiceValidator
{
    public class ServiceCreateRequestValidator : AbstractValidator<ServiceCreateRequest>
    {
        public ServiceCreateRequestValidator()
        {
            RuleFor(s => s.ServiceName)
                .NotEmpty().WithMessage("Service name is required.")
                .MaximumLength(100).WithMessage("Service name must not exceed 100 characters.");

            RuleFor(s => s.Price)
                .NotNull().WithMessage("Price is required.");

            RuleFor(s => s.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(s => s.ServiceTypeId)
                .NotNull().WithMessage("Service type is required.");

            RuleFor(s => s.Image)
                .NotEmpty().WithMessage("Image is required.");
        }
    }

}
