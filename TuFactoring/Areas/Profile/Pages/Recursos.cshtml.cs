using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuFactoring.Services;

namespace TuFactoring.Areas.Profile.Pages
{
    public class RecursosModel : PageModel
    {
        private readonly IAuthService _aS;

        public RecursosModel(IAuthService aS)
        {
            this._aS = aS;
        }
        public void OnGet()
        {
            
        }
        
        public async Task<JsonResult> OnPostPing()
        {
            var token = HttpContext.Session.GetString("token");
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var p = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            var w = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, p, token, w);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
                return new JsonResult(new { msg = true});
            }

            return new JsonResult(new { msg = false });
        }


        public async Task<JsonResult> OnPostToken()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return new JsonResult(new { token = "" });
            }

            return new JsonResult(new { token = token });
        }
    }
}