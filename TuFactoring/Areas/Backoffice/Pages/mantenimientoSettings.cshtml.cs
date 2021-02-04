using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuFactoring.Services;
using TuFactoringModels;

namespace TuFactoring.Areas.Backoffice.Pages
{
    public class mantenimientoSettingsModel : PageModel
    {
        #region Data

        private readonly IGlobalService _gS;
        private readonly IAuthService _aS;

        private readonly SignInManager<User> _signInManager;
        #endregion

        public mantenimientoSettingsModel(IGlobalService gS, SignInManager<User> signInManager, IAuthService aS)
        {
            this._gS = gS;
            this._signInManager = signInManager;
            this._aS = aS;
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

        public async Task<IActionResult> OnPostTypesContent()
        {
            List<string> discriminators = new List<string>() { "NUMBER","AMOUNT","DATE","STRING" };

            return new JsonResult(discriminators);
        }

        public async Task<JsonResult> OnPostSettings()
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            List<Setting> setting = new List<Setting>();
           
            var data = await this._gS.GetSettings(Int32.Parse(o),token);

            if (data.Count ==0 || data[0].Error == null)
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

        public async Task<IActionResult> OnPostCrear([FromBody]Setting setting)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            setting.Country = Int32.Parse(o);
            var data = await this._gS.CreateSetting(setting,token);

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

        public async Task<IActionResult> OnPostActualizar([FromBody]Setting setting)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            setting.Country = Int32.Parse(o);
            setting.Status = null;
            var data = await this._gS.UpdateSetting(setting, token);

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