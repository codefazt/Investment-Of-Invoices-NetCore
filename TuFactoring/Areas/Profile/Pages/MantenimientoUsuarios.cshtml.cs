using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuFactoringModels;
using TuFactoring.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace TuFactoring.Areas.Profile.Pages
{
    public class MantenimientoUsuariosModel : PageModel
    {
        #region data
        private readonly IAuthService _aS;
        public string Participant { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";

        private readonly SignInManager<User> _signInManager;
        #endregion

        public MantenimientoUsuariosModel(IAuthService aS, SignInManager<User> signInManager)
        {
            this._aS = aS;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }
            
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();

            return Page();
        }

        public async Task<JsonResult> OnPostRoles()
        {
            var token = HttpContext.Session.GetString("token");
            
            var p = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            var roles = await this._aS.GetRoles(p,token);

            return new JsonResult(roles);
        }

        public async Task<JsonResult> OnPostUsers()
        {
            var token = HttpContext.Session.GetString("token");
            
            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var p = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            var data = await this._aS.GetStaff(c, p,token);


            #region RefreshToken
            if(data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, p, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            #endregion

            return new JsonResult(data);
        }

       
        public async Task<IActionResult> OnPostBloquear([FromBody]User usuario)
        {
            var token = HttpContext.Session.GetString("token");
            var data = await this._aS.BlockUser(usuario,token);
            var p = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();

            #region RefreshToken
            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, p, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            #endregion

            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostCrear([FromBody]User usuario)
        {
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            var p = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();

            usuario.CountryId = o;
            usuario.OwnerId = c;
            usuario.Participant = p;
            
            var users = await this._aS.GetStaff(c, p, token);

            for(var i=0; i < users.Count; i++)
            {
                if(users[i].Email == usuario.Email)
                {
                    return new JsonResult(new User() { Error = "duplicatedEmail" });
                }
            }

            usuario = await this._aS.InviteUser(usuario,token);

            #region RefreshToken
            if (usuario.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, p, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            #endregion

            return new JsonResult(usuario);
        }

        public async Task<IActionResult> OnPostActualizar([FromBody]User usuario)
        {
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var p = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();

            var users = await this._aS.GetStaff(c, p, token);

            for (var i = 0; i < users.Count; i++)
            {
                if (users[i].Id.ToString() != usuario.Id.ToString() && users[i].Email == usuario.Email)
                {
                    return new JsonResult(new User() { Error = "duplicatedEmail" });
                }
            }


            var data = await this._aS.UpdateInviteUser(usuario,token);

            #region RefreshToken
            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, p, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            #endregion
            return new JsonResult(data);
        }

        
    }
}