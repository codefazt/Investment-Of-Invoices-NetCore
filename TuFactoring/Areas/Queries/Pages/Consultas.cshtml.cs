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

namespace TuFactoring.Areas.Queries.Pages
{
    public class ConsultasModel : PageModel
    {
        #region Data
        //Variables de  los Claims
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IInvoiceService _invoiceService;
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }
        private string Discriminator { get; set; }
        private string Owner { get; set; }
        private string token { get; set; }

        //Variables de la pagina
        public string TipoParticipante { get; set; }
        public bool? financiedNull { get; set; }
        private List<Country> catologoPaises { get; set; }
        private Invoices datoFactura { get; set; }
        private List<Invoices> dataFactura { get; set; }
        public List<SelectListItem> Supplier_Options { get; set; }
        public List<SelectListItem> Debtor_Options { get; set; }
        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> Currency_options { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public string rol { get; set; }
        #endregion
        public ConsultasModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            filter = new filterInvoice()
            {
                Financied = null
            };

            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);
            financiedNull = null;

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = Participant;
            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(Country))
            {
                var data = await _globalService.GetDataCountryInvoices(Int32.Parse(Country), token);

                if (data.Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(data));
            }
            List<Invoices> facturaCheck = new List<Invoices>();
            Supplier_Options = new List<SelectListItem>();
            Debtor_Options = new List<SelectListItem>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();

            //Provicional
            ListStatus(Participant);
            //Provicional

            //GetSupplierForFactor
            if (Participant == "DEBTOR")
            {
                var proveedores = await _peopleService.GetSuppliers(Country, Owner, token);

                if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                foreach (var supplier in proveedores)
                {
                    if (!Supplier_Options.Exists(z => z.Value == supplier.Id))
                    {
                        Supplier_Options.Add(new SelectListItem() { Value = supplier.Id, Text = supplier.Name });
                    }

                }
            }
            else if (Participant == "SUPPLIER")
            {
                var clientes = await _peopleService.GetDeptors(Owner, token);

                if (clientes.Count > 0 && clientes[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                foreach (var cls in clientes)
                {
                    if (!Supplier_Options.Exists(x => x.Value == cls.Id))
                    {
                        Supplier_Options.Add(new SelectListItem() { Value = cls.Id, Text = cls.Name });
                    }

                }
            }
            else if (Participant == "CONFIRMANT" || Participant == "BACKOFFICE")
            {
                List<ResponseProveedores> clientes = null;
                List<ResponseProveedores> proveedores = null;

                if (Participant == "CONFIRMANT")
                {
                    clientes = await _peopleService.GetDeptorsForConfirmant(Confirmant, token);
                    proveedores = await _peopleService.GetSupplierForConfirmant(Confirmant, token);

                    if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
                    {
                        return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                    }
                }
                else
                {
                    clientes = await _peopleService.GetDeptorsForConfirmant(Owner,token);
                    proveedores = await _peopleService.GetSupplierForConfirmant(Owner,token);

                    if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
                    {
                        return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                    }
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
            }
            else
            {
                var proveedores = await _peopleService.GetSupplierForFactor(Owner, token);

                if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                foreach (var supplier in proveedores)
                {
                    if (!Supplier_Options.Exists(z => z.Value == supplier.Id))
                    {
                        Supplier_Options.Add(new SelectListItem() { Value = supplier.Id, Text = supplier.Name });
                    }

                }
            }


            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            return Page();
        }

        public async Task<JsonResult> OnPostLlenarConsultaAsync([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            List<Invoices> ConsultaGeneral = new List<Invoices>();
            if (Participant == "CONFIRMANT") ConsultaGeneral = await _invoiceService.GetConsultaInvoices(Confirmant, Participant, pag.Filter, pag.Pagination,token);
            else if (Participant == "BACKOFFICE") ConsultaGeneral = await _invoiceService.GetConsultaInvoices(Country, Participant, pag.Filter, pag.Pagination,token);
            else ConsultaGeneral = await _invoiceService.GetConsultaInvoices(Owner, Participant, pag.Filter, pag.Pagination,token);
            
            rol = (string)JsonConvert.SerializeObject(Participant);
            //dataJsonFactura = (string)JsonConvert.SerializeObject(ConsultaGeneral);

            return new JsonResult(ConsultaGeneral);
        }

        public async Task<IActionResult> OnPost()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            dataFilter = JsonConvert.SerializeObject(filter);

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = Participant;
            List<Invoices> facturaCheck = new List<Invoices>();
            Supplier_Options = new List<SelectListItem>();
            Debtor_Options = new List<SelectListItem>();
            Currency_options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();
            ListStatus(Participant);
            if (Participant == "DEBTOR")
            {
                var proveedores = await _peopleService.GetSuppliers(Country, Owner, token);

                if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                foreach (var supplier in proveedores)
                {
                    if (!Supplier_Options.Exists(z => z.Value == supplier.Id))
                    {
                        Supplier_Options.Add(new SelectListItem() { Value = supplier.Id, Text = supplier.Name });
                    }

                }
            }
            else if (Participant == "SUPPLIER")
            {
                var clientes = await _peopleService.GetDeptors(Owner, token);

                if (clientes.Count > 0 && clientes[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                foreach (var cls in clientes)
                {
                    if (!Supplier_Options.Exists(x => x.Value == cls.Id))
                    {
                        Supplier_Options.Add(new SelectListItem() { Value = cls.Id, Text = cls.Name });
                    }

                }
            }
            else if (Participant == "CONFIRMANT")
            {
                var clientes = await _peopleService.GetDeptorsForConfirmant(Confirmant,token);
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
            }
            else
            {
                var proveedores = await _peopleService.GetSupplierForFactor(Owner, token);

                if (proveedores.Count > 0 && proveedores[0].Errors == this.__notAuthorized)
                {
                    return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
                }

                foreach (var supplier in proveedores)
                {
                    if (!Supplier_Options.Exists(z => z.Value == supplier.Id))
                    {
                        Supplier_Options.Add(new SelectListItem() { Value = supplier.Id, Text = supplier.Name });
                    }

                }
            }

            foreach (var currency in JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Currencies)
            {
                Currency_options.Add(new SelectListItem() { Value = "" + currency.Id.Value, Text = currency.Name });
            }

            return Page();

        }

        public async Task<JsonResult> OnPostDetalleAsync([FromBody]Invoices factura)
        {

            var DetalleInvoice = await _invoiceService.GetDetalleConsultaInvoices(factura.Id,token);
            return new JsonResult(DetalleInvoice);

        }
       
        private void ListStatus(string participant)
        {
            if (participant == "DEBTOR" || participant == "BACKOFFICE")
            {
                Status_Options.Add(new SelectListItem() { Value = "draft", Text = "FACTURA CREADA" });
                Status_Options.Add(new SelectListItem() { Value = "posted", Text = "FACTURA POSTULADA" });
                Status_Options.Add(new SelectListItem() { Value = "confirmed", Text = "FACTURA CONFIRMADA" });
                Status_Options.Add(new SelectListItem() { Value = "paid", Text = "FACTURA PAGADA" });
                Status_Options.Add(new SelectListItem() { Value = "finalize", Text = "FACTURA COMPLETADA" });
                Status_Options.Add(new SelectListItem() { Value = "overdue", Text = "FACTURA VENCIDA" });
            }
            else if (participant == "CONFIRMANT" || participant == "SUPPLIER")
            {
                Status_Options.Add(new SelectListItem() { Value = "posted", Text = "FACTURA POSTULADA" });
                Status_Options.Add(new SelectListItem() { Value = "confirmed", Text = "FACTURA CONFIRMADA" });
                Status_Options.Add(new SelectListItem() { Value = "offered", Text = "FACTURA OFERTADA" });
                Status_Options.Add(new SelectListItem() { Value = "released", Text = "FACTURA LIBERADA" });
                Status_Options.Add(new SelectListItem() { Value = "published", Text = "FACTURA PUBLICADA" });
                Status_Options.Add(new SelectListItem() { Value = "sold", Text = "FACTURA VENDIDA" });
                Status_Options.Add(new SelectListItem() { Value = "processing", Text = "FACTURA EN PROCESO DE PAGOS" });
                Status_Options.Add(new SelectListItem() { Value = "paid", Text = "FACTURA PAGADA" });
                Status_Options.Add(new SelectListItem() { Value = "postponed", Text = "FACTURA EN ESPERA DEL VENCIMIENTO" });
                Status_Options.Add(new SelectListItem() { Value = "overdue", Text = "FACTURA VENCIDA" });
                Status_Options.Add(new SelectListItem() { Value = "finalize", Text = "FACTURA COMPLETADA" });
            }
            else if (participant == "FACTOR")
            {
                Status_Options.Add(new SelectListItem() { Value = "published", Text = "FACTURA PUBLICADA" });
                Status_Options.Add(new SelectListItem() { Value = "sold", Text = "FACTURA VENDIDA" });
                Status_Options.Add(new SelectListItem() { Value = "processing", Text = "FACTURA EN PROCESO DE PAGOS" });
                Status_Options.Add(new SelectListItem() { Value = "paid", Text = "FACTURA PAGADA" });
            }
        }
    }

}
