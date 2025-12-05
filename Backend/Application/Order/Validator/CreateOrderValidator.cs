using Application.Order.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Validator
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(r => r.Items).NotEmpty().WithMessage("FoodID and Quantity cannot be empty");
            RuleFor(r => r.ShippingInformation.Adress).NotEmpty().WithMessage("Adress is required");
            RuleFor(r => r.ShippingInformation.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(r => r.ShippingInformation.State).NotEmpty().WithMessage("State is required");
            RuleFor(r => r.ShippingInformation.City).NotEmpty().WithMessage("City is required");
            RuleFor(r => r.ShippingInformation.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
            RuleFor(r => r.ShippingInformation.FirstName).NotEmpty().WithMessage("Firstname is required");
            RuleFor(r => r.ShippingInformation.LastName).NotEmpty().WithMessage("Lastname is required");
            RuleFor(r => r.ShippingInformation.PostalCode).NotEmpty().WithMessage("PostalCode is required");
        }
    }
}
