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
    public class PublicarFacturasModel : PageModel
    {
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";
        public List<SelectListItem> Currency_options { get; set; }
        public List<SelectListItem> Programs_Options { get; set; }

        private readonly IAuctionService _aS;
        private readonly IAuthService _auS;
        private readonly IGlobalService _gS;
        private readonly SignInManager<User> _signInManager;

        public PublicarFacturasModel(IAuctionService aS, IGlobalService gS, SignInManager<User> signInManager, IAuthService auS)
        {
            this._aS = aS;
            this._gS = gS;
            this._auS = auS;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            Currency_options = new List<SelectListItem>();
            Programs_Options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CountryInvoicesProgram") == null ||
                JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id != o ||
                JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id == null)
            {

                var data = await this._gS.GetDataCountryInvoices(o, token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoicesProgram", JsonConvert.SerializeObject(data));
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            foreach (var program in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Program)
            {
                if (!Programs_Options.Exists(x => x.Text == program.Abbreviation))
                    Programs_Options.Add(new SelectListItem() { Value = "" + program.Abbreviation, Text = program.Abbreviation });
            }

            filter = new List<filterInvoice>();
            dataJsonFilter = new List<string>();

            for (var i = 0; i < Currency_options.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies);


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            Currency_options = new List<SelectListItem>();
            Programs_Options = new List<SelectListItem>();

            if (HttpContext.Session.GetString("CountryInvoicesProgram") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id != o)
            {
                HttpContext.Session.SetString("CountryInvoicesProgram", JsonConvert.SerializeObject(await this._gS.GetDataCountryInvoices(o, token)));
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            foreach (var program in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Program)
            {
                if (!Programs_Options.Exists(x => x.Text == program.Abbreviation))
                    Programs_Options.Add(new SelectListItem() { Value = "" + program.Abbreviation, Text = program.Abbreviation });
            }

            for (var i = 0; i < filter.Count; i++)
            {
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Currencies);

            return Page();
        }

        public async Task<JsonResult> OnPostReleasable([FromBody] RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var dataFacturas = await this._aS.GetPublishable(c,pag.Type,pag.Filter,pag.Pagination,token);

            if (dataFacturas.Count == 0 || dataFacturas[0].Error == null || dataFacturas[0].Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._auS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(dataFacturas);
        }

        public async Task<JsonResult> OnPostPublicar([FromBody]UpdateInvoice factura)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            factura.country = o;

            var data = await this._aS.PublicationsInvoice(factura, token);

            if (data.Errors == null || data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._auS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostPosponer([FromBody]UpdateInvoice factura)
        {

            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            factura.country = o;

            var data = await this._aS.PostponeInvoice(factura, token);

            if (data.Errors == null || data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._auS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }
        public async Task<JsonResult> OnPostPublicarAll([FromBody]List<UpdateInvoice> ofertas)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            ofertas.ForEach(x => x.country = o);

            var data = await this._aS.PublicationsAllInvoices(ofertas,token);

            if (data.Count == 0 || data[0].Errors == null || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._auS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostPosponerAll([FromBody]List<UpdateInvoice> ofertas)
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            ofertas.ForEach(x => x.country = o);

            var data = await this._aS.PosponeAllInvoices(ofertas,token);

            if (data.Count == 0 || data[0].Errors == null || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._auS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

    }
}