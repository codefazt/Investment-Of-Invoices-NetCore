using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Confirmant.Pages
{
    public class FacturasConciliadasModel : PageModel
    {
        private Pagos pays { get; set; } = new Pagos();
        public string JsonPagosReconciled { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        private readonly IPeopleService _peopleService;
        private readonly IGlobalService _globalService;
        public bool isFintech { get; set; }
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _authService;
        private readonly SignInManager<User> _signInManager;
        private string Participant { get; set; }
        private string Owner { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";


        public FacturasConciliadasModel(SignInManager<User> signInManager, IPeopleService peopleService, IGlobalService globalService, IPaymentService paymentService, IAuthService authService)
        {
            _paymentService = paymentService;
            _authService = authService;
            _globalService = globalService;
            _peopleService = peopleService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(o))
            {
                var currencies = await _globalService.GetDataCountryInvoices(Int32.Parse(o), token);

                if (currencies.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(currencies));
            }

            Currency_options = new List<SelectListItem>();

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            var principalCurrency = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currency;

            var dataReconciled = await this._paymentService.GetAplicationPays(o, c, token, "RECONCILED");

            if (dataReconciled.Count > 0 && dataReconciled[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            if (principalCurrency != null)
            {
                dataReconciled = dataReconciled.OrderBy(x => x.Currency.Id != principalCurrency.Id).ThenBy(y => y.Receiver.Name).ThenBy(z => z.Person.Name).ToList();
            }

            JsonPagosReconciled = JsonConvert.SerializeObject(dataReconciled);

            //Refresh Token
            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._authService.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token, w);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _peopleService.IsFintech(country, token);
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

        public async Task<JsonResult> OnPostPay([FromBody]Payment pago)
        {
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            
            var buttonPay = await _paymentService.MutationPayReceipt(pago, token);
            
            if (buttonPay.Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authService.RefreshToken(id, CultureInfo.CurrentCulture.Name, "CONFIRMANT", token,c);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(buttonPay);
        }
    }
}
