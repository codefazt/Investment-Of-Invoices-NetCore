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

namespace TuFactoring.Areas.Factor.Pages
{
    public class MercadoFacturasModel : PageModel
    {
        [BindProperty]
        public string dataJsonFactor { get; set; }
        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        public List<SelectListItem> Confirmant_options { get; set; }
        public List<SelectListItem> Debtor_options { get; set; }
        public bool? OfferedNull { get; set; }

        public string dataToken { get; set; }

        private readonly IAuctionService _aS;
        private readonly IAuthService _authS;
        private readonly IGlobalService _gS;

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
            filter = new filterInvoice()
            {
                IsOffered = null
            };

            OfferedNull = null;

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataToken = token;
            dataFilter = JsonConvert.SerializeObject(filter);

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            
            
            Currency_options = new List<SelectListItem>();
            
            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != o)
            {
                var data = await this._gS.GetDataCountryCurrency(o, token);

                if(data != null && data.Errors == this.__notAuthorized){
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
            }
            
            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }
            
            dataJsonFactor = (string)JsonConvert.SerializeObject(c);
            
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


            dataToken = token;

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            
            

            Currency_options = new List<SelectListItem>();
            
            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != o)
            {
                var data = await this._gS.GetDataCountryCurrency(o, token);

                if (data != null && data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
            }
            
            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            
            dataJsonFactor = (string)JsonConvert.SerializeObject(c);

            OfferedNull = filter.IsOffered;

            return Page();
        }

        public async Task<JsonResult> OnPostInvoices([FromBody]RequestPagination pagination)
        {
            var token = HttpContext.Session.GetString("token");
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var data = await this._aS.GetPublications(o, "FACTOR", null, pagination.Filter, token, pagination.Pagination);
            var ordenedDataFactura = data;
            if (data.Count > 0 && String.IsNullOrEmpty(data[0].Errors))
            {
                ordenedDataFactura = data.OrderByDescending(x => x.isOffered).ThenBy(x => x.Invoice.Supplier.Name).ToList();
            }

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "FACTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(ordenedDataFactura);
        }

        public async Task<JsonResult> OnPostAuction()
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var dataAuction = await this._aS.GetAuction(o,token);
            
            if(dataAuction.Count == 0)
            {
                var auctionNotAvailable = "Auction not found";
                return new JsonResult(auctionNotAvailable);
            }

            return new JsonResult(dataAuction[0]);
        }

        public async Task<JsonResult> OnPostOfertar([FromBody]Offert oferta)
        {
            var token = HttpContext.Session.GetString("token");

            Publications data = new Publications();

            oferta.Factor_id =  this._signInManager.UserManager.GetUserId(User);

            data = await this._aS.OfferPublication(oferta,token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "FACTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }
        
    }
}