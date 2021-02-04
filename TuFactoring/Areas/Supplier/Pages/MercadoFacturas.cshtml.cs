using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoring.Services;
using System.Globalization;

namespace TuFactoring.Areas.Supplier.Pages
{
    public class MercadoFacturasModel : PageModel
    {
        [BindProperty]
        public string dataJsonAuction { get; set; }
        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        public List<SelectListItem> Confirmant_options { get; set; }
        public List<SelectListItem> Debtor_options { get; set; }

        private readonly IAuctionService _aS;
        private readonly IGlobalService _gS;
        private readonly IAuthService _authS;
        private readonly SignInManager<User> _signInManager;
        private readonly string __notAuthorized = "You are not authorised to perform this action";

        public MercadoFacturasModel(IAuctionService aS, SignInManager<User> signInManager, IGlobalService gS, IAuthService authS)
        {
            this._aS = aS;
            this._gS = gS;
            this._authS = authS;
            this._signInManager = signInManager;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            filter = new filterInvoice();

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault()); 
            

            Currency_options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != o)
            {
                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(await this._gS.GetDataCountryCurrency(o,token)));
            }


            var dataAuction = await this._aS.GetAuction(o,token);
            if (dataAuction.Count > 0 && dataAuction[0].Error == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }
            
            dataJsonAuction = (string)JsonConvert.SerializeObject(dataAuction);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            dataFilter = JsonConvert.SerializeObject(filter);


            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
        ;

            Currency_options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != o)
            {
                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(await this._gS.GetDataCountryCurrency(o,token)));
            }
            
            var dataAuction = await this._aS.GetAuction(o,token);

            if (dataAuction.Count > 0 && dataAuction[0].Error == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }
           
            dataJsonAuction = (string)JsonConvert.SerializeObject(dataAuction);

            return Page();
        }

        public async Task<JsonResult> OnPostInvoices([FromBody]RequestPagination pagination)
        {
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var data = await this._aS.GetPublications(o, "SUPPLIER", c, pagination.Filter, token, pagination.Pagination);

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


        public async Task<JsonResult> OnPostAuction()
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var dataAuction = await this._aS.GetAuction(o, token);

            if (dataAuction.Count == 0)
            {
                var auctionNotAvailable = "Auction not found";
                return new JsonResult(auctionNotAvailable);
            }

            return new JsonResult(dataAuction[0]);
        }

    }
}