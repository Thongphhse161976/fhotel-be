using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.UserValidator
{
    public class UserUpdateRequestValidator: AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            // Validate FirstName: must not be empty if provided
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name cannot be empty if provided.");

            // Validate LastName: must not be empty if provided
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name cannot be empty if provided.");

            // Validate Email: must be in a valid format if provided
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty if provided.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            // Validate Password: must not be empty if provided
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty if provided.");

            // Validate IdentificationNumber: must be 9 or 12 digits if provided
            RuleFor(x => x.IdentificationNumber)
                .NotEmpty().WithMessage("Identification number cannot be empty if provided.")
                .Matches(@"^\d{9}(\d{3})?$").WithMessage("Identification number must be either 9 or 12 digits.");

            // Validate PhoneNumber: must be 10 or 11 digits if provided
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number cannot be empty if provided.")
                .Matches(@"^\d{10,11}$").WithMessage("Phone number must be 10 or 11 digits long.");

            // Validate Address: must not be empty if provided
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address cannot be empty if provided.");

            // Validate Sex: must not be null if provided
            RuleFor(x => x.Sex)
                .NotNull().WithMessage("Sex is required and must be selected if provided.");
            // Validate IsActive: must not be null if provided
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive is required and must be selected if provided.");

            // Validate RoleId: must not be empty if provided
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required and must be selected if provided.");
        }
    }
}
