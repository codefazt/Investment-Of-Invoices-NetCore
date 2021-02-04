using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Confirmant.Pages
{
    public class ConsultaFinanciamientoModel : PageModel
    {

        //Variables de  los Claims
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IInvoiceService _invoiceService;
        private readonly IAuthService _authService;
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }
        private string Owner { get; set; }

        //Variables de la pagina
        ListCountry Identifications;
        public string TipoParticipante { get; set; }
        public List<bool?> financiedNull { get; set; } = new List<bool?>();

        public List<SelectListItem> Debtor_Options { get; set; }
        public List<SelectListItem> Email_options { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        public List<SelectListItem> Status_Options { get; set; }

        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }
        [BindProperty]
        public List<string> dataFilter { get; set; }
        [BindProperty]
        public string IdenJson { get; set; }
        [BindProperty]
        public string listadosInicialesJson { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }

        public ConsultaFinanciamientoModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IInvoiceService invoiceService, IAuthService authService)
        {
            _invoiceService = invoiceService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout");
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Debtor_Options = new List<SelectListItem>();
            Email_options = new List<SelectListItem>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();
            ListStatus();
            //----------- Llenado de Country
            if (HttpContext.Session.GetString("CountryIdentifications") == null ||
                HttpContext.Session.GetString("CountryIdentifications") == "" ||
                HttpContext.Session.GetString("CountryIdentifications") == "null")
            {
                Identifications = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Country) });
                HttpContext.Session.SetString("CountryIdentifications", JsonConvert.SerializeObject(Identifications));
            }
            else Identifications = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryIdentifications"));
            //----------- Llenado de Country

            filter = new List<filterInvoice>();
            financiedNull = new List<bool?>();
            dataFilter = new List<string>();

            for (var i = 0; i < Identifications.Currencies.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataFilter.Add(JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = JsonConvert.SerializeObject(Identifications.Currencies);
            listadosInicialesJson = JsonConvert.SerializeObject(Identifications);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = Participant;

            //var listaClientes = await _peopleService.GetLsitEmailClientConfirmant(Confirmant);
            //----------- Llenado de Country
            if (HttpContext.Session.GetString("CountryIdentifications") == null ||
                HttpContext.Session.GetString("CountryIdentifications") == "" ||
                HttpContext.Session.GetString("CountryIdentifications") == "null")
            {
                Identifications = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                HttpContext.Session.SetString("CountryIdentifications", JsonConvert.SerializeObject(Identifications));
            }
            else Identifications = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryIdentifications"));
            //----------- Llenado de Country
            Debtor_Options = new List<SelectListItem>();
            Email_options = new List<SelectListItem>();
            Currency_options = new List<SelectListItem>();
            dataJsonCurrencies = JsonConvert.SerializeObject(Identifications.Currencies);
            listadosInicialesJson = JsonConvert.SerializeObject(Identifications);

            financiedNull = new List<bool?>();
            for (var i = 0; i < filter.Count; i++)
            {
                dataFilter.Add(JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);

            }
            return Page();
        }

        public async Task<IActionResult> OnPostLlenarConsultaAsync([FromBody] RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var Estados = await _globalService.ConsultaIdentificationsAndCitiesTF(new ParamCountry { Id = Int32.Parse(Country) });
            filterInvoice currencyAccountInvoiceFilter = new filterInvoice();
            if (pag.Filter != null) currencyAccountInvoiceFilter = pag.Filter;
            Prospectos ListaClientesBanco = new Prospectos();
            List<ListAccountantsInvoices> listaFinanciedInvoices = new List<ListAccountantsInvoices>();

            if (Estados != null)
            {
                if (Estados.Currencies != null)
                {
                    if (pag.Filter.AmountRiskFrom == null) pag.Filter.AmountRiskFrom = 0;
                    if (pag.Filter.Abbreviation == null) pag.Filter.Abbreviation = "CREDIT";
                    ListaClientesBanco = await _peopleService.GetConsultaFinaciamientoConfirmant(new ParamClienteOFConfirmant { Bank_id = Confirmant, Filter = pag.Filter, Pagination = pag.Pagination }, token);
                    var ListaAccountantsInvoices = await _invoiceService.GetFinanciablesBankInvoices(new ParamAccountantsInvoices { Country = Int32.Parse(Country), Confirmant = Confirmant }, currencyAccountInvoiceFilter, token);
                    ListaAccountantsInvoices.Currency = pag.Filter.Currency_id;
                    listaFinanciedInvoices.Add(ListaAccountantsInvoices);
                }
            }

            return new JsonResult(new { lista = ListaClientesBanco, estado = Estados, listaFinanciedInvoices = listaFinanciedInvoices });
        }
        public async Task<JsonResult> OnPostDetalleAsync([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            //pag.Filter.Debtor_id = null;
            var ConsultaGeneral = await _invoiceService.GetConsultaInvoices(Confirmant, "CONFIRMANT", pag.Filter, pag.Pagination, token);

            return new JsonResult(ConsultaGeneral);

        }

        private void ListStatus()
        {
            Status_Options.Add(new SelectListItem() { Value = "draft", Text = "CARGADA" });
            Status_Options.Add(new SelectListItem() { Value = "posted", Text = "POSTULADA" });
            Status_Options.Add(new SelectListItem() { Value = "confirmed", Text = "CONFIRMADA" });
            Status_Options.Add(new SelectListItem() { Value = "finalize", Text = "COMPLETADA" });
            Status_Options.Add(new SelectListItem() { Value = "overdue", Text = "VENCIDA" });
        }
    }
}