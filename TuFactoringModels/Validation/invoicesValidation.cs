using FluentValidation;

namespace TuFactoringModels.Validation
{
    public class invoicesValidation : AbstractValidator<Invoices>
    {
        public invoicesValidation()
        {
            RuleFor(x => x.Number).NotEmpty().Length(1, 255);
            RuleFor(x => x.Original_amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Issued_date).NotEmpty().Length(1, 255)
                .Must(ValidationResources.validateTimeInferiorString)
                .NotEqual(x => x.Expiration_date);
            RuleFor(x => x.Expiration_date).NotEmpty().Length(1, 255)
                .Must(ValidationResources.validateTimeSuperiorString)
                .NotEqual(x => x.Issued_date);
            RuleFor(x => x.Supplier_id).NotEmpty().Length(1, 255);
            RuleFor(x => x.Debtor_id).NotEmpty().Length(1, 255);
            RuleFor(x => x.Currency_id).NotEmpty();
            RuleFor(x => x).Must(ValidationResources.validateCharges);
        }
        
        
    }
}
