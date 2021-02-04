using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;

namespace TuFactoring.Areas.Supplier.Pages
{
    public class EdicionFacturasModel : PageModel
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

        public List<SelectListItem> Debtor_Options { get; set; }

        public List<SelectListItem> Currency_options { get; set; }

        private readonly IInvoiceService _iS;

        private readonly IPeopleService _pS;

        private readonly IAuthService _aS;

        private readonly IGlobalService _gS;

        private readonly SignInManager<User> _sg;

        #endregion
        public EdicionFacturasModel(IInvoiceService iS, IPeopleService pS, IGlobalService gS, SignInManager<User> sg, IAuthService aS)
        {
            this._iS = iS;
            this._pS = pS;
            this._aS = aS;
            this._gS = gS;
            this._sg = sg;
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

            if (HttpContext.Session.GetString("CountryInvoices") == null ||
            JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(o) ||
            JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id == null)
            {

                var data = await this._gS.GetDataCountryInvoices(Int32.Parse(o), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
            }

            var data2 = await this._pS.GetDebtors(c, token);

            if (data2.Count > 0)
            {
                if (data2[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }
            }

            HttpContext.Session.SetString("Debtors", JsonConvert.SerializeObject(data2));

            Debtor_Options = new List<SelectListItem>();

            Currency_options = new List<SelectListItem>();

            foreach (var cliente in JsonConvert.DeserializeObject<List<ResponseProveedores>>(HttpContext.Session.GetString("Debtors")))
            {
                Debtor_Options.Add(new SelectListItem() { Value = cliente.Id, Text = cliente.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
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

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(o))
            {
                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(await this._gS.GetDataCountryInvoices(Int32.Parse(o), token)));
            }

            Debtor_Options = new List<SelectListItem>();

            Currency_options = new List<SelectListItem>();

            foreach (var debtor in await this._pS.GetDebtors(c, token))
            {
                Debtor_Options.Add(new SelectListItem() { Value = debtor.Id, Text = debtor.Name });
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            financiedNull = new List<bool?>();

            for (var i = 0; i < filter.Count; i++)
            {
                dataJsonFilter.Add((string)JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);
            }

            dataJsonCurrencies = (string)JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies);

            return Page();
        }

        public async Task<JsonResult> OnPostInvoices([FromBody] RequestPagination pag)
        {
            ParametersDebtorEdition Parameters = new ParametersDebtorEdition();

            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            Parameters.Id = c;
            Parameters.Country = Int32.Parse(o);
            pag.Filter.Participant = "SUPPLIER";

            pag.Order = @"
                invoices.created_at desc, invoices.expiration_date desc
            ";
            pag.Group = "invoices.id, countries.id, programs.id";

            var data = await this._iS.GetInvoicesSupplier(Parameters, "Edicion Facturas", pag.Filter, pag.Pagination, token, pag.Order, pag.Group);

            if (data.Count == 0 || data[0].Error == null)
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

        public async Task<JsonResult> OnPostClientes()
        {
            var token = HttpContext.Session.GetString("token");

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var clientes = await this._pS.GetDebtors(c, token);

            return new JsonResult(clientes);
        }

        public async Task<JsonResult> OnPostGuardar([FromBody] Invoices invoice)
        {
            var token = HttpContext.Session.GetString("token");

            invoice.Debtor_id = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
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
        //
        public async Task<JsonResult> OnPostEliminar([FromBody] UpdateInvoice invoice)
        {
            var token = HttpContext.Session.GetString("token");            

            var data = await this._iS.DeleteInvoice(invoice, token);

            if (data.Errors == null)
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
        //
        public async Task<JsonResult> OnPostActualizar([FromBody] Invoices invoice)
        {
            var token = HttpContext.Session.GetString("token");

            invoice.Supplier_id = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            invoice.Country_id = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var data = await this._iS.UpdateInvoice(invoice, token);

            if (data.Error == null || data.Errors == null)
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

        public async Task<JsonResult> OnPostDeleteDeduction([FromBody] Charges deduction)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.DeleteDeduction(deduction, token);

            if (data.Error == null)
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

        public async Task<JsonResult> OnPostUpdateDeduction([FromBody] Charges deduction)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.UpdateDeduction(deduction, token);

            if (data.Error == null)
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

        public async Task<JsonResult> OnPostCreateDeduction([FromBody] Charges deduction)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._iS.CreateDeduction(deduction, token);

            if (data.Error == null)
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