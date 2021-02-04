using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.Validation
{
    public class OffertValidation : AbstractValidator<Offert>
    {
        public OffertValidation()
        {
            RuleFor(x => x.Publication_id).NotEmpty().Length(1, 255);
            RuleFor(x => x.Factor_id).NotEmpty().Length(1, 255);
            RuleFor(x => x.Bid_amount).NotEmpty().GreaterThanOrEqualTo(0).LessThan(100);
        }
    }
}
