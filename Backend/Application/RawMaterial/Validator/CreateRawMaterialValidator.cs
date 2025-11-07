using Application.RawMaterial.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RawMaterial.Validator
{
    public class CreateRawMaterialValidator : AbstractValidator<CreateRawMaterialDto>
    {
        public CreateRawMaterialValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Raw material name is required.")
                .MaximumLength(200).WithMessage("Raw material name cannot exceed 200 characters.");

            RuleFor(x => x.FoodId)
                .NotEmpty().GreaterThan(0).WithMessage("Raw material number must be greater than zero.");
            RuleFor(x => x.FoodEx2)
                .MaximumLength(100).WithMessage("FoodEx2 must not exceed 100 characters.");
            RuleFor(x => x.Cooking)
                .MaximumLength(100).WithMessage("Cooking must not exceed 100 characters.");
            RuleFor(x => x.Portion)
                .GreaterThanOrEqualTo(0).When(x => x.Portion.HasValue).WithMessage("Portion must be greater than or equal to 0 if provided.");
            RuleFor(x => x.Factor)
                .GreaterThanOrEqualTo(0).When(x => x.Factor.HasValue).WithMessage("Factor must be greater than or equal to 0 if provided.");
            RuleFor(x => x.ConvertedToRow)
                .GreaterThanOrEqualTo(0).When(x => x.ConvertedToRow.HasValue).WithMessage("ConvertedToRaw must be greater than 0 if provided.");
        }
    }
}
