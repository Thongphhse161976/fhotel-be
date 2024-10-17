using FHotel.Service.DTOs.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.UserValidator
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            // Validate FirstName: must not be empty
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.");
            // Validate LastName: must not be empty
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.");
            // Validate Email: must not be empty and should be in a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            // Validate Password: must not be empty
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
            // Validate IdentificationNumber: must be 9 or 12 digits
            RuleFor(x => x.IdentificationNumber)
                .NotEmpty().WithMessage("Identification number is required.")
                .Matches(@"^\d{9}(\d{3})?$").WithMessage("Identification number must be either 9 or 12 digits.");
            // Validate PhoneNumber: must be 10 or 11 digits
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{9,10}$").WithMessage("Phone number must be 10 or 11 digits long.");
            // Validate Address: must not be empty
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");
            // Validate Sex: must not be null
            RuleFor(x => x.Sex)
                .NotNull().WithMessage("Sex is required and must be selected.");
            // Validate RoleId: must not be empty
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required and must be selected.");
            
        }
    }
}
