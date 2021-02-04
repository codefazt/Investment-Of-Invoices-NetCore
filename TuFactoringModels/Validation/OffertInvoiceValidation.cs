using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.Validation
{
    public class OffertInvoiceValidation : AbstractValidator<OffertInvoice>
    {
        public OffertInvoiceValidation()
        {
            RuleFor(x => x.Publication_id).NotEmpty().Length(1, 255);
            RuleFor(x => x.country).NotEmpty();
            RuleFor(x => x.Bid_amount).NotNull().GreaterThanOrEqualTo(0).LessThan(100);
        }
    }
}
