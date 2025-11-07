using Application.Classification.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classification.Validator
{
    public class UpdateClassificationValidator : AbstractValidator<UpdateClassificationDto>
    {
        public UpdateClassificationValidator()
        {
            RuleFor(r => r.Id).NotEmpty().GreaterThan(0).WithMessage("Id cannot be empty/less/equal to 0");
            RuleFor(t => t.Type).MaximumLength(100).WithMessage("Cannot exceed 100 characters");
            RuleFor(t => t.Facet).MaximumLength(100).WithMessage("Cannot exceed 100 characters");
            RuleFor(t => t.FacetCode).MaximumLength(100).WithMessage("Cannot exceed 100 characters");
            RuleFor(t => t.Code).MaximumLength(100).WithMessage("Cannot exceed 100 characters");
            RuleFor(t => t.Name).MaximumLength(100).WithMessage("Cannot exceed 100 characters");
            RuleFor(t => t.LangualId).MaximumLength(100).WithMessage("Cannot exceed 100 characters");
        }
    }
}
