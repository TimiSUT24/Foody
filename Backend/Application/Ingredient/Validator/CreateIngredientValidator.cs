using Application.Ingredient.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Validator
{
    public class CreateIngredientValidator : AbstractValidator<CreateIngredientDto>
    {
        public CreateIngredientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ingredient name is required.")
                .MaximumLength(200).WithMessage("Ingredient name cannot exceed 200 characters.");
            RuleFor(x => x.FoodId)
                .NotEmpty().GreaterThan(0).WithMessage("Ingredient number must be greater than zero.");

            RuleFor(x => x.WaterFactor)
                .GreaterThanOrEqualTo(0).When(x => x.WaterFactor.HasValue).WithMessage("Portion must be greater than or equal to 0 if provided.");
            RuleFor(x => x.FatFactor)
                .GreaterThanOrEqualTo(0).When(x => x.FatFactor.HasValue).WithMessage("Factor must be greater than or equal to 0 if provided.");
            RuleFor(x => x.WeightBeforeCooking)
                .GreaterThanOrEqualTo(0).When(x => x.WeightBeforeCooking.HasValue).WithMessage("Portion must be greater than or equal to 0 if provided.");
            RuleFor(x => x.WeightAfterCooking)
                .GreaterThanOrEqualTo(0).When(x => x.WeightAfterCooking.HasValue).WithMessage("Factor must be greater than or equal to 0 if provided.");
            RuleFor(x => x.CookingFactor)
                .GreaterThanOrEqualTo(0).When(x => x.CookingFactor.HasValue).WithMessage("Factor must be greater than or equal to 0 if provided.");
        }
    }
}
