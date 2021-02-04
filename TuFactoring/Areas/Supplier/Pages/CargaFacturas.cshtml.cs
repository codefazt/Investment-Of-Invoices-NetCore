using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;
using System.Globalization;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Supplier.Pages
{
    public class CargaFacturasModel : PageModel
    {
        #region Data

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        private readonly IInvoiceService _iS;

        private readonly IPeopleService _pS;

        private readonly IGlobalService _gS;

        private readonly IAuthService _aS;

        private readonly SignInManager<User> _signInManager;
        #endregion

        public CargaFacturasModel(IInvoiceService iS, IPeopleService pS, IGlobalService gS, SignInManager<User> signInManager, IAuthService aS)
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

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            try
            {
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

                if (data[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("Debtors", JsonConvert.SerializeObject(data));
            }
            catch (Exception) { }

            return Page();
        }

        public async Task<JsonResult> OnPostClientes()
        {
            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var clientes = await this._pS.GetDebtors(c, token);

            return new JsonResult(clientes);
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

        public async Task<JsonResult> OnPostCrear([FromBody] Invoices invoice)
        {
            var token = HttpContext.Session.GetString("token");

            invoice.Supplier_id = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            invoice.Country_id = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            Invoices Data = await this._iS.CreateInvoice(invoice, token);

            if (Data.Error == null || Data.Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(Data);
        }

        public async Task<JsonResult> OnPostGuardar([FromBody] List<Invoices> facturas)
        {
            var token = HttpContext.Session.GetString("token");

            foreach (var item in facturas)
            {
                item.Country_id = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
                item.Supplier_id = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            }

            var data = await this._iS.CreateInvoices(facturas, token);

            if (data.Count == 0 || data[0].Errors == null)
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
