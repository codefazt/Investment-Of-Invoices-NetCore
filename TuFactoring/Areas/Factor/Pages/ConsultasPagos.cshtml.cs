using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;
using System.Globalization;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Factor.Pages
{
    public class ConsultasPagosModel : PageModel
    {
        #region Data
        //Variables de  los Claims
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _aS;
        private string Confirmant { get; set; }
        private string Country { get; set; }
        public string JsonUnpayedInvoices { get; set; }
        private string Owner { get; set; }
        private string Participant { get; set; }

        //Variables de la pagina
        public bool? financiedNull { get; set; }
        public List<SelectListItem> Banks_Options { get; set; }
        public List<SelectListItem> Supplier_Options { get; set; }
        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        private ListCountry Countries { get; set; }
        private readonly string __notAuthorized = "You are not authorised to perform this action";

        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public bool get { get; set; }

        #endregion
        public ConsultasPagosModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IPaymentService paymentService, IAuthService aS)
        {
            _paymentService = paymentService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _aS = aS;
        }

        public async Task<IActionResult> OnGetAsync([FromBody]RequestPagination pag)
        {

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);

            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Banks_Options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(Country))
            {
                var data = await _globalService.GetDataCountryInvoices(Int32.Parse(Country), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
            }
            if (HttpContext.Session.GetString("CountryBanks") == null || HttpContext.Session.GetString("CountryBanks") == "" || HttpContext.Session.GetString("CountryBanks") == "null")
            {
                Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = Int32.Parse(Country) });
                HttpContext.Session.SetString("CountryBanks", JsonConvert.SerializeObject(Countries));
            }
            else Countries = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryBanks"));

            if (Countries.Entities != null)
            {
                foreach (var bank in Countries.Entities) Banks_Options.Add(new SelectListItem() { Value = bank.Id, Text = bank.Person.Name });
            }
            List<Invoices> facturaCheck = new List<Invoices>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);

            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Banks_Options = new List<SelectListItem>();
            List<Invoices> facturaCheck = new List<Invoices>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CountryBanks") == null || HttpContext.Session.GetString("CountryBanks") == "" || HttpContext.Session.GetString("CountryBanks") == "null")
            {
                Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = Int32.Parse(Country) });
                HttpContext.Session.SetString("CountryBanks", JsonConvert.SerializeObject(Countries));
            }
            else Countries = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryBanks"));
            if (Countries.Entities != null)
            {
                foreach (var bank in Countries.Entities) Banks_Options.Add(new SelectListItem() { Value = bank.Id, Text = bank.Person.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            return Page();

        }

        public async Task<JsonResult> OnPostPayedInvoices([FromBody]RequestPagination pag)
        {
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            var principalCurrency = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currency;
            string filtroBanco = null;
            //var paymentsQuery = await _paymentService.ReceiptsQueryConfirmant(Owner, "paid", Country, null, null, pag.Filter, pag.Pagination, token);
            
            if(pag.Filter != null)
            {
                if (pag.Filter.Confirmant_id != null && pag.Filter.Confirmant_id != "" && pag.Filter.Confirmant_id != "selecione") filtroBanco = pag.Filter.Confirmant_id;
            }
            var paymentsQuery = await _paymentService.ReceiptsQueryConfirmant(new ParamConsultaPeyments() { Payer= Owner, Entity = filtroBanco, State = "paid,unpaid", Country = Country}, pag.Filter, pag.Pagination, token);
            //var unpaymentsQuery = await _paymentService.ReceiptsQueryConfirmant(new ParamConsultaPeyments() { Payer = Owner, Entity = filtroBanco, State = "unpaid", Country = Country }, pag.Filter, pag.Pagination, token);
            var unpaymentsQuery = new List<Receipts>();
            if (paymentsQuery.Count == 0 || paymentsQuery[0].Errors == null || unpaymentsQuery.Count == 0 || unpaymentsQuery[0].Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (unpaymentsQuery != null && unpaymentsQuery.Count > 0)
            {
                foreach (var receipt in unpaymentsQuery) paymentsQuery.Add(receipt);
            }
            if (principalCurrency != null) paymentsQuery = paymentsQuery.OrderBy(x => x.Currency.Id != principalCurrency.Id).ThenByDescending(y => y.Receipt_date).ThenBy(z => z.Receiver.Name).ToList();
            return new JsonResult(paymentsQuery);
        }

        public JsonResult OnPostUnpayedInvoices([FromBody]RequestPagination pag)
        {
            return new JsonResult(new List<Receipts>());
        }
        /*
         public async Task<JsonResult> OnPostUnpayedInvoices([FromBody]RequestPagination pag)
        {
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");           
            var unpaymentsQuery = await _paymentService.ReceiptsQueryConfirmant(new ParamConsultaPeyments() { Payer = Owner, State = "unpaid", Country = Country }, pag.Filter, pag.Pagination, token);
            if (unpaymentsQuery.Count == 0 || unpaymentsQuery[0].Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            return new JsonResult(unpaymentsQuery);
        }
         */

    }

}
