using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TuFactoring.CustomProviders;
using TuFactoring.Services;
using TuFactoringModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TuFactoringModels.Validation;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;

        private GlobalRegister registrarClienteTF { get; set; } = new GlobalRegister();

        [BindProperty]
        private Persons PeopleDesmascarados { get; set; } = null;
        [BindProperty]
        public string registroJson { get; set; }
        private string Participant { get; set; }
        private string Owner { get; set; }
        private string Country { get; set; }
        public string TipoParticipante { get; set; }
        public int Rol { get; set; }


        public RegisterModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService)
        {
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
        }

        public async Task<IActionResult> OnGetAsync(int idRegistro)
        {
            HttpContext.Session.SetInt32("idRegistro", idRegistro);
            registrarClienteTF.Registrarse = new Persons();
            registrarClienteTF.Rol = idRegistro;
            registrarClienteTF.User = new User();

            if (idRegistro > 0 && idRegistro < 5)
            {
                ViewData["Layout"] = "~/Pages/Shared/_RegisterLayout.cshtml";
                ViewData["IdRegistro"] = "";
                Rol = idRegistro;
            }
            else
            {
                Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
                Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
                registrarClienteTF.User.Participant = Participant;
                TipoParticipante = Participant;

                ViewData["Layout"] = "~/Pages/Shared/_Layout.cshtml";
                ViewData["IdRegistro"] = "appRegistroEmpresas";
                var token = HttpContext.Session.GetString("token");
                if (token == null || token == "" || token == "null") return RedirectToPage("/logout");
            }

            string pais = HttpContext.Session.GetString("pais");
            string paisCookie = Request.Cookies["Country"];

            try { 
                 if ((HttpContext.Session.GetString("CountryRegister") == null ||
                    HttpContext.Session.GetString("CountryRegister") == "" ||
                    HttpContext.Session.GetString("CountryRegister") == "null") ||
                    pais != paisCookie)
                {
                    HttpContext.Session.SetString("pais", Request.Cookies["Country"]);
                    registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryRegister", JsonConvert.SerializeObject(registrarClienteTF.DataPaises));
                }
            else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryRegister"));
            } catch (Exception e) { return RedirectToPage("../Index"); }
            //registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
            registroJson = JsonConvert.SerializeObject(registrarClienteTF);
            return Page();
        }

        public async Task<JsonResult> OnPost([FromBody]RequestRegister registro)
        {
            var token = HttpContext.Session.GetString("token");
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var respuesta = await new CaptchaValidation().Validate(registro.TokenReCaptcha);
            if (!respuesta.Success || respuesta.Score < 0.5)
            {
                return new JsonResult("errorCaptcha");
            }

            string ID = null;
            var registroGuardar = registro.Person;
            registroGuardar.Document.Id = null;
            if(registroGuardar.Email != null) registroGuardar.Email.Id = null;
            if (registroGuardar.Address != null) registroGuardar.Address.Id = null;
            if (registroGuardar.Phone != null) registroGuardar.Phone.Id = null;
            if (registroGuardar.Contacts != null)  foreach (var contacto in registroGuardar.Contacts) contacto.Id = null;

            if (HttpContext.Session.GetString("registroDesemascarado") != null &&
                HttpContext.Session.GetString("registroDesemascarado") != "null" &&
                HttpContext.Session.GetString("registroDesemascarado") != "")
            {
                var people = JsonConvert.DeserializeObject<Prospecto>(HttpContext.Session.GetString("People"));
                var data = JsonConvert.DeserializeObject<Persons>(HttpContext.Session.GetString("registroDesemascarado"));
                string participante = null;
                if(people != null)
                {
                    ID = people.Id;
                    if (people.Agreements != null)
                    {
                        foreach(var iden in people.Agreements)
                        {
                            if(registroGuardar.Participant == "BACKOFFICE" && iden.Participant == "CONFIRMANT")
                            {
                                participante = "bank";
                            }
                        }
                    }
                }
                if (participante == null)
                {
                    data.Participant = registroGuardar.Participant;
                    if (registroGuardar.Discriminator == "LEGAL") data.Email = null;
                    if (registroGuardar.Accepted_agreement == true) data.Accepted_agreement = true;
                    else { data.Accepted_agreement = false; }

                    if (registroGuardar.Participant == "BACKOFFICE")
                    {
                        data.Routing_number = registroGuardar.Routing_number;
                        data.Related = registroGuardar.Related;
                        foreach (var contacto in registroGuardar.Contacts)
                        {
                            if (contacto.Label == "ADMINISTRATOR") data.Contacts.Add(contacto);
                        }
                    }
                    data.Accounts = new List<TuFactoringModels.nuevaVersion.Account>();
                    data.Suppliers = new List<Associate>();
                    data.Customers = new List<Associate>();
                    registroGuardar = data;
                }
            }
            registroGuardar.Country = int.Parse(Request.Cookies["Country"]);
            int? id = 0;

            if (registroGuardar.Participant != "BACKOFFICE" && registroGuardar.Participant != "CONFIRMANT")
            {
                id = HttpContext.Session.GetInt32("idRegistro");
            }

            registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryRegister"));
            if (registroGuardar.Accounts != null)
            {
                if (registrarClienteTF.DataPaises != null)
                {
                    if(registrarClienteTF.DataPaises.Currencies != null)
                    {
                        foreach(var moneda in registrarClienteTF.DataPaises.Currencies)
                        {
                            int principal = 0;
                            if (registroGuardar.Accounts.Count > 0)
                            {
                                foreach (var cuenta in registroGuardar.Accounts)
                                {
                                    if(moneda.Id == cuenta.Currency && principal == 0)
                                    {
                                        principal = 1;
                                        cuenta.Default = true;
                                    }
                                    cuenta.Status = true;
                                }
                            }
                        }
                    }
                }
                
            }

            if (registroGuardar.Address != null) registroGuardar.Address.Label = "LEGAL";
            RegistroValidation validator = new RegistroValidation("nada");
            var result = validator.Validate(registroGuardar);

            if (result.IsValid)
            {
                Prospecto actualizado = new Prospecto();
                registroGuardar.Address.Country = int.Parse(Request.Cookies["Country"]);
                if(registroGuardar.Phone != null) registroGuardar.Phone.Country = int.Parse(Request.Cookies["Country"]);

                string registrado = null;
                if (id==1 || registroGuardar.Participant == "CONFIRMANT") registrado = await _peopleService.RegisterDebtorTF(registroGuardar);
                if (id == 2) registrado = await _peopleService.RegisterSupplierTF(registroGuardar);
                if (id == 3 || id == 4) registrado = await _peopleService.RegisterFactorTF(registroGuardar);
                if (registroGuardar.Participant == "BACKOFFICE" && ID == null) registrado = await _peopleService.RegisterConfirmantTF(registroGuardar, token);
                if (registroGuardar.Participant == "BACKOFFICE" && ID != null)
                {
                    actualizado = await _peopleService.UpdateProfileTF(UpdateBank(registroGuardar, ID, "CONFIRMANT"), token);
                    if (actualizado.Error == null) registrado = "success: ";
                    else registrado = actualizado.Error;
                }
                if (registrado.Contains("success:")) HttpContext.Session.SetString("registroDesemascarado", "");

                return new JsonResult(registrado);
            }
            else
            {
                return new JsonResult(new { errorValidacion = result.Errors });
            }
        }

        public async Task<JsonResult> OnPostVerificarDoc([FromBody]Persons comprobarDoc)
        {
            Prospecto regsitro = null;
            Persons PeopleRegisted = new Persons();
            HttpContext.Session.SetString("registroDesemascarado", "");
            HttpContext.Session.SetString("registroDesemascaradoError", "");
            var respuesta = await _peopleService.RegisterByDocument(new ParamProspecto {
                Document = new DocumentIdentification {
                    Country = Int32.Parse(Request.Cookies["Country"]),
                    Identification = comprobarDoc.Document.Identification,
                    Prefix = comprobarDoc.Document.Prefix,
                    Number = comprobarDoc.Document.Number,
                }
            });

            try
            {
                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = respuesta.Person;
                regsitro = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = Int32.Parse(Request.Cookies["Country"]) });

                if (regsitro.Documents != null && regsitro.Documents.Count > 0) PeopleRegisted.Document = regsitro.Documents[0];
                if (regsitro.Addresses != null && regsitro.Addresses.Count > 0) PeopleRegisted.Address = regsitro.Addresses[0];
                if (regsitro.Phones != null && regsitro.Phones.Count > 0) PeopleRegisted.Phone = regsitro.Phones[0];
                if (regsitro.Emails != null && regsitro.Emails.Count > 0) PeopleRegisted.Email = regsitro.Emails[0]; ;
                PeopleRegisted.Discriminator = regsitro.Discriminator;
                PeopleRegisted.Company = regsitro.Company;
                PeopleRegisted.FirstName = regsitro.FirstName;
                PeopleRegisted.LastName = regsitro.LastName;
                PeopleRegisted.Category = regsitro.Category;
                PeopleRegisted.Country = regsitro.Country;
                PeopleRegisted.Contacts = regsitro.Contacts;
                if(regsitro.Entities != null) PeopleRegisted.Routing_number = regsitro.Entities[0].Routing_number;
                if (regsitro.Accounts == null) PeopleRegisted.Accounts = new List<TuFactoringModels.nuevaVersion.Account>();
                else
                {
                    foreach (var cuenta in regsitro.Accounts)
                    {
                        TuFactoringModels.nuevaVersion.Account newAccount = new TuFactoringModels.nuevaVersion.Account();
                        newAccount.Entity = cuenta.Entity.Id;
                        newAccount.AccountNumber = cuenta.AccountNumber;
                        newAccount.AccountType = cuenta.AccountType;
                        newAccount.Currency = cuenta.Currency;

                        PeopleRegisted.Accounts.Add(newAccount);
                    }
                }

                HttpContext.Session.SetString("People", JsonConvert.SerializeObject(regsitro));
                HttpContext.Session.SetString("registroDesemascarado", JsonConvert.SerializeObject(PeopleRegisted));
                HttpContext.Session.SetString("registroDesemascaradoError", JsonConvert.SerializeObject(PeopleRegisted));
                PeopleRegisted.Contacts = new List<TuFactoringModels.nuevaVersion.Contact>();
                //HttpContext.Session.SetString("registroDesemascarado", JsonConvert.SerializeObject(PeopleRegisted));
                return new JsonResult(new { registro = PeopleRegisted, contacts = regsitro.Contacts, contratos = regsitro.Agreements });

            }
            catch { return null; }
        }

        public async Task<JsonResult> OnPostVerificarDocActualizar([FromBody]Persons comprobarDocA)
        {
            Prospecto regsitro = null;
            Associate contact = new Associate();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.RegisterByDocument(new ParamProspecto
            {
                Document = new DocumentIdentification
                {
                    Country = Int32.Parse(Request.Cookies["Country"]),
                    Identification = comprobarDocA.Document.Identification,
                    Prefix = comprobarDocA.Document.Prefix,
                    Number = comprobarDocA.Document.Number,
                }
            });

            try
            {
                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = respuesta.Person;
                regsitro = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = Int32.Parse(Request.Cookies["Country"]) });
                contact.Person = regsitro.Id;
                contact.Company = regsitro.Company;
                if (regsitro.Documents != null)
                {
                    contact.Prefix = regsitro.Documents[0].Prefix;
                    contact.Identification = regsitro.Documents[0].Identification;
                    contact.Number = regsitro.Documents[0].Number;
                }
                if(regsitro.Contacts != null)
                {
                    foreach(var contacto in regsitro.Contacts)
                    {
                        if (contacto.Label == "LEGAL")
                        {
                            contact.Name = contacto.Name;
                            contact.Email = contacto.Email;
                            contact.PhoneNumber = contacto.PhoneNumber;                            
                        }
                    }

                }

                if(regsitro.Agreements != null)
                {
                    foreach(var iden in regsitro.Agreements)
                    {
                        //if (Participant == iden.Participant) contact.State = "igual";
                    }
                }
                return new JsonResult( contact );

            }
            catch { return null; }
        }

        public async Task<JsonResult> OnPostSelectCity([FromBody]GlobalRegister Region)
        {

            ListCountry country = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = Region.State });
            return new JsonResult(country.Regions[0].Cities);

        }

        public async Task<JsonResult> OnPostVerificarEmail([FromBody]Persons usuario)
        {
            bool EmailExistent = false;
            string error = null;
            try
            {
                var respuesta = await _peopleService.ConsultaEmails(usuario.Email.Address, int.Parse(Request.Cookies["Country"]));
                if (respuesta != null)
                {
                    if ((respuesta.Id != null && respuesta.Name != null) && (respuesta.Id != "" && respuesta.Name != "")) EmailExistent = true;
                    if (respuesta.Id == null && respuesta.Name == "record not found") error = respuesta.Name;

                }
            }
            catch
            {

            }
            return new JsonResult(new { emailExist = EmailExistent, errors = error });
        }

        public async Task<JsonResult> OnPostAccount([FromBody]TuFactoringModels.nuevaVersion.Account param)
        {

            var respuesta = await _peopleService.ConsultaAccount(param.Id, param.AccountNumber);

            try
            {
                
                return new JsonResult(respuesta);

            }
            catch { return null; }
        }

        public JsonResult OnPostRefresh()
        {
            var id = HttpContext.Session.GetInt32("idRegistro");
            var pais = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryRegister"));
            var mascara = HttpContext.Session.GetString("registroDesemascarado");
            return new JsonResult("Refres");
        }
        private Profiles UpdateBank(Persons registroGuardar, string id, string participant)
        {
            Profiles PerfilUser = new Profiles();
            PerfilUser.Id = id;
            PerfilUser.Country = int.Parse(Country);
            PerfilUser.Company = registroGuardar.Company;
            PerfilUser.Addresses.Add(registroGuardar.Address);
            PerfilUser.Contacts = registroGuardar.Contacts;
            PerfilUser.Discriminator = registroGuardar.Discriminator;
            //PerfilUser.Documents.Add(registroGuardar.Document);
            if (registroGuardar.Email != null) PerfilUser.Emails.Add(registroGuardar.Email);
            PerfilUser.FirstName = registroGuardar.FirstName;
            PerfilUser.LastName = registroGuardar.LastName;
            PerfilUser.Participant = participant;
            PerfilUser.Phones.Add(registroGuardar.Phone);
            PerfilUser.Category = registroGuardar.Category;
            if (registroGuardar.Accounts != null) PerfilUser.Accounts = registroGuardar.Accounts;

            return PerfilUser;
        }
    }
}
