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

namespace TuFactoring.Areas.Admin.Pages
{
    public class MantenimientoRolesModel : PageModel
    {
        #region Data
        
        private readonly IAuthService _aS;

        private readonly SignInManager<User> _signInManager;
        #endregion

        public MantenimientoRolesModel(IAuthService aS, SignInManager<User> signInManager)
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

            return Page();
        }

        public async Task<IActionResult> OnPostDiscriminator()
        {
            List<string> participant = new List<string>() { "DEBTOR", "SUPPLIER", "CONFIRMANT", "FACTOR", "BACKOFFICE" };

            return new JsonResult(participant);
        }


        public async Task<JsonResult> OnPostRoles()
        {
            var token = HttpContext.Session.GetString("token");


            List<Role> roles = new List<Role>();
            var data = await this._aS.GetRoles("BACKOFFICE",token);

            roles.AddRange(data);

             data = await this._aS.GetRoles("DEBTOR", token);

            roles.AddRange(data);
            
             data = await this._aS.GetRoles("SUPPLIER", token);

            roles.AddRange(data);

            data = await this._aS.GetRoles("FACTOR", token);

            roles.AddRange(data);

            data = await this._aS.GetRoles("CONFIRMANT", token);

            roles.AddRange(data);

            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }

            return new JsonResult(roles);
        }

        public async Task<IActionResult> OnPostCrear([FromBody]Role rol)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._aS.CreateRole(rol,token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<IActionResult> OnPostActualizar([FromBody]Role rol)
        {
            var token = HttpContext.Session.GetString("token");

            var data = await this._aS.UpdateRole(rol, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostBloquear([FromBody]Role rol)
        {
            if (!rol.Id.HasValue)
                return new JsonResult(new Response() { Error = "Not ID" });

            var token = HttpContext.Session.GetString("token");

            var data = await this._aS.BlockRole(rol.Id.Value, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

    }
}