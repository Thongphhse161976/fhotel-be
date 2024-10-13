using FHotel.Service.DTOs.HotelRegistations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Validators.HotelResgistrationValidator
{
    public class HotelRegistrationUpdateStatusRequestValidator: AbstractValidator<HotelRegistrationUpdateRequest>
    {
        public HotelRegistrationUpdateStatusRequestValidator()
        {

        }
    }
}
