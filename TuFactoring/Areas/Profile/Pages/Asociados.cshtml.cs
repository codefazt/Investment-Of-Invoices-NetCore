using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;
using Account = TuFactoringModels.nuevaVersion.Account;

namespace TuFactoring.Areas.Profile.Pages
{
    public class AsociadosModel : PageModel
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

        public AsociadosModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IAuthService aS, IAuthorizationService AuthorizationService)
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
            TipoParticipante = Participant;
            if (Participant == "DEBTOR") { NRol = 1; }
            else if (Participant == "SUPPLIER") { NRol = 2; }

            registrarClienteTF.User = new User();
            registrarClienteTF.Registrarse = new PesonProfile();
            registrarClienteTF.User.Participant = Participant;
            registrarClienteTF.User.Discriminator = Discriminator;

            if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
            {
                registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
            }
            else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

            filterInvoice PeopleId = new filterInvoice();
            PeopleId.Id = Owner;
            registrarClienteTF.Perfil = await _peopleService.ConsultaAsociados(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
            if (registrarClienteTF.Perfil.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            RellenarPerfil();
            registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
            registroJson = JsonConvert.SerializeObject(registrarClienteTF);
            return Page();
        }
        public async Task<JsonResult> OnPost([FromBody]Persons registroGuardar)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Discriminator = User.Claims.Where(x => x.Type == "Discriminator").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");

            registrarClienteTF.User = new User();
            registrarClienteTF.Registrarse = new PesonProfile();
            registrarClienteTF.User.Participant = Participant;
            registrarClienteTF.User.Discriminator = Discriminator;
            Profiles PerfilUser = new Profiles();

            PerfilUser.Id = Owner;
            PerfilUser.Country = int.Parse(Country);
            PerfilUser.Company = registroGuardar.Company;
            PerfilUser.FirstName = registroGuardar.FirstName;
            PerfilUser.LastName = registroGuardar.LastName;
            PerfilUser.Participant = Participant;
            PerfilUser.Discriminator = Discriminator;
            PerfilUser.Category = registroGuardar.Category;
            PerfilUser.Addresses = null;
            PerfilUser.Contacts = null;
            PerfilUser.Documents = null;
            PerfilUser.Emails = null;
            PerfilUser.Phones = null;
            PerfilUser.Accounts = null;
            if (Participant == "SUPPLIER") PerfilUser.Customers = registroGuardar.Customers;
            if (Participant == "DEBTOR") PerfilUser.Suppliers = registroGuardar.Suppliers;

            registrarClienteTF.Perfil = await _peopleService.UpdateAsociadoTF(PerfilUser, token);
            if (registrarClienteTF.Perfil.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (registrarClienteTF.Perfil != null)
            {
                if (HttpContext.Session.GetString("CountryPerfil") == null || HttpContext.Session.GetString("CountryPerfil") == "null" || HttpContext.Session.GetString("CountryPerfil") == "")
                {
                    registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            else return new JsonResult(null);

        }
        // Invitaciones
        public async Task<JsonResult> OnPostAcceptInvitation([FromBody]ParamGuests Invitado)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.AcceptInvitation(Invitado, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta.Error == null || respuesta.Error == "")
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
                    registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.ConsultaAsociados(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(new { error = respuesta.Error });
        }
        public async Task<JsonResult> OnPostToggleInvitation([FromBody]ParamGuests Invitado)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.ToggleInvitation(Invitado, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta.Error == null || respuesta.Error == "")
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
                    registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.ConsultaAsociados(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult( new { error = respuesta.Error });
        }
        public async Task<JsonResult> OnPostCancelInvitation([FromBody]ParamGuests Invitado)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.CancelInvitation(Invitado, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta.Error == null || respuesta.Error == "")
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
                    registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.ConsultaAsociados(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(new { error = respuesta.Error });
        }
        public async Task<JsonResult> OnPostRejectInvitation([FromBody]ParamGuests Invitado)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.RejectInvitation(Invitado, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta.Error == null || respuesta.Error == "")
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
                    registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.ConsultaAsociados(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(new { error = respuesta.Error });
        }
        public async Task<JsonResult> OnPostSendInvitation([FromBody]ParamGuests Invitado)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Invitado.Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Invitado.Person = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var respuesta = await _peopleService.SendInvitation(Invitado, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token, Confirmant);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta.Error == null || respuesta.Error == "")
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
                    registrarClienteTF.DataPaises = await _globalService.ConsultasAsociadosTF(new ParamCountry { Id = Int32.Parse(Request.Cookies["Country"]) });
                    HttpContext.Session.SetString("CountryPerfil", JsonConvert.SerializeObject(registrarCliente.DataPaises));
                }
                else registrarClienteTF.DataPaises = JsonConvert.DeserializeObject<ListCountry>(HttpContext.Session.GetString("CountryPerfil"));

                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                registrarClienteTF.Perfil = await _peopleService.ConsultaAsociados(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) });
                RellenarPerfil();
                registrarClienteTF.AuthRol = (await _AuthorizationService.AuthorizeAsync(User, "PolicyProfileChange")).Succeeded;
                return new JsonResult(registrarClienteTF);
            }
            return new JsonResult(new { error = respuesta.Error });
        }

        private void RellenarPerfil()
        {
            if (registrarClienteTF.Perfil.Documents != null && registrarClienteTF.Perfil.Documents.Count > 0) registrarClienteTF.Registrarse.Document = registrarClienteTF.Perfil.Documents[0];
            registrarClienteTF.Registrarse.Discriminator = registrarClienteTF.Perfil.Discriminator;
            registrarClienteTF.Registrarse.Company = registrarClienteTF.Perfil.Company;
            registrarClienteTF.Registrarse.FirstName = registrarClienteTF.Perfil.FirstName;
            registrarClienteTF.Registrarse.Category = registrarClienteTF.Perfil.Category;
            registrarClienteTF.Registrarse.Country = registrarClienteTF.Perfil.Country;

            if (registrarClienteTF.Perfil.Customers != null && Participant == "SUPPLIER")
            {
                foreach (var asociado in registrarClienteTF.Perfil.Customers)
                {
                    AssociateConsulta associate = new AssociateConsulta()
                    {
                        Id = asociado.Id,
                        Company = asociado.Company,
                        Name = asociado.Name,
                        Email = asociado.Email,
                        Identification = asociado.Identification,
                        Prefix = asociado.Prefix,
                        Number = asociado.Number,
                        InvitedAt = asociado.InvitedAt,
                        Invited = asociado.Invited,
                        State = asociado.State,
                        PhoneNumber = asociado.Phone_number,
                        Phone_number = asociado.Phone_number
                    };
                    if (asociado.Person != null) associate.Person = asociado.Person;
                    registrarClienteTF.Registrarse.Customers.Add(associate);
                }
            }
            else registrarClienteTF.Registrarse.Customers = new List<AssociateConsulta>();

            if (registrarClienteTF.Perfil.Suppliers != null && Participant == "DEBTOR")
            {
                foreach (var asociado in registrarClienteTF.Perfil.Suppliers)
                {
                    AssociateConsulta associate = new AssociateConsulta()
                    {
                        Id = asociado.Id,
                        Company = asociado.Company,
                        Name = asociado.Name,
                        Email = asociado.Email,
                        Identification = asociado.Identification,
                        Prefix = asociado.Prefix,
                        Number = asociado.Number,
                        InvitedAt = asociado.InvitedAt,
                        Invited = asociado.Invited,
                        State = asociado.State,
                        PhoneNumber = asociado.Phone_number,
                        Phone_number = asociado.Phone_number
                    };
                    if (asociado.Person != null) associate.Person = asociado.Person;
                    registrarClienteTF.Registrarse.Suppliers.Add(associate);
                }
            }
            else registrarClienteTF.Registrarse.Suppliers = new List<AssociateConsulta>();
        }

    }
}