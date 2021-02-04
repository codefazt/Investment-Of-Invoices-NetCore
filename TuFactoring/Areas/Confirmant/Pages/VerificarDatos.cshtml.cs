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
    public class VerificarDatosModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _aS;
        public string verificarUsuarios { get; set; }
        private Verification prospectoValidar { get; set; } = new Verification();
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }

        //Variables de la pagina
        [BindProperty]
        public string listadosInicialesJson { get; set; }
        [BindProperty]
        public string filtroParticipant { get; set; }
        [BindProperty]
        public filterInvoice filter { get; set; }
        [BindProperty]
        public string dataFilter { get; set; }
        ListCountry Identifications;
        public bool? financiedNull { get; set; }
        public List<SelectListItem> Status_Options { get; set; }
        public List<SelectListItem> State_Options { get; set; }
        public List<SelectListItem> City_Options { get; set; }
        public List<SelectListItem> Discriminator_options { get; set; }
        public List<SelectListItem> Participant_options { get; set; }

        public VerificarDatosModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IAuthService aS)
        {
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _aS = aS;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });

            filter = new filterInvoice() { Financied = false };
            dataFilter = JsonConvert.SerializeObject(filter);
            financiedNull = null;
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();

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
                new SelectListItem() { Value = "FACTOR", Text = "INVERSIONISTAS" }
            };

            //Para llenar El filtro de Estados
            State_Options = new List<SelectListItem>();
            foreach (var state in Identifications.Regions)
            {
                State_Options.Add(new SelectListItem() { Value = state.id.ToString(), Text = state.Name });
            }
            State_Options.Sort((a, b) => a.Text.CompareTo(b.Text));
            listadosInicialesJson = JsonConvert.SerializeObject(Identifications);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            //if (filter.Participant != null) filtroParticipant = filter.Participant;
            dataFilter = JsonConvert.SerializeObject(filter);
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
            State_Options.Sort((a, b) => a.Text.CompareTo(b.Text));
            return Page();
        }

        public async Task<JsonResult> OnPostLlenarTabla([FromBody]RequestPagination pag)
        {
            var token = HttpContext.Session.GetString("token");
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            prospectoValidar = await _peopleService.ConsultaDatosParaVerificarAsync(Confirmant, pag.Filter, pag.Pagination, token);

            if(prospectoValidar.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null) HttpContext.Session.SetString("token", l.Token);
            }
            

            if (prospectoValidar != null) prospectoValidar.List.Sort((b, a) => a.CreatedAt.Date.CompareTo(b.CreatedAt.Date));
            var Estados = await _globalService.ConsultaEstatesTF(new ParamCountry { Id = Int32.Parse(Country) });
            return new JsonResult(new { prospecto = prospectoValidar, estados = Estados });
        }

        public async Task<JsonResult> OnPostDetalleCliente([FromBody]Prospecto person)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");

            DetallesClientes detallesClientes = new DetallesClientes();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
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

            if (person.Addresses != null) Pais = await _globalService.ConsultaIdentificationsAndCitiesTF(new ParamCountry { Id = int.Parse(Country), Region = person.Addresses[0].Region});
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
                if(person.Emails != null) detallesClientes.Email = person.Emails[0].Address;
                detallesClientes.Representante = null;
                detallesClientes.Contacto = null;
            }

            return new JsonResult(detallesClientes);
        }

        public async Task<JsonResult> OnPostVerificarProspecto([FromBody]ApproveVerification approveVerification)
        {
            string idVerifecar = approveVerification.Id;
            approveVerification.Id = null;
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");

            var respuesta = await _peopleService.MutacionDatosParaVerificarAsync(idVerifecar, approveVerification, token);
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
