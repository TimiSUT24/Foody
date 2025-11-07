using Application.NutritionValue.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Validator
{
    public class UpdateNutritionValueValidator : AbstractValidator<UpdateNutritionValueDto>
    {
        public UpdateNutritionValueValidator()
        {
            RuleFor(n => n.Name).NotEmpty().WithMessage("name cannot be empty")
             .MaximumLength(200).WithMessage("cannot exceed 200 length");
            RuleFor(i => i.Id).NotEmpty().GreaterThan(0).WithMessage("FoodId cannot be empty");
        }
    }
}
