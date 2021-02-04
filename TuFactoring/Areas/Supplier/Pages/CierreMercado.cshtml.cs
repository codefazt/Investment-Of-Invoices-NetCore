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

namespace TuFactoring.Areas.Supplier.Pages
{
    public class CierreMercadoModel : PageModel
    {
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }
        [BindProperty]
        public List<bool?> financiedNull { get; set; }
        public string dataJsonAuction { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        public List<SelectListItem> Confirmant_options { get; set; }
        public List<SelectListItem> Debtor_options { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        private readonly IAuctionService _aS;
        private readonly IAuthService _authS;
        private readonly IGlobalService _gS;
        private readonly SignInManager<User> _signInManager;

        public CierreMercadoModel(IAuctionService aS, SignInManager<User> signInManager, IGlobalService gS, IAuthService authS)
        {
            this._aS = aS;
            this._gS = gS;
            this._signInManager = signInManager;
            this._authS = authS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());            

            Currency_options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != o)
            {
                var data = await this._gS.GetDataCountryCurrency(o, token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
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


            var dataAuction = await this._aS.GetAuction(o,token);

            //if (dataAuction.Count > 0) { if (dataAuction[0].Error != null) dataAuction[0].State = "closed"; };

            dataJsonAuction = (string)JsonConvert.SerializeObject(dataAuction);

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
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            Currency_options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != o)
            {
                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(await this._gS.GetDataCountryCurrency(o,token)));
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

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies);


            var dataAuction = await this._aS.GetAuction(o,token);

            //if (dataAuction.Count > 0) { if (dataAuction[0].Error != null) dataAuction[0].State = "closed"; };

            dataJsonAuction = (string)JsonConvert.SerializeObject(dataAuction);

            return Page();
        }

        public async Task<IActionResult> OnPostDeals([FromBody]RequestPagination pag)
        {

            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var data = await this._aS.GetWinner(o, c, pag.Filter, pag.Pagination, token);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostAceptar([FromBody]UpdateInvoice invoice)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            invoice.country = o;
            
            var data = await this._aS.AceptOffert(invoice,token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostRechazar([FromBody]UpdateInvoice invoice)
        {

            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            invoice.country = o;

            var data = await this._aS.RejectOfferMarket(invoice,token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostAceptarAll([FromBody]List<UpdateInvoice> invoices)
        {

            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            for(var i = 0; i < invoices.LongCount(); i++)
            {

                invoices[i].country = o;
            }

            var data = await this._aS.AceptOffertAll(invoices,token);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }
    }
}