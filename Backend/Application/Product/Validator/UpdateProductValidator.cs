using Application.Product.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Validator
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Product name is required.")
               .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");
            RuleFor(x => x.Id)
                .NotEmpty().GreaterThan(0).WithMessage("Product number must be greater than zero.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value.");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be a non-negative value.");
        }
    }
}
