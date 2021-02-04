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

namespace TuFactoring.Areas.Confirmant.Pages
{
    public class CompraFacturaModel : PageModel
    {
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }

        public List<bool?> financiedNull { get; set; }
        public bool isFintech { get; set; }

        public List<SelectListItem> Supplier_Options { get; set; }

        public List<SelectListItem> Debtor_Options { get; set; }

        public List<SelectListItem> Currency_Options { get; set; }

        public List<SelectListItem> Programs_Options { get; set; }

        private readonly IInvoiceService _iS;

        private readonly SignInManager<User> _signInManager;

        private readonly IGlobalService _gS;

        private readonly IAuthService _aS;

        private readonly IPeopleService _pS;

        private readonly IAuctionService _auctionService;

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        public CompraFacturaModel(IInvoiceService iS, SignInManager<User> signInManager, IGlobalService gS, IAuthService aS, IAuctionService auctionService, IPeopleService peopleService)
        {
            this._iS = iS;
            this._pS = peopleService;
            this._auctionService = auctionService;
            this._signInManager = signInManager;
            this._aS = aS;
            this._gS = gS;
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

            Programs_Options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CountryInvoicesProgram") == null ||
                JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id != Int32.Parse(o) ||
                JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id == null)
            {

                var data = await this._gS.GetDataCountryInvoices(Int32.Parse(o), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoicesProgram", JsonConvert.SerializeObject(data));
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies)
            {
                Currency_Options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            foreach (var program in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Program)
            {
                if (!Programs_Options.Exists(x => x.Text == program.Abbreviation))
                    Programs_Options.Add(new SelectListItem() { Value = "" + program.Abbreviation, Text = program.Abbreviation });
            }

            filter = new List<filterInvoice>();
            dataJsonFilter = new List<string>();
            financiedNull = new List<bool?>();

            for (var i = 0; i < Currency_Options.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies);

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _pS.IsFintech(country, token);
            if (isFintech != null || isFintech.Count > 0)
            {
                if (isFintech[0].Entities[0].IsFintech)
                {
                    this.isFintech = true;
                }
                else
                {
                    this.isFintech = false;
                }
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
            Programs_Options = new List<SelectListItem>();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (HttpContext.Session.GetString("CountryInvoicesProgram") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id != Int32.Parse(o))
            {
                HttpContext.Session.SetString("CountryInvoicesProgram", JsonConvert.SerializeObject(await this._gS.GetDataCountryInvoices(Int32.Parse(o), token)));
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies)
            {
                Currency_Options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            foreach (var program in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Program)
            {
                if (!Programs_Options.Exists(x => x.Text == program.Abbreviation))
                    Programs_Options.Add(new SelectListItem() { Value = "" + program.Abbreviation, Text = program.Abbreviation });
            }

            financiedNull = new List<bool?>();

            for (var i = 0; i < filter.Count; i++)
            {
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies);

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _pS.IsFintech(country, token);
            if (isFintech != null || isFintech.Count > 0)
            {
                if (isFintech[0].Entities[0].IsFintech)
                {
                    this.isFintech = true;
                }
                else
                {
                    this.isFintech = false;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmeds([FromBody]RequestPagination pag)
        {
            List<Publications> confirmeds = new List<Publications>();

            var token = HttpContext.Session.GetString("token");

            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

            var data = await this._iS.GetConfirmed(w, pag.Filter, pag.Pagination, token);

            if (data.Count == 0 || data[0].Errors == null || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token,w);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            for(int x = 0; x < data.Count; x++)
            {
                if(data[x].Invoice.Term_days >= 30 && data[x].Program.Abbreviation == "DIRECT" || data[x].Program.Abbreviation == "CONFIRMING")
                {
                    confirmeds.Add(data[x]);
                }
            }

            return new JsonResult(confirmeds);
        }

        public async Task<JsonResult> OnPostOferta([FromBody] List<OffertInvoice> offerting)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

            offerting.ForEach(item => { item.confirmant = w; item.country = Int32.Parse(o); });

            var data = await this._iS.OffertInvoices(offerting,token);

            if (data.Count == 0 || data[0].Error == null || data[0].Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token, w);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostRechazar([FromBody] UpdateInvoice invoice)
        {

            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

            invoice.country = o;

            var data = await this._auctionService.RejectOfferMarket(invoice, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token,w);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }
    }
}