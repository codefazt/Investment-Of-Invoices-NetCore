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

namespace TuFactoring.Areas.Backoffice.Pages
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
        private string Confirmant { get; set; }
        private string Country { get; set; }
        public string JsonUnpayedInvoices { get; set; }
        private string Owner { get; set; }


        //Variables de la pagina
        public bool? financiedNull { get; set; }
        public List<SelectListItem> Supplier_Options { get; set; }
        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> Currency_options { get; set; }

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
        public ConsultasPagosModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
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

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(Country))
            {
                var data = await _globalService.GetDataCountryInvoices(Int32.Parse(Country), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
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

            List<Invoices> facturaCheck = new List<Invoices>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();


            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            return Page();

        }

        public async Task<JsonResult> OnPostPayedInvoices([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var paymentsQuery = await _paymentService.ReceiptsQueryBackoffice("paid",Country, pag.Filter, pag.Pagination, token);

            return new JsonResult(paymentsQuery);
        }

        public async Task<JsonResult> OnPostUnpayedInvoices([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var unpaymentsQuery = await _paymentService.ReceiptsQueryBackoffice("unpaid",Country, pag.Filter, pag.Pagination, token);

            return new JsonResult(unpaymentsQuery);
        }

    }

}