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

namespace TuFactoring.Areas.Debtor.Pages
{
    public class CargaMasivaModel : PageModel
    {
        #region Data

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        private readonly IInvoiceService _iS;

        private readonly IPeopleService _pS;

        private readonly IGlobalService _gS;

        private readonly IAuthService _aS;

        private readonly SignInManager<User> _signInManager;
        #endregion

        public CargaMasivaModel(IInvoiceService iS, IPeopleService pS, IGlobalService gS, SignInManager<User> signInManager, IAuthService aS)
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
                
                var data = await this._pS.GetSuppliers(o, c, token);

                if (data[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("Suppliers", JsonConvert.SerializeObject(data));
            }
            catch (Exception) { }

            return Page();
        }

        public async Task<JsonResult> OnPostGuardar([FromBody]List<Invoices> facturas)
        {
            var token = HttpContext.Session.GetString("token");

            foreach (var item in facturas)
            {
                item.Country_id =Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
                item.Debtor_id = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            }

            var data = await this._iS.CreateInvoices(facturas, token);

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
    }
}