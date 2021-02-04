using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class usersValidation : AbstractValidator<User>
    {
        public usersValidation()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(255);
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(255);
            //RuleFor(x => x.Roles_id).NotEmpty().NotNull();
            RuleFor(x => x.Foto).Must(ValidationResources.validateImg);
        }

    }
}
