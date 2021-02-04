using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Profile.Pages
{
    public class UsuariosModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _authService;
        private string Participant { get; set; }
        private string IdUser { get; set; }
        private string Country { get; set; }
        private string Discriminator { get; set; }
        private string Owner { get; set; }
        private RegistroGlobal registrarCliente { get; set; } = new RegistroGlobal();
        private GlobalActualizar registrarClienteTF { get; set; } = new GlobalActualizar();
        [BindProperty]
        public int NRol { get; set; }
        [BindProperty]
        public string UserJson { get; set; }
        public string TipoParticipante { get; set; }

        public UsuariosModel(SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IAuthService authService)
        {
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _authService = authService;
        }
        public async Task<IActionResult> OnGet()
        {
            IdUser = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = Participant;
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null" || Owner == null || IdUser == null) return RedirectToPage("/logout");

            if (Participant == "DEBTOR") { NRol = 1; }
            else if (Participant == "SUPPLIER") { NRol = 2; }
            else if (Participant == "FACTOR") { NRol = 3; }
            else if (Participant == "CONFIRMANT") { NRol = 5; }
            else if (Participant == "BACKOFFICE") { NRol = 6; }

            registrarClienteTF.User = new User();
            registrarClienteTF.User = await _authService.GetProfileUserById(IdUser, token);
            UserJson = JsonConvert.SerializeObject(registrarClienteTF.User);
            return Page();
        }
        public async Task<JsonResult> OnPost([FromBody]User user)
        {
            IdUser = User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            TipoParticipante = Participant;
            user.Id  = new Guid(IdUser);

            var users = await this._authService.GetStaff(Owner, Participant, token);

            for (var i = 0; i < users.Count; i++)
            {
                if (users[i].Id.ToString() != user.Id.ToString() && users[i].Email == user.Email)
                {
                    return new JsonResult(new User() { Error = "duplicatedEmail" });
                }
            }
            var UserActualizar = await _authService.UpdateUser(user, token);
            return new JsonResult(UserActualizar);

        }
    }
}