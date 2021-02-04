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

namespace TuFactoring.Areas.Debtor.Pages
{
    public class ConfirmarFacturasModel : PageModel
    {
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }

        public bool isFintech { get; set; }

        public List<bool?> financiedNull { get; set; }

        public List<SelectListItem> Currency_Options { get; set; }

        private readonly IInvoiceService _iS;
        private readonly IGlobalService _gS;
        private readonly IAuthService _aS;
        private readonly IPeopleService _pS;
        private readonly IAuctionService _auctionService;

        private readonly SignInManager<User> _signInManager;

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        public ConfirmarFacturasModel(IInvoiceService iS, SignInManager<User> signInManager, IGlobalService gS, IAuthService aS, IAuctionService auctionService, IPeopleService peopleService)
        {
            this._iS = iS;
            this._gS = gS;
            this._aS = aS;
            this._pS = peopleService;
            this._auctionService = auctionService;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            
            Currency_Options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CountryInvoices") == null)
            {
                var data = await this._gS.GetDataCountryCurrency(Int32.Parse(o), token, true);

                if(data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_Options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }
            
            filter = new List<filterInvoice>();
            dataJsonFilter = new List<string>();
            financiedNull = new List<bool?>();

            for(var i = 0; i < Currency_Options.Count;i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies);

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _pS.IsFintech(country, token);
            if (isFintech[0].Entities[0].IsFintech)
            {
                this.isFintech = true;
            }
            else
            {
                this.isFintech = false;
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

            Currency_Options = new List<SelectListItem>();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (HttpContext.Session.GetString("CountryInvoices") == null)
            {
                var data = await this._gS.GetDataCountryCurrency(Int32.Parse(o), token, true);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_Options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }
            
            financiedNull = new List<bool?>();

            for (var i = 0; i < filter.Count; i++)
            {
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);
            }
            
            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies);

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _pS.IsFintech(country, token);
            if (isFintech[0].Entities[0].IsFintech)
            {
                this.isFintech = true;
            }
            else
            {
                this.isFintech = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCandidates([FromBody]RequestPagination pag)
        {

            var token = HttpContext.Session.GetString("token");

            var w = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var data = await this._iS.GetCandidates(w, pag.Filter, pag.Pagination,token, "DEBTOR");

            if (data.Count == 0 || data[0].Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        //
        public async Task<JsonResult> OnPostConfirma([FromBody]List<UpdateInvoice> confirming)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            confirming.ForEach(item => {item.country = Int32.Parse(o); });

            var confirmInvoice = await this._iS.ConfirmInvoices(confirming,token);

            if (confirmInvoice.Count == 0 || confirmInvoice[0].Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            return new JsonResult(confirmInvoice);
        }

        public async Task<JsonResult> OnPostRechazar([FromBody] UpdateInvoice oferta)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            oferta.country = o;

            var data = await this._auctionService.RejectOffer(oferta, token);

            if (data.Errors == null || data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostRechazarAll([FromBody] List<UpdateInvoice> ofertas)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            ofertas.ForEach(x => x.country = o);

            var data = await this._auctionService.RejectAllInvoices(ofertas, token);

            if (data.Count == 0 || data[0].Errors == null || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }


        //Deducciones
        public async Task<JsonResult> OnPostDeleteDeduction([FromBody]Charges deduction)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.DeleteDeduction(deduction, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostUpdateDeduction([FromBody]Charges deduction)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.UpdateDeduction(deduction, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostCreateDeduction([FromBody]Charges deduction)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.CreateDeduction(deduction, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

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

            if (HttpContext.Session.GetString("CountryInvoicesProgram") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id != Int32.Parse(o))
            {
                HttpContext.Session.SetString("CountryInvoicesProgram", JsonConvert.SerializeObject(await this._gS.GetDataCountryInvoices(Int32.Parse(o), token)));
            }

            return new JsonResult(HttpContext.Session.GetString("CountryInvoicesProgram"));
        }

    }
}