using FHotel.Services.DTOs.Payments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.PaymentValidator
{
    public class PaymentRequestValidator: AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            // Validate ReservationId: must not be null
            RuleFor(x => x.ReservationId)
                .NotNull().WithMessage("Reservation ID is required.");

            // Validate AmountPaid: must be greater than or equal to zero
            RuleFor(x => x.AmountPaid)
                .NotNull().WithMessage("Amount paid is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Amount paid must be greater than or equal to zero.");

            // Validate PaymentMethodId: must not be null
            RuleFor(x => x.PaymentMethodId)
                .NotNull().WithMessage("Payment method ID is required.");
        }
    }
}
