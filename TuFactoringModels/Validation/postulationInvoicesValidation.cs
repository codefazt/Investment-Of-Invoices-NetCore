using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.Validation
{
    public class postulationInvoicesValidation : AbstractValidator<PostulationInvoice>
    {
        public postulationInvoicesValidation()
        {
            RuleFor(x => x.Invoice).NotEmpty().Length(1, 255);
            RuleFor(x => x.Confirmant).NotEmpty().Length(1, 255);
        }
    }
}
