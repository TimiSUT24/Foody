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

        }
    }
}
