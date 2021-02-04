using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.Validation
{
    public class FilterEmptyValidation : AbstractValidator<filterInvoice>
    {
        public FilterEmptyValidation()
        {
            RuleFor(x => x.Factor_id).Null().Empty();
            RuleFor(x => x.Supplier_id).Null().Empty();
            RuleFor(x => x.Confirmant_id).Null().Empty();
            RuleFor(x => x.Debtor_id).Null().Empty();
            RuleFor(x => x.DebtorId).Null().Empty();
            RuleFor(x => x.Number).Null().Empty();
            RuleFor(x => x.Currency_id).Null().Empty();
            RuleFor(x => x.Financied).Null().Empty();
            RuleFor(x => x.IsOffered).Null().Empty();
            RuleFor(x => x.BidsStatus).Null().Empty();
            RuleFor(x => x.ExpirationFrom).Null().Empty();
            RuleFor(x => x.ExpirationTo).Null().Empty();
            RuleFor(x => x.IssuedFrom).Null().Empty();
            RuleFor(x => x.IssuedTo).Null().Empty();
            RuleFor(x => x.AmountFrom).Null().Empty();
            RuleFor(x => x.AmountTo).Null().Empty();
            RuleFor(x => x.InvoiceStatus).Null().Empty();
            RuleFor(x => x.InvoiceStatusNot).Null().Empty();
            RuleFor(x => x.Abbreviation).Null().Empty();
            RuleFor(x => x.Debtor).Null().Empty();
            RuleFor(x => x.Email).Null().Empty();
            RuleFor(x => x.AmountRiskFrom).Null().Empty();
            RuleFor(x => x.AmountRiskTo).Null().Empty();
            RuleFor(x => x.AmountRiskAvailableFrom).Null().Empty();
            RuleFor(x => x.AmountRiskAvailableTo).Null().Empty();
            RuleFor(x => x.Category).Null().Empty();
            RuleFor(x => x.Name).Null().Empty();
            RuleFor(x => x.Participant).Null().Empty();
            RuleFor(x => x.Discriminator).Null().Empty();
            RuleFor(x => x.City).Null().Empty();
            RuleFor(x => x.Region).Null().Empty();
            RuleFor(x => x.People).Null().Empty();
            RuleFor(x => x.Account).Null().Empty();
            RuleFor(x => x.Event).Null().Empty();
            RuleFor(x => x.Status).Null().Empty();
            RuleFor(x => x.ChangeFrom).Null().Empty();
            RuleFor(x => x.ChangeTo).Null().Empty();
            RuleFor(x => x.ChangeStatus).Null().Empty();
            RuleFor(x => x.Program).Null().Empty();
        }
    }
}
