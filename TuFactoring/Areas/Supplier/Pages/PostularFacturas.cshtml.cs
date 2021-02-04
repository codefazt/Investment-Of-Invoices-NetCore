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
using TuFactoringModels.nuevaVersion;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TuFactoring.Areas.Supplier.Pages
{
    public class PostularFacturasModel : PageModel
    {
        #region Data
        [BindProperty]
        public List<filterInvoice> filter { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public int? filterClear { get; set; }
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        public List<bool?> financiedNull { get; set; }
        private string IdUser { get; set; }
        private string Confirmant { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        public List<SelectListItem> Debtor_Options { get; set; }

        public List<SelectListItem> Currency_options { get; set; }

        private readonly IInvoiceService _iS;

        private readonly IPeopleService _pS;

        private readonly IGlobalService _gS;

        private readonly IAuthService _aS;

        private readonly SignInManager<User> _signInManager;
        #endregion

        public PostularFacturasModel(IInvoiceService iS, SignInManager<User> signInManager, IGlobalService gS, IPeopleService pS, IAuthService aS)
        {
            this._iS = iS;
            this._pS = pS;
            this._gS = gS;
            this._aS = aS;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            if (HttpContext.Session.GetString("Confirmants") != null)
            {
                HttpContext.Session.Remove("Confirmants");
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();

            #region Principal
            if (HttpContext.Session.GetString("CurrencyCountry") == null ||
                JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id == null ||
                JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != Int32.Parse(o))
            {
                var data2 = await this._gS.GetDataCountryCurrency(Int32.Parse(o), token);

                if (data2.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data2));
            }

            var data = await this._pS.GetDebtors(c, token);

            if (data.Count > 0 && data[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            HttpContext.Session.SetString("Debtors", JsonConvert.SerializeObject(data));

            Debtor_Options = new List<SelectListItem>();

            Currency_options = new List<SelectListItem>();

            foreach (var debtor in JsonConvert.DeserializeObject<List<ResponseProveedores>>(HttpContext.Session.GetString("Debtors")))
            {
                Debtor_Options.Add(new SelectListItem() { Value = debtor.Id, Text = debtor.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            filter = new List<filterInvoice>();
            dataJsonFilter = new List<string>();
            financiedNull = new List<bool?>();

            for (var i = 0; i < Currency_options.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies);

            #endregion

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout");
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            #region principal

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != Int32.Parse(o))
            {
                var data = await this._gS.GetDataCountryCurrency(Int32.Parse(o), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
                dataJsonCurrencies = JsonConvert.SerializeObject(data.Currencies);

            }

            if (HttpContext.Session.GetString("Debtors") == null)
            {
                HttpContext.Session.SetString("Debtors", JsonConvert.SerializeObject(await this._pS.GetDebtors(o, token)));
            }

            if (HttpContext.Session.GetString("Confirmants") == null)
            {
                HttpContext.Session.SetString("Confirmants", JsonConvert.SerializeObject(await this._iS.GetConfirmantsSupplier(c, token)));
            }

            Debtor_Options = new List<SelectListItem>();

            Currency_options = new List<SelectListItem>();

            foreach (var debtor in JsonConvert.DeserializeObject<List<ResponseProveedores>>(HttpContext.Session.GetString("Debtors")))
            {
                Debtor_Options.Add(new SelectListItem() { Value = debtor.Id, Text = debtor.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            financiedNull = new List<bool?>();

            for (var i = 0; i < filter.Count; i++)
            {
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies);

            #endregion

            return Page();
        }

        public async Task<JsonResult> OnPostPostulates([FromBody] RequestPagination pag)
        {
            List<Invoices> postulables = new List<Invoices>();

            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var data = await this._iS.GetPostulatesSupplier(c, pag.Filter, pag.Pagination, token);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            for(int x = 0; x < data.Count; x++)
            {
                if(data[x].Term_days >= 30)
                {
                    postulables.Add(data[x]);
                }
            }

            return new JsonResult(postulables);
        }

        public async Task<JsonResult> OnPostConfirmants()
        {
            var token = HttpContext.Session.GetString("token");

            if (HttpContext.Session.GetString("Confirmants") == null)
            {
                var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

                HttpContext.Session.SetString("Confirmants", JsonConvert.SerializeObject(await this._iS.GetConfirmantsSupplier(c, token)));
            }

            var data = JsonConvert.DeserializeObject<List<Entity>>(HttpContext.Session.GetString("Confirmants"));

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostAccounts()
        {
            var owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");

            var accounts = await this._pS.GetConsultaAccountsPeople(new ParamProspecto()
            {
                Country = Int32.Parse(country),
                Filter = new filterInvoice()
                {
                    Id = owner
                }
            }, token);

            return new JsonResult(accounts);
        }

        public async Task<JsonResult> OnPostPostular([FromBody] List<Invoices> invoice)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            invoice.ForEach(x => x.Country_id = Int32.Parse(o));

            var data = await this._iS.PostulateInvoices(invoice, token);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostCatalogo()
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(o))
            {
                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(await this._gS.GetDataCountryInvoices(Int32.Parse(o), token)));
            }

            return new JsonResult(HttpContext.Session.GetString("CountryInvoices"));
        }



    }

}