using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Validator
{
    public class CreateProductValidator : AbstractValidator<Dto.Request.CreateProductDto>
    {
        public CreateProductValidator()
        {
                 RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");
            RuleFor(x => x.Id)
                .NotEmpty().GreaterThan(0).WithMessage("Product number must be greater than zero.");
            RuleFor(x => x.FoodTypeId)
                .GreaterThan(0).WithMessage("Food type ID must be greater than zero.");
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value.");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock must be a non-negative value.");
        }
    }
}
