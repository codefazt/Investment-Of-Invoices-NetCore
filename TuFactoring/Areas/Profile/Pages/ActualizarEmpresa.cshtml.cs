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
using Microsoft.AspNetCore.Authorization;
using Account = TuFactoringModels.nuevaVersion.Account;
using System.Globalization;

namespace TuFactoring.Areas.Profile.Pages
{
    public class ActualizarEmpresaModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _aS;
        private readonly IAuthorizationService _AuthorizationService;
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }
        private string Discriminator { get; set; }
        private string Owner { get; set; }
        private RegistroGlobal registrarCliente { get; set; } = new RegistroGlobal();
        private GlobalActualizar registrarClienteTF { get; set; } = new GlobalActualizar();
        [BindProperty]
        public int NRol { get; set; }
        [BindProperty]
        public string registroJson { get; set; }
        public string TipoParticipante { get; set; }

        public ActualizarEmpresaModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IAuthService aS, IAuthorizationService AuthorizationService)
        {
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _aS = aS;
            _AuthorizationService = AuthorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            if (token == null || token == "" || token == "null" || Owner == null) return RedirectToPage("/logout");
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
            var Auth = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
            var AuthRol = User.IsInRole("LEGAL");
            TipoParticipante = Participant;
            switch (Participant)
            {
                case "CONFIRMANT":
                    NRol = 5; break;
                case "DEBTOR":
                    NRol = 1; break;
                case "FACTOR":
                    NRol = 3; break;
                case "SUPPLIER":
                    NRol = 2; break;
                case "BACKOFFICE":
                    NRol = 6; break;
            }

            registrarClienteTF.User = new User();
            registrarClienteTF.Registrarse = new PesonProfile();
            registrarClienteTF.User.Participant = Participant;
            registrarClienteTF.User.Discriminator = Discriminator;

            if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
            {
                registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
            }
            else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

            filterInvoice PeopleId = new filterInvoice();
            PeopleId.Id = Owner;
            registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Participant = Participant, Filter = PeopleId, Country = int.Parse(Country) }, token);
            if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = registrarClienteTF.Perfil.Addresses[0].Region });

            if (registrarClienteTF.Perfil != null && registrarClienteTF.Perfil.Contacts != null && registrarClienteTF.Perfil.Contacts.Count > 0)
            {
                foreach (var legal in registrarClienteTF.Perfil.Contacts)
                {
                    if (legal.Label == "LEGAL") HttpContext.Session.SetString("RepresentanteLegal", JsonConvert.SerializeObject(legal));
                }
            }
            RellenarPerfil();

            registrarClienteTF.AuthRol = Auth;
            registrarClienteTF.ContratAuth = (await _AuthorizationService.AuthorizeAsync(User, "PolicyContracts")).Succeeded;
            registroJson = JsonConvert.SerializeObject(registrarClienteTF);
            return Page();
        }

        public async Task<JsonResult> OnPost([FromBody]Persons registroGuardar)
        {

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
            var Auth = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
            string Err = null;
            var token = HttpContext.Session.GetString("token");

            registrarClienteTF.User = new User();
            registrarClienteTF.Registrarse = new PesonProfile();
            registrarClienteTF.User.Participant = Participant;
            registrarClienteTF.User.Discriminator = Discriminator;

            Profiles PerfilUser = new Profiles();
            registroGuardar.Address.Country = int.Parse(Country);
            registroGuardar.Address.Label = "LEGAL";
            if (registroGuardar.Phone != null) registroGuardar.Phone.Country = int.Parse(Country);

            PerfilUser.Id = Owner;
            PerfilUser.Country = int.Parse(Country);
            //PerfilUser.Company = registroGuardar.Company;
            PerfilUser.Addresses.Add(registroGuardar.Address);
            if (registroGuardar.Contacts != null)
            {
                foreach (var Contacto in registroGuardar.Contacts)
                {
                    if (Contacto.Label != "LEGAL") PerfilUser.Contacts.Add(Contacto);
                    else
                    {
                        Contact Representante = JsonConvert.DeserializeObject<Contact>(HttpContext.Session.GetString("RepresentanteLegal"));
                        Representante.PhoneNumber = Contacto.PhoneNumber;
                        if (Representante != null && Representante.Id != null && Representante.Id != "") PerfilUser.Contacts.Add(Representante);
                    }
                }
            }
            //PerfilUser.Contacts = registroGuardar.Contacts;
            PerfilUser.Discriminator = registroGuardar.Discriminator;
            //PerfilUser.Documents.Add(registroGuardar.Document);
            if (registroGuardar.Email != null) PerfilUser.Emails.Add(registroGuardar.Email);
            //PerfilUser.FirstName = registroGuardar.FirstName;
            //PerfilUser.LastName = registroGuardar.LastName;
            PerfilUser.Participant = registroGuardar.Participant;
            //PerfilUser.Phones.Add(registroGuardar.Phone);
            //PerfilUser.Category = registroGuardar.Category;
            if (registroGuardar.Accounts != null) PerfilUser.Accounts = registroGuardar.Accounts;
            registrarClienteTF.Perfil = await _peopleService.UpdateProfileTF(PerfilUser, token);
            if (registrarClienteTF.Perfil.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (registrarClienteTF.Perfil.Error != null)
            {
                Err = registrarClienteTF.Perfil.Error;
                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Participant = Participant, Filter = PeopleId, Country = int.Parse(Country) }, token);

            }
            if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
            {
                registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = int.Parse(Country) });
                HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
            }
            else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));
            if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = int.Parse(Country), Region = registrarClienteTF.Perfil.Addresses[0].Region });
            RellenarPerfil();

            registrarClienteTF.AuthRol = Auth;
            registrarClienteTF.ContratAuth = (await _AuthorizationService.AuthorizeAsync(User, "PolicyContracts")).Succeeded;
            return new JsonResult(new { error = Err, person = registrarClienteTF });

        }

        public async Task<JsonResult> OnPostSelectCity([FromBody]GlobalRegister Region)
        {
            ListCountry country = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = Region.State });
            return new JsonResult(country.Regions[0].Cities);

        }


        // Invitaciones
        public async Task<JsonResult> OnPostAcceptInvitation([FromBody]ParamGuests Invitado)
        {
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.AcceptInvitation(Invitado, token);

            if (respuesta != null)
            {
                Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
                Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
                Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
                Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
                TipoParticipante = Participant;

                registrarClienteTF.User = new User();
                registrarClienteTF.Registrarse = new PesonProfile();
                registrarClienteTF.User.Participant = Participant;
                registrarClienteTF.User.Discriminator = Discriminator;

                if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
                {
                    registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = registrarClienteTF.Perfil.Addresses[0].Region });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(null);
        }
        public async Task<JsonResult> OnPostToggleInvitation([FromBody]ParamGuests Invitado)
        {
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.ToggleInvitation(Invitado, token);

            if (respuesta != null)
            {
                Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
                Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
                Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
                Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
                TipoParticipante = Participant;

                registrarClienteTF.User = new User();
                registrarClienteTF.Registrarse = new PesonProfile();
                registrarClienteTF.User.Participant = Participant;
                registrarClienteTF.User.Discriminator = Discriminator;

                if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
                {
                    registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = registrarClienteTF.Perfil.Addresses[0].Region });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostCancelInvitation([FromBody]ParamGuests Invitado)
        {
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.CancelInvitation(Invitado, token);

            if (respuesta != null)
            {
                Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
                Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
                Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
                Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
                TipoParticipante = Participant;

                registrarClienteTF.User = new User();
                registrarClienteTF.Registrarse = new PesonProfile();
                registrarClienteTF.User.Participant = Participant;
                registrarClienteTF.User.Discriminator = Discriminator;

                if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
                {
                    registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = registrarClienteTF.Perfil.Addresses[0].Region });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostRejectInvitation([FromBody]ParamGuests Invitado)
        {
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.RejectInvitation(Invitado, token);

            if (respuesta != null)
            {
                Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
                Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
                Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
                Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
                TipoParticipante = Participant;

                registrarClienteTF.User = new User();
                registrarClienteTF.Registrarse = new PesonProfile();
                registrarClienteTF.User.Participant = Participant;
                registrarClienteTF.User.Discriminator = Discriminator;

                if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
                {
                    registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = registrarClienteTF.Perfil.Addresses[0].Region });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(null);
        }

        //Contratos
        public async Task<JsonResult> OnPostContrato([FromBody] AcceptanceAgreements agreementsContratoMarco)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            agreementsContratoMarco.Person = Owner;

            var respuesta = await _peopleService.MutacionContratoAsync(agreementsContratoMarco, token);

            if (respuesta != null)
            {
                TipoParticipante = Participant;
                registrarClienteTF.User = new User();
                registrarClienteTF.Registrarse = new PesonProfile();
                registrarClienteTF.User.Participant = Participant;
                registrarClienteTF.User.Discriminator = Discriminator;

                if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
                {
                    registrarClienteTF.DataPaises = await _globalService.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.RegisterById(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                if (registrarClienteTF.Perfil.Addresses != null) registrarClienteTF.Cities = await _globalService.ConsultaCitiesTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]), Region = registrarClienteTF.Perfil.Addresses[0].Region });
                RellenarPerfil();

                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                registrarClienteTF.ContratAuth = (await _AuthorizationService.AuthorizeAsync(User, "PolicyContracts")).Succeeded;
                return new JsonResult(registrarClienteTF);

            }
            else { return new JsonResult(null); }
        }

        private void RellenarPerfil()
        {
            if (registrarClienteTF.Perfil.Documents != null && registrarClienteTF.Perfil.Documents.Count > 0) registrarClienteTF.Registrarse.Document = registrarClienteTF.Perfil.Documents[0];
            if (registrarClienteTF.Perfil.Addresses != null && registrarClienteTF.Perfil.Addresses.Count > 0) registrarClienteTF.Registrarse.Address = registrarClienteTF.Perfil.Addresses[0];
            if (registrarClienteTF.Perfil.Phones != null && registrarClienteTF.Perfil.Phones.Count > 0) registrarClienteTF.Registrarse.Phone = registrarClienteTF.Perfil.Phones[0];
            if (registrarClienteTF.Perfil.Emails != null && registrarClienteTF.Perfil.Emails.Count > 0) registrarClienteTF.Registrarse.Email = registrarClienteTF.Perfil.Emails[0]; ;
            registrarClienteTF.Registrarse.Discriminator = registrarClienteTF.Perfil.Discriminator;
            registrarClienteTF.Registrarse.Company = registrarClienteTF.Perfil.Company;
            registrarClienteTF.Registrarse.FirstName = registrarClienteTF.Perfil.FirstName;
            registrarClienteTF.Registrarse.LastName = registrarClienteTF.Perfil.LastName;
            registrarClienteTF.Registrarse.Category = registrarClienteTF.Perfil.Category;
            registrarClienteTF.Registrarse.Country = registrarClienteTF.Perfil.Country;
            if (registrarClienteTF.Perfil.Accounts == null) registrarClienteTF.Registrarse.Accounts = new List<Account>();
            else
            {
                foreach (var cuenta in registrarClienteTF.Perfil.Accounts)
                {
                    Account newAccount = new Account();
                    newAccount.Id = cuenta.Id;
                    newAccount.Entity = cuenta.Entity.Id;
                    newAccount.AccountNumber = cuenta.AccountNumber;
                    newAccount.AccountType = cuenta.AccountType;
                    newAccount.Currency = cuenta.Currency;
                    newAccount.Default = cuenta.Default;
                    newAccount.Status = cuenta.Status;

                    registrarClienteTF.Registrarse.Accounts.Add(newAccount);
                }
            }
        }
    }
}
