using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;
using System.Globalization;

namespace TuFactoring.Areas.Supplier.Pages
{
    public class FacturasVencimientoModel : PageModel
    {
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }

        public List<bool?> financiedNull { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        public List<SelectListItem> Currency_options { get; set; }

        public List<SelectListItem> Bank_options { get; set; }

        private readonly IInvoiceService _iS;

        private readonly IAuctionService _auS;

        private readonly IGlobalService _gS;

        private readonly IPeopleService _pS;

        private readonly IAuthService _aS;

        public FacturasVencimientoModel(IInvoiceService iS, IPeopleService pS, IGlobalService gS, IAuthService aS, IAuctionService auS)
        {
            this._iS = iS;
            this._auS = auS;
            this._gS = gS;
            this._pS = pS;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != Int32.Parse(o))
            {
                var data = await this._gS.GetDataCountryInvoices(Int32.Parse(o), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
            }

            Currency_options = new List<SelectListItem>();
            
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            
            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != Int32.Parse(o))
            {
                var data = await this._gS.GetDataCountryInvoices(Int32.Parse(o), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
            }
            
            Currency_options = new List<SelectListItem>();
            
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

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies);

            return Page();
        }

        public async Task<JsonResult> OnPostPostponed([FromBody]RequestPagination pagination)
        {
            List<Invoices> dataResponse = new List<Invoices>();
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var data = await this._iS.GetPostponed(c,pagination.Filter,pagination.Pagination,token);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
           }

            for (int x = 0; x < data.Count; x++)
            {
                if (data[x].Publications[0].State != "paid")
                {
                    dataResponse.Add(data[x]);
                }
            }

            return new JsonResult(dataResponse);
        }

        public async Task<JsonResult> OnPostPublicar([FromBody] UpdateInvoice factura)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            factura.country = o;

            var data = await this._auS.PublicationsInvoice(factura, token);

            if (data.Errors == null || data.Error == null)
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
    }
}