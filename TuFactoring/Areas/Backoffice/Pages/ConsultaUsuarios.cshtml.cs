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

namespace TuFactoring.Areas.Backoffice.Pages
{
    public class ConsultaUsuariosModel : PageModel
    {
        //Variables de  los Claims
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IInvoiceService _invoiceService;
        private readonly IAuthService _aS;
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }
        private string Owner { get; set; }

        //Variables de la pagina
        ListCountry Identifications;
        public string TipoParticipante { get; set; }
        public bool? financiedNull { get; set; }

        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> State_Options { get; set; }
        public List<SelectListItem> City_Options { get; set; }
        public List<SelectListItem> Discriminator_options { get; set; }
        public List<SelectListItem> Participant_options { get; set; }

        [BindProperty]
        public string dataJsonFactura { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        [BindProperty]
        public string IdenJson { get; set; }
        [BindProperty]
        public string listadosInicialesJson { get; set; }

        public ConsultaUsuariosModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IInvoiceService invoiceService, IAuthService aS)
        {
            _invoiceService = invoiceService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout");

            filter = new filterInvoice() { Financied = null };
            dataFilter = JsonConvert.SerializeObject(filter);
            financiedNull = null;
            TipoParticipante = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            //----------- Llenado de Country
            if (HttpContext.Session.GetString("CountryIdentifications") == null ||
                HttpContext.Session.GetString("CountryIdentifications") == "" ||
                HttpContext.Session.GetString("CountryIdentifications") == "null")
            {
                Identifications = await _globalService.ConsultaRegiosAndCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                HttpContext.Session.SetString("CountryIdentifications", JsonConvert.SerializeObject(Identifications));
            }
            else Identifications = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryIdentifications"));
            //----------- Llenado de Country

            City_Options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();
            Status_Options.Add(new SelectListItem() { Value = "0", Text = "Activo" });
            Status_Options.Add(new SelectListItem() { Value = "1", Text = "Bloqueado" });

            Discriminator_options = new List<SelectListItem>();
            Discriminator_options.Add(new SelectListItem() { Value = "LEGAL", Text = "Persona Legal" });
            Discriminator_options.Add(new SelectListItem() { Value = "PERSON", Text = "Persona Natural" });

            Participant_options = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "DEBTOR", Text = "EMPRESAS" },
                new SelectListItem() { Value = "SUPPLIER", Text = "PROVEEDORES" },
                new SelectListItem() { Value = "CONFIRMANT", Text = "BANCOS" },
                new SelectListItem() { Value = "FACTOR", Text = "INVERSIONISTAS" }
            };

            //Para llenar El filtro de Estados
            State_Options = new List<SelectListItem>();
            foreach(var state in Identifications.Regions)
            {
                State_Options.Add(new SelectListItem() { Value = state.id.ToString(), Text = state.Name });
            }

            listadosInicialesJson = JsonConvert.SerializeObject(Identifications);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            dataFilter = JsonConvert.SerializeObject(filter);

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = Participant;

            //var listaClientes = await _peopleService.GetLsitEmailClientConfirmant(Confirmant);
            //----------- Llenado de Country
            if (HttpContext.Session.GetString("CountryIdentifications") == null ||
                HttpContext.Session.GetString("CountryIdentifications") == "" ||
                HttpContext.Session.GetString("CountryIdentifications") == "null")
            {
                Identifications = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                HttpContext.Session.SetString("CountryIdentifications", JsonConvert.SerializeObject(Identifications));
            }
            else Identifications = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryIdentifications"));
            //----------- Llenado de Country
            listadosInicialesJson = JsonConvert.SerializeObject(Identifications);


            City_Options = new List<SelectListItem>();
            Status_Options = new List<SelectListItem>();
            Status_Options.Add(new SelectListItem() { Value = "0", Text = "Activo" });
            Status_Options.Add(new SelectListItem() { Value = "1", Text = "Bloqueado" });

            Discriminator_options = new List<SelectListItem>();
            Discriminator_options.Add(new SelectListItem() { Value = "LEGAL", Text = "Persona Legal" });
            Discriminator_options.Add(new SelectListItem() { Value = "PERSON", Text = "Persona Natural" });

            Participant_options = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "DEBTOR", Text = "EMPRESAS" },
                new SelectListItem() { Value = "SUPPLIER", Text = "PROVEEDORES" },
                new SelectListItem() { Value = "CONFIRMANT", Text = "BANCOS" },
                new SelectListItem() { Value = "FACTOR", Text = "INVERSIONISTAS" }
            };
            //Para llenar El filtro de Estados
            State_Options = new List<SelectListItem>();
            foreach (var state in Identifications.Regions)
            {
                State_Options.Add(new SelectListItem() { Value = state.id.ToString(), Text = state.Name });
                if (state.Cities != null)
                {
                    foreach (var city in state.Cities) City_Options.Add(new SelectListItem() { Value = city.Id.ToString(), Text = city.Name });
                }

            }
            foreach (var item in City_Options)
            {
                if (item.Value == filter.City.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLlenarConsultaAsync([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var ListaClientesBanco = await _peopleService.GetConsultaUsersBackOffice(new ParamProspecto { Country = int.Parse(Country), Filter=pag.Filter, Pagination= pag.Pagination }, token);
            if (ListaClientesBanco.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null) HttpContext.Session.SetString("token", l.Token);
            }
            return new JsonResult(ListaClientesBanco);
        }

        public async Task<JsonResult> OnPostDetalleAsync([FromBody]Prospecto person)
        {
            var token = HttpContext.Session.GetString("token");
            DetallesClientes detallesClientes = new DetallesClientes();
            ListCountry Pais = new ListCountry();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            filterInvoice PeopleId = new filterInvoice();
            PeopleId.Id = person.Id;
            person = await _peopleService.GetDetalleClientConfirmant(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
            if (person.Error == null)
            {
                detallesClientes.Error = person.Error;
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                if (l.Error == null) HttpContext.Session.SetString("token", l.Token);
            }

            //----------- Llenado de Country
            if (person.Addresses != null && person.Addresses.Count > 0) Pais = await _globalService.ConsultaIdentificationsAndCitiesTF(new ParamCountry { Id = int.Parse(Country), Region = person.Addresses[0].Region });
            else Pais = await _globalService.ConsultasCountryTF(new ParamCountry { Id = int.Parse(Country) });
            //----------- Llenado de Country

            foreach (var iden in Pais.Identifications)
            {
                if (iden.Id == person.Documents[0].Identification && iden.Prefix == false)
                {
                    detallesClientes.Document.Abbreviation = iden.Abbreviation;
                    detallesClientes.Document.Number = person.Documents[0].Number;
                }

                if (person.Contacts != null)
                {
                    foreach (var contacto in person.Contacts)
                    {
                        if (contacto.Label == "LEGAL")
                        {
                            detallesClientes.Representante.DocumentNumber = contacto.DocumentNumber;
                            detallesClientes.Representante.Name = contacto.Name;
                            detallesClientes.Representante.PhoneNumber = contacto.PhoneNumber;
                            detallesClientes.Representante.Email = contacto.Email;
                            if (iden.Id == contacto.Identification && iden.Prefix == false) detallesClientes.Representante.Label = iden.Abbreviation;

                            else if (iden.Id == contacto.Identification && iden.Prefix == true)
                            {
                                foreach (var prefx in iden.Prefixes)
                                {
                                    if (prefx.Id == contacto.Prefix) detallesClientes.Representante.Label = prefx.Abbreviation;
                                }
                            }
                        }
                        else if (contacto.Label == "CONTACT")
                        {
                            detallesClientes.Contacto.DocumentNumber = contacto.DocumentNumber;
                            detallesClientes.Contacto.Name = contacto.Name;
                            detallesClientes.Contacto.PhoneNumber = contacto.PhoneNumber;
                            detallesClientes.Contacto.Email = contacto.Email;
                            if (iden.Id == contacto.Identification && iden.Prefix == false) detallesClientes.Contacto.Label = iden.Abbreviation;

                            else if (iden.Id == contacto.Identification && iden.Prefix == true)
                            {
                                foreach (var prefx in iden.Prefixes)
                                {
                                    if (prefx.Id == contacto.Prefix) detallesClientes.Contacto.Label = prefx.Abbreviation;
                                }
                            }
                        }
                    }
                }
            }
            if (Pais.Regions != null && Pais.Regions.Count > 0)
            {
                if (Pais.Regions[0].Cities != null && Pais.Regions[0].Cities.Count > 0)
                {
                    foreach (var city in Pais.Regions[0].Cities)
                    {
                        if (person.Addresses[0].City == city.Id)
                        {
                            detallesClientes.Address.Region = Pais.Regions[0].Name;
                            detallesClientes.Address.City = city.Name;
                            continue;
                        }
                    }
                }
            }

            detallesClientes.Company = person.Company;
            if (person.Phones != null && person.Phones.Count > 0) detallesClientes.Phone = person.Phones[0].Number;
            if (person.Addresses != null && person.Addresses.Count > 0) detallesClientes.Address.Line1 = person.Addresses[0].Line1;
            if (person.Addresses != null && person.Addresses.Count > 0) detallesClientes.Address.Line2 = person.Addresses[0].Line2;

            if (person.Quotas != null)
            {
                foreach (var limite in person.Quotas)
                {
                    if (limite.Abbreviation == "CREDIT" || limite.Abbreviation == "FINANCING")
                    {
                        foreach (var coin in Pais.Currencies)
                        {
                            if (coin.Id == limite.Currency)
                            {
                                detallesClientes.LimiteCredito.Currency = coin.Symbol;
                                //detallesClientes.LimiteCredito.Abbreviation = coin.Iso_4217;
                            }
                        }
                        detallesClientes.LimiteCredito.Abbreviation = limite.Abbreviation;
                        detallesClientes.LimiteCredito.Available = limite.Available;
                    }
                }
            }
            else detallesClientes.LimiteCredito = null;

            if (person.Accounts != null)
            {
                foreach (var cuenta in person.Accounts)
                {
                    if (cuenta.Default == true)
                    {
                        foreach (var bank in Pais.Entities)
                        {
                            if (bank.Id == cuenta.Entity.Id) detallesClientes.CuentaBancaria.Entity = bank.Person.Name;
                        }
                        detallesClientes.CuentaBancaria.AccountNumber = cuenta.AccountNumber;
                        detallesClientes.CuentaBancaria.AccountType = cuenta.AccountType;
                        continue;
                    }
                    detallesClientes.CuentaBancaria = null;
                }
            }
            else detallesClientes.CuentaBancaria = null;
            if (detallesClientes.Contacto.DocumentNumber == null || detallesClientes.Contacto.DocumentNumber == "") detallesClientes.Contacto = null;
            if (person.Discriminator == "PERSON")
            {
                detallesClientes.Company = person.FirstName;
                if (person.Emails != null) detallesClientes.Email = person.Emails[0].Address;
                detallesClientes.Representante = null;
                detallesClientes.Contacto = null;
            }

            return new JsonResult(detallesClientes);
        }
        public async Task<JsonResult> OnPostSelectCity([FromBody]GlobalRegister Region)
        {
            ListCountry country = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = Region.State });
            return new JsonResult(country.Regions[0].Cities);

        }
    }
}
