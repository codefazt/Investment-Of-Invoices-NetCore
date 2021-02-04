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
    public class ConsultasModel : PageModel
    {
        #region Data
        //Variables de  los Claims
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IInvoiceService _invoiceService;
        private readonly IAuthService _aS;
        private string Confirmant { get; set; }
        private string Country { get; set; }


        //Variables de la pagina
        public string Owner { get; set; }
        public bool isFintech { get; set; }
        public bool? financiedNull { get; set; }
        private List<Invoices> dataFactura { get; set; }
        public List<SelectListItem> Supplier_Options { get; set; }
        public List<SelectListItem> Debtor_Options { get; set; }
        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> Currency_options { get; set; }
        public List<SelectListItem> Programs_Options { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public bool get { get; set; }
        
        #endregion
        public ConsultasModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IInvoiceService invoiceService, IAuthService aS)
        {
            _invoiceService = invoiceService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            get = true;

            filter = new filterInvoice()
            {
                Financied = null,
            };

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);
            financiedNull = null;
            
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
           
            if (HttpContext.Session.GetString("CountryInvoicesProgram") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoicesProgram")).Id != Int32.Parse(Country))
            {
                var data = await _globalService.GetDataCountryInvoices(Int32.Parse(Country), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoicesProgram", JsonConvert.SerializeObject(data));
            }
            List<Invoices> facturaCheck = new List<Invoices>();
            Supplier_Options = new List<SelectListItem>();
            Debtor_Options = new List<SelectListItem>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();

            Programs_Options = new List<SelectListItem>();

            ListStatus();
            
            
            List<ResponseProveedores> clientes = null;
            List<ResponseProveedores> proveedores = null;

               
            clientes = await _peopleService.GetDeptorsForConfirmant(Confirmant, token);

            if (clientes.Count > 0 && clientes[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }


            proveedores = await _peopleService.GetSupplierForConfirmant(Confirmant, token);
            

            if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }               

            foreach (var prov in proveedores)
            {
                if (!Supplier_Options.Exists(x => x.Value == prov.Id))
                {
                    Supplier_Options.Add(new SelectListItem() { Value = prov.Id, Text = prov.Name });
                }

            }

            foreach (var cls in clientes)
            {
                if (!Debtor_Options.Exists(x => x.Value == cls.Id))
                {
                    Debtor_Options.Add(new SelectListItem() { Value = cls.Id, Text = cls.Name });
                }

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

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _peopleService.IsFintech(country, token);
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
            get = false;

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);
            
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            List<Invoices> facturaCheck = new List<Invoices>();
            Supplier_Options = new List<SelectListItem>();
            Debtor_Options = new List<SelectListItem>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();

            Programs_Options = new List<SelectListItem>();
            ListStatus();
            
            
            var clientes = await _peopleService.GetDeptorsForConfirmant(Confirmant, token);

            if (clientes.Count > 0 && clientes[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var proveedores = await _peopleService.GetSupplierForConfirmant(Confirmant, token);

            if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            foreach (var prov in proveedores)
            {
                if (!Supplier_Options.Exists(x => x.Value == prov.Id))
                {
                    Supplier_Options.Add(new SelectListItem() { Value = prov.Id, Text = prov.Name });
                }

            }

            foreach (var cls in clientes)
            {
                if (!Debtor_Options.Exists(x => x.Value == cls.Id))
                {
                    Debtor_Options.Add(new SelectListItem() { Value = cls.Id, Text = cls.Name });
                }

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

            var country = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var isFintech = await _peopleService.IsFintech(country, token);
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

        public async Task<JsonResult> OnPostLlenarConsultaAsync([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

            pag.Order = @"
                invoices.participant,
                case when countries.currency_id = invoices.currency_id then 0 else invoices.currency_id end asc,supplier.name,people.name, 
                invoices.expiration_date desc";

            pag.Group = "invoices.id, countries.currency_id,supplier.id,people.id";
            
            var data = await _invoiceService.GetConsultaInvoices(Confirmant, "CONFIRMANT", pag.Filter, pag.Pagination, token, pag.Order, pag.Group, true);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            if (Utils.filterIsEmpty(pag.Filter) && data.Count > 0)
            {
                data = data.FindAll(x => x.Term_days > 0);
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostDetalleAsync([FromBody]Invoices factura)
        {
            var token = HttpContext.Session.GetString("token");
            
            var data = await _invoiceService.GetDetalleConsultaInvoices(factura.Id, token);

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

        private void ListStatus()
        {
            Status_Options.Add(new SelectListItem() { Value = "revised", Text = "revisedStatus" });
            Status_Options.Add(new SelectListItem() { Value = "posted", Text = "postedStatus" });
            Status_Options.Add(new SelectListItem() { Value = "rejectedForClient", Text = "rejectedForClientStatus" });
            Status_Options.Add(new SelectListItem() { Value = "confirmedForClient", Text = "notificationConfirmedStatus" });
            Status_Options.Add(new SelectListItem() { Value = "confirmedToOverdue", Text = "confirmedToOverdueStatus" });
            Status_Options.Add(new SelectListItem() { Value = "offeredBank", Text = "offeredBankStatus" });
            Status_Options.Add(new SelectListItem() { Value = "soldBank", Text = "soldBankStatus" });
            Status_Options.Add(new SelectListItem() { Value = "processing", Text = "processingStatus" });
            Status_Options.Add(new SelectListItem() { Value = "soldToOverdue", Text = "soldToOverdueStatus" });
            Status_Options.Add(new SelectListItem() { Value = "finalizeBank", Text = "canceledStatus" });
        }
    }

}
