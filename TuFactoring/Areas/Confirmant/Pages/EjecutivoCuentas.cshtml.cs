using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;
using TuFactoringModels.nuevaVersion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace TuFactoring.Areas.Confirmant.Pages
{
    public class EjecutivoCuentasModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _aS;

        public string ListaEjcutivoCuenta { get; set; }
        private Prospectos prospectoLimiteCredito { get; set; } = new Prospectos();
        private List<consultaVerificacion> Segmentados { get; set; } = new List<consultaVerificacion>();
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }
        private string IdUser { get; set; }
        private string token { get; set; }

        //Variables de la pagina
        [BindProperty]
        public string listadosInicialesJson { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }
        [BindProperty]
        public List<string> dataFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        ListCountry Identifications;
        public List<bool?> financiedNull { get; set; }
        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> State_Options { get; set; }
        public List<SelectListItem> City_Options { get; set; }
        public List<SelectListItem> Discriminator_options { get; set; }
        public List<SelectListItem> Participant_options { get; set; }

        public EjecutivoCuentasModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IPaymentService paymentService, IAuthService aS)
        {
            _paymentService = paymentService;
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout");
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            //----------- Llenado de Country
            /*
             if (HttpContext.Session.GetString("CountryIdentifications") == null ||
                HttpContext.Session.GetString("CountryIdentifications") == "" ||
                HttpContext.Session.GetString("CountryIdentifications") == "null")
            {
                Identifications = await _globalService.ConsultaRegiosAndCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                HttpContext.Session.SetString("CountryIdentifications", JsonConvert.SerializeObject(Identifications));
            }
            else Identifications = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryIdentifications"));
             */
            //----------- Llenado de Country

            Identifications = await _globalService.ConsultaRegiosAndCitiesTF(new ParamCountry { Id = Int32.Parse(Country) });

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
            }

            filter = new List<filterInvoice>();
            financiedNull = new List<bool?>();
            dataFilter = new List<string>();

            for (var i=0; i < Identifications.Currencies.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataFilter.Add(JsonConvert.SerializeObject(filter[i]));
            }
            //filter = new filterInvoice() { Financied = false };
            
            dataJsonCurrencies = JsonConvert.SerializeObject(Identifications.Currencies);
            listadosInicialesJson = JsonConvert.SerializeObject(Identifications);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

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
            dataJsonCurrencies = JsonConvert.SerializeObject(Identifications.Currencies);

            City_Options = new List<SelectListItem>();
            Discriminator_options = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "LEGAL", Text = "Persona Legal" },
                new SelectListItem() { Value = "PERSON", Text = "Persona Natural" }
            };
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
            financiedNull = new List<bool?>();
            for (var i = 0; i < filter.Count; i++)
            {
                dataFilter.Add(JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);
                
            }
            return Page();
        }
        public async Task<JsonResult> OnPostLlenarTabla([FromBody]RequestPagination pag)
        {
            IdUser = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var currency = pag.Filter.Currency_id;
            if(pag.Filter != null)
            {
                if((pag.Filter.AmountRiskTo == null && pag.Filter.AmountRiskAvailableFrom == null && pag.Filter.AmountRiskAvailableTo == null && pag.Filter.AmountRiskFrom == null)) pag.Filter.Currency_id = null;
            }
            
            prospectoLimiteCredito.List = new List<Prospecto>();
            var Estados = await _globalService.ConsultaIdentificationsAndCitiesTF(new ParamCountry { Id = Int32.Parse(Country) });
            var listaUsuarios = await _peopleService.ConsultaDatosEjecutivoCuentasAsync(new ParamCreditLimit { Confirmant = Confirmant, Country = int.Parse(Country), User = IdUser, Filter = pag.Filter, Pagination = pag.Pagination }, token);
            if (listaUsuarios.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null) HttpContext.Session.SetString("token", l.Token);
            }

            if (listaUsuarios != null)
            {
                if (listaUsuarios.List != null)
                {
                    foreach (var usuario in listaUsuarios.List)
                    {
                        if (usuario.Identities != null)
                        {
                            foreach (var participante in usuario.Identities)
                            {
                                if (participante.Participant == "DEBTOR") prospectoLimiteCredito.List.Add(usuario);
                            }
                        }
                    }
                }
                else
                {
                    prospectoLimiteCredito = null;
                }
            }

            prospectoLimiteCredito.Error = listaUsuarios.Error;
            return new JsonResult(new { prospecto = prospectoLimiteCredito, estados = Estados, idCurrency = currency });
        }
        public async Task<JsonResult> OnPostDetalleCliente([FromBody]Prospecto person)
        {
            DetallesClientes detallesClientes = new DetallesClientes();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            ListCountry Pais = new ListCountry();
            filterInvoice PeopleId = new filterInvoice();
            PeopleId.Id = person.Id;
            person = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
            if (person.Error == null)
            {
                detallesClientes.Error = person.Error;
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                if (l.Error == null) HttpContext.Session.SetString("token", l.Token);
            }

            if (person.Addresses != null) Pais = await _globalService.ConsultaIdentificationsAndCitiesTF(new ParamCountry { Id = int.Parse(Country), Region = person.Addresses[0].Region });

            if (Pais.Identifications != null)
            {
                foreach (var iden in Pais.Identifications)
                {
                    if (person.Documents != null)
                    {
                        if (iden.Id == person.Documents[0].Identification && iden.Prefix == false)
                        {
                            detallesClientes.Document.Abbreviation = iden.Abbreviation;
                            detallesClientes.Document.Number = person.Documents[0].Number;
                        }
                        else if (iden.Id == person.Documents[0].Identification && iden.Prefix == true)
                        {
                            foreach (var prefx in iden.Prefixes)
                            {
                                if (prefx.Id == person.Documents[0].Prefix)
                                {
                                    detallesClientes.Document.Abbreviation = prefx.Abbreviation;
                                    detallesClientes.Document.Number = person.Documents[0].Number;
                                }
                            }
                        }
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
            }

            if (Pais.Regions != null)
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

            detallesClientes.Company = person.Company;
            if (person.Phones != null) { detallesClientes.Phone = person.Phones[0].Number; }
            else detallesClientes.Phone = null;
            if (person.Addresses != null)
            {
                detallesClientes.Address.Line1 = person.Addresses[0].Line1;
                detallesClientes.Address.Line2 = person.Addresses[0].Line2;
            }

            if (detallesClientes.Contacto.DocumentNumber == null || detallesClientes.Contacto.DocumentNumber == "") detallesClientes.Contacto = null;

            if (person.Discriminator == "PERSON")
            {
                detallesClientes.Company = person.FirstName;
                /*
                detallesClientes.Representante.Label = detallesClientes.Document.Abbreviation;
                detallesClientes.Representante.DocumentNumber = detallesClientes.Document.Number;
                detallesClientes.Representante.Name = person.FirstName;
                detallesClientes.Representante.PhoneNumber = person.Phones[0].Number;
                 */
                if (person.Emails != null) detallesClientes.Email = person.Emails[0].Address;
                detallesClientes.Representante = null;
                detallesClientes.Contacto = null;
            }

            return new JsonResult(detallesClientes);
        }
        public async Task<JsonResult> OnPostAsignarLimite([FromBody]AllocateQuota limiteCliente)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            IdUser = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");

            limiteCliente.Country = int.Parse(Country);
            limiteCliente.Confirmant = Confirmant;
            limiteCliente.User = IdUser;

            var respuesta = await _peopleService.MutacionLimiteCuentaAsync(limiteCliente, token);
            if (respuesta != "You are not authorised to perform this action")
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null) HttpContext.Session.SetString("token", l.Token);
            }
            return new JsonResult(respuesta);

        }
    }
}
