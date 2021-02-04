using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class RolesValidation : AbstractValidator<Role>
    {
        public RolesValidation()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(255);
            RuleFor(x => x.Participant).NotEmpty().NotNull().MaximumLength(255);
            RuleFor(x => x.Abbreviation).NotEmpty().NotNull().MaximumLength(255);
        }
    }
}
