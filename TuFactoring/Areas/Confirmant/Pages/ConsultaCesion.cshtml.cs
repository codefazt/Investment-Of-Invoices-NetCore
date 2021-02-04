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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace TuFactoring.Areas.Confirmant.Pages
{
    public class ConsultaCesionModel : PageModel
    {
        #region Data
        //Variables de  los Claims
        private readonly IInvoiceService _invoiceService;
        private readonly IAuthService _aS;
        private readonly IGlobalService _gS;
        private IHostingEnvironment _environment;


        //Variables de la pagina
        private string Country { get; set; }
        private readonly string __notAuthorized = "You are not authorised to perform this action";
        public List<SelectListItem> Currency_options { get; set; }

        [BindProperty]
        public string dataJsonFactura { get; set; }

        #endregion
        public ConsultaCesionModel(IHostingEnvironment environment, IInvoiceService invoiceService, IAuthService aS, IGlobalService gS)
        {
            _invoiceService = invoiceService;
            _environment = environment;
            this._gS = gS;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(Country))
            {
                var data = await _gS.GetDataCountryInvoices(Int32.Parse(Country), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
            }

            Currency_options = new List<SelectListItem>();

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

            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            Currency_options = new List<SelectListItem>();

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            return Page();

        }

        public async Task<JsonResult> OnPostLlenarConsultaAsync([FromBody] RequestPagination pag)
        {
            List<Publications> dataResponse = new List<Publications>();

            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var principalCurrency = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currency;

            var token = HttpContext.Session.GetString("token");

            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

            var owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var data = await _invoiceService.GetPublicationsSessionsFactor(owner, Int32.Parse(o), pag.Filter, pag.Pagination, token);

            if (data.Count == 0 || data[0].Errors == null || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token, w);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            for (int x = 0; x < data.Count; x++)
            {
                if (data[x].State != "processing")
                {
                    dataResponse.Add(data[x]);
                }
            }

            if (principalCurrency != null) 
            {
                dataResponse = dataResponse.OrderBy(x => x.currency.Id != principalCurrency.Id).ThenBy(y => y.Invoice.Supplier.Name).ThenBy(z => z.Invoice.Debtor.Name).ToList();
            } 

            return new JsonResult(dataResponse);
        }

        public async Task<JsonResult> OnPostGetPDF([FromBody] Publications publication)
        {
            var token = HttpContext.Session.GetString("token");

            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token, w);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            var refreshToken = "Token refreshed";
            return new JsonResult(refreshToken);
        }

    }

}