using Application.Auth.Dto.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Validator
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x)
            .Must(x =>
                x.Email != string.Empty ||
                x.FirstName != string.Empty ||
                x.LastName != string.Empty ||
                x.PhoneNumber != string.Empty)
            .WithMessage("At least one field must be provided");
        }
    }
}
