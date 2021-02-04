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
using TuFactoringModels.nuevaVersion;
using System.Globalization;

namespace TuFactoring.Areas.Debtor.Pages
{
    public class FinanciamientoFacturasModel : PageModel
    {
        #region Data
        [BindProperty]
        public List<string> dataJsonFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }

        public List<bool?> financiedNull { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";
        public List<SelectListItem> Supplier_Options { get; set; }

        public List<SelectListItem> Currency_options { get; set; }

        public List<SelectListItem> Bank_options { get; set; }
        public List<SelectListItem> Programs_Options { get; set; }

        private readonly IInvoiceService _iS;

        private readonly IGlobalService _gS;

        private readonly IPeopleService _pS;

        private readonly IAuthService _aS;

        #endregion

        public FinanciamientoFacturasModel(IInvoiceService iS, IGlobalService gS, IPeopleService pS, IAuthService aS)
        {
            this._iS = iS;
            this._pS = pS;
            this._gS = gS;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            var token = HttpContext.Session.GetString("token");
            
            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();

            var data = await this._iS.GetConfirmants(c, token);

            if (data.Count > 0 && data[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            HttpContext.Session.SetString("Confirmants", JsonConvert.SerializeObject(data));

            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != Int32.Parse(o))
            {
                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(await this._gS.GetDataCountryCurrency(Int32.Parse(o), token)));
            }

            HttpContext.Session.SetString("Suppliers", JsonConvert.SerializeObject(await this._pS.GetSuppliers(o, c,token)));
            
            Supplier_Options = new List<SelectListItem>();

            Currency_options = new List<SelectListItem>();

            Bank_options = new List<SelectListItem>();

            Programs_Options = new List<SelectListItem>();

            foreach (var supplier in JsonConvert.DeserializeObject<List<ResponseProveedores>>(HttpContext.Session.GetString("Suppliers")))
            {
                Supplier_Options.Add(new SelectListItem() { Value = supplier.Id, Text = supplier.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            foreach (var confirmant in JsonConvert.DeserializeObject<List<Entity>>(HttpContext.Session.GetString("Confirmants")))
            {
                Bank_options.Add(new SelectListItem() { Value = "" + confirmant.Id, Text = confirmant.Person.Name });
            }

            foreach (var program in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Program)
            {
                if (!Programs_Options.Exists(x => x.Text == program.Abbreviation))
                    Programs_Options.Add(new SelectListItem() { Value = "" + program.Abbreviation, Text = program.Abbreviation });
            }

            filter = new List<filterInvoice>();
            dataJsonFilter = new List<string>();
            financiedNull = new List<bool?>();

            for (var i = 0; i < Currency_options.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies);
            

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();

            #region principal
            if (HttpContext.Session.GetString("CurrencyCountry") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Id != Int32.Parse(o))
            {
                var data = await this._gS.GetDataCountryCurrency(Int32.Parse(o), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CurrencyCountry", JsonConvert.SerializeObject(data));
            }

            if (HttpContext.Session.GetString("Confirmants") == null)
            {
                var data = await this._iS.GetConfirmants(c, token);

                if (data.Count > 0 && data[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("Confirmants", JsonConvert.SerializeObject(data));
            }

            if (HttpContext.Session.GetString("Suppliers") == null)
            {
                var data = await this._pS.GetSuppliers(o, c, token);

                if (data.Count > 0 && data[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("Suppliers", JsonConvert.SerializeObject(data));
            }
            
            Supplier_Options = new List<SelectListItem>();

            Currency_options = new List<SelectListItem>();

            Bank_options = new List<SelectListItem>();

            Programs_Options = new List<SelectListItem>();

            financiedNull = new List<bool?>();

            foreach (var supplier in JsonConvert.DeserializeObject<List<ResponseProveedores>>(HttpContext.Session.GetString("Suppliers")))
            {
                Supplier_Options.Add(new SelectListItem() { Value = supplier.Id, Text = supplier.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            foreach (var program in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Program)
            {
                if (!Programs_Options.Exists(x => x.Text == program.Abbreviation))
                    Programs_Options.Add(new SelectListItem() { Value = "" + program.Abbreviation, Text = program.Abbreviation });
            }

            foreach (var confirmant in JsonConvert.DeserializeObject<List<Entity>>(HttpContext.Session.GetString("Confirmants")))
            {
                Bank_options.Add(new SelectListItem() { Value = "" + confirmant.Id, Text = confirmant.Person.Name });
            }

            for (var i = 0; i < filter.Count; i++)
            {
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CurrencyCountry")).Currencies);

            #endregion
            

            return Page();
        }

        public async Task<JsonResult> OnPostFinanciable([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            List<Financiable> dataFactura = new List<Financiable>();

            dataFactura = await this._iS.GetFinanciables(c, pag.Filter, pag.Pagination,token);
            
            foreach (var item in dataFactura)
            {
                if ((item != null))
                {
                    if(item.Publications.Count > 0)
                        item.Publication = item.Publications[item.Publications.Count -1];
                }
            }

            #region RefreshToken
            if (dataFactura.Count == 0 || dataFactura[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            #endregion

            return new JsonResult(dataFactura);
        }
        
        public async Task<JsonResult> OnPostFinanciar([FromBody]List<Invoices> facturas)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.FinancingInvoices(facturas, token);

            #region RefresToken
            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            #endregion

            return new JsonResult(data);
        }



    }
}