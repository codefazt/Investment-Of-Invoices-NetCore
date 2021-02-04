using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels.Validation
{
   public class RegistroValidation : AbstractValidator<Persons>
    {
        private string ExpreRegularLegal { get; set; }
        private string ExpreRegularPerson { get; set; }

        public RegistroValidation(string ExpreR)
        {
            
            RuleFor(x => x.Document.Identification).NotEmpty().NotNull();
            RuleFor(x => x.Document.Number).NotEmpty().NotNull().MaximumLength(30);
            RuleFor(x => x.Participant).NotNull().NotEmpty();
            RuleFor(x => x.Category).NotNull().NotEmpty();
            RuleFor(x => x.Address.Label).NotNull().NotEmpty().MaximumLength(15);
            RuleFor(x => x.Address.Line1).NotNull().NotEmpty().MinimumLength(15);
            When(x => x.Address.Line2 != null && x.Address.Line2 != "", () => { RuleFor(x => x.Address.Line2).MinimumLength(15).MaximumLength(200); }); 
            RuleFor(x => x.Address.Region).NotNull().NotEmpty();
            RuleFor(x => x.Address.City).NotNull().NotEmpty();
            RuleFor(x => x.Phone.Label).NotNull().NotEmpty().MaximumLength(15);
            RuleFor(x => x.Phone.Number).NotNull().NotEmpty();

            When(x => x.Contacts != null, () => {

                RuleForEach(x => x.Contacts).ChildRules(contacto => {

                    contacto.RuleFor(x => x.Label).NotNull().NotEmpty();
                    contacto.RuleFor(x => x.DocumentNumber).NotNull().NotEmpty().MaximumLength(255);
                    contacto.RuleFor(x => x.Identification).NotNull().NotEmpty();
                    contacto.RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(2).MaximumLength(255);
                    contacto.RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().MinimumLength(10).MaximumLength(60);
                    contacto.RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();

                });
            });
            When(x => x.Accounts != null, () => {

                RuleForEach(x => x.Accounts).ChildRules(cuenta => {

                    cuenta.When(x => x.AccountType != "REQUEST", () => {

                        cuenta.RuleFor(x => x.Entity).NotNull().NotEmpty().MaximumLength(255);
                        cuenta.RuleFor(x => x.AccountType).NotNull().NotEmpty();
                        cuenta.RuleFor(x => x.Currency).NotEmpty().NotNull();
                        cuenta.RuleFor(x => x.AccountNumber).NotEmpty().NotNull().MaximumLength(255);

                    }).Otherwise(() => {

                        cuenta.RuleFor(x => x.Entity).NotNull().NotEmpty().MaximumLength(255);
                        cuenta.RuleFor(x => x.AccountType).NotNull().NotEmpty();
                        cuenta.RuleFor(x => x.AccountNumber).Null();
                        cuenta.RuleFor(x => x.Currency).Null();
                    });

                });

            });
            When(x => x.Customers != null, () =>
            {
                RuleForEach(x => x.Customers).ChildRules(asociado => {

                    asociado.RuleFor(x => x.Identification).NotEmpty().NotNull();
                    asociado.RuleFor(x => x.Number).NotEmpty().NotNull();
                    asociado.RuleFor(x => x.Company).NotEmpty().NotNull().Matches("[A-Za-zÁ-ý]{1,}").MinimumLength(4).MaximumLength(255);
                    asociado.RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(4).MaximumLength(255);
                    asociado.RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().MinimumLength(10).MaximumLength(60);
                    asociado.RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();

                });
            });
            When(x => x.Suppliers != null, () =>
            {
                RuleForEach(x => x.Suppliers).ChildRules(asociado => {

                    asociado.RuleFor(x => x.Identification).NotEmpty().NotNull();
                    asociado.RuleFor(x => x.Number).NotEmpty().NotNull();
                    asociado.RuleFor(x => x.Company).NotEmpty().NotNull().Matches("[A-Za-zÁ-ý]{1,}").MinimumLength(4).MaximumLength(255);
                    asociado.RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(4).MaximumLength(255);
                    asociado.RuleFor(x => x.Email).NotEmpty().NotNull().EmailAddress().MinimumLength(10).MaximumLength(60);
                    asociado.RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();

                });
            });
            When(x => x.Discriminator == "LEGAL",() => {

                RuleFor(x => x.Company).NotEmpty().NotNull().MinimumLength(4).MaximumLength(255);
                RuleFor(x => x.FirstName).Null().Empty();
                RuleFor(x => x.LastName).Null().Empty();
                RuleFor(x => x.Email).Null().Empty();
                
            });
            When(x => x.Discriminator == "PERSON", () => {

                RuleFor(x => x.FirstName).NotEmpty().NotNull().MinimumLength(4).MaximumLength(255);
                RuleFor(x => x.Email.Label).NotNull().NotEmpty().MaximumLength(15);
                RuleFor(x => x.Email.Address).NotEmpty().NotNull().EmailAddress().MinimumLength(10).MaximumLength(60);
                RuleFor(x => x.LastName).NotNull().MaximumLength(1);
                RuleFor(x => x.Company).Null().Empty();
            });

        }

        public void SetExpreRegularLegal(string ExpreR)
        {
            this.ExpreRegularLegal = ExpreR;
        }
        public void SetExpreRegularPerson(string ExpreR)
        {
            this.ExpreRegularPerson = ExpreR;
        }

    }
}
