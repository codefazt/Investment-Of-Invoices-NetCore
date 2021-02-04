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
using TuFactoringModels.nuevaVersion;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TuFactoring.Areas.Factor.Pages
{
    public class PagoFacturasModel : PageModel
    {
        private Pagos pagos { get; set; } = new Pagos();
        public string JsonPagos { get; set; }

        public string payDate { get; set; }

        public string receiptsDate { get; set; }

        private readonly IPeopleService _peopleService;
        private readonly IGlobalService _globalService;
        private readonly IAuctionService _auctionService;
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _aS;
        private readonly SignInManager<User> _signInManager;
        public List<SelectListItem> Currency_options { get; set; }
        private string Participant { get; set; }
        private string Owner { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";
        public int maxLengthAccount { get; set; } = 20;

        public PagoFacturasModel(SignInManager<User> signInManager, IAuctionService auctionService, IPeopleService peopleService, IGlobalService globalService, IPaymentService paymentService, IAuthService aS)
        {
            _auctionService = auctionService;
            _paymentService = paymentService;
            _globalService = globalService;
            _peopleService = peopleService;
            _signInManager = signInManager;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Country PaisesBanks = new Country();
            List<Receipts> dataFiltered = new List<Receipts>();

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(Country))
            {
                var currencies = await _globalService.GetDataCountryInvoices(Int32.Parse(Country), token);

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

            if (HttpContext.Session.GetString("PaisesBanks") == null)
            {
                PaisesBanks = await _globalService.ConsultaBanksAsync(Int32.Parse(Country), token);

                if (PaisesBanks != null && PaisesBanks.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("PaisesBanks", JsonConvert.SerializeObject(PaisesBanks));
            }
            else
            {
                PaisesBanks = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("PaisesBanks"));
            }
            try
            {
                pagos.Settings = PaisesBanks.Settings.FindAll(x => x.Abbreviation == "REGEXP_ACCOUNT_NUMBER");


                if (PaisesBanks.Settings != null)
                {

                    var setti = PaisesBanks.Settings.First(x => x.Abbreviation == "MAXLEN_ACCOUNT_NUMBER");
                    this.maxLengthAccount = Int32.Parse(setti.Content);

                }

            }
            catch (Exception ex)
            {
                var msj = ex.Message;
                pagos.Entities = new List<Entity>();
            }

            var dataAuction = await _auctionService.GetAuction(Int32.Parse(Country), token);

            if (dataAuction.Count > 0 && dataAuction[0].Error == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }


            var data = await _paymentService.ConsultaFacturasAsyncFactor(Owner, Country, "SALE", "draft,processing,unpaid", token);

            if (data != null && data.Count > 0 && data[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            for (int x = 0; x < data.Count; x++)
            {
                if (data[x].Publications[0].Discount != 0)
                {
                    dataFiltered.Add(data[x]);
                }
            }

            var accounts = await this._peopleService.GetConsultaAccountsPeople(new ParamProspecto()
            {
                Country = Int32.Parse(Country),
                Participant = Participant,
                Filter = new filterInvoice()
                {
                    Id = Owner
                }
            }, token);

            if (accounts != null && accounts.Count > 0 && accounts[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }


            accounts = accounts.FindAll(x => x.Status);


            pagos.Entities = new List<Entity>();

            if (accounts != null)
            {
                for (var i = 0; i < accounts.Count; i++)
                {
                    if (pagos.Entities.FirstOrDefault(x => x.Id == accounts[i].Entity.Id) == null)
                    {
                        pagos.Entities.Add(accounts[i].Entity);
                    }

                }

            }

            if (principalCurrency != null)
            {
                dataFiltered = dataFiltered.OrderBy(x => x.Currency.Id != principalCurrency.Id).ThenBy(y => y.Entity.Person.Name).ToList();
            }

            pagos.Subasta = dataAuction;

            pagos.Accounts = accounts;
            pagos.Facturas = dataFiltered;
            JsonPagos = JsonConvert.SerializeObject(pagos);

            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "FACTOR", token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }

            return Page();
        }

        public async Task<JsonResult> OnPostPay([FromBody] TuFactoringModels.Payment pago)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await _paymentService.MutacionPagoFacturasAsync(pago, token);

            if (data == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "FACTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            return new JsonResult(data);
        }
    }
}
