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
        }
    }
}
