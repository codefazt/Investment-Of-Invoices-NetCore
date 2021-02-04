using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;

namespace TuFactoring.Areas.Backoffice.Pages
{
    public class ContabilidadModel : PageModel
    {
        #region Data

        private readonly IAuthService _aS;
        private readonly IGlobalService _gS;

        private readonly SignInManager<User> _signInManager;

        public readonly List<SelectListItem> _status_options;

        [BindProperty]
        public filterInvoice filter { get; set; }
        public string filterData { get; set; }

        #endregion

        public ContabilidadModel(IAuthService aS, SignInManager<User> signInManager, IGlobalService gS)
        {
            this._aS = aS;
            this._gS = gS;
            this._signInManager = signInManager;
            this._status_options = new List<SelectListItem>() {
                  new SelectListItem()
                  {
                      Value ="true",
                      Text = "true"
                  },
                  new SelectListItem()
                  {
                      Value ="false",
                      Text = "false"
                  },
            };

        }

        public async Task<IActionResult> OnGet()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            filter = new filterInvoice();

            this.filterData = JsonConvert.SerializeObject(filter);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            this.filterData = JsonConvert.SerializeObject(filter);

            return Page();
        }

        public async Task<JsonResult> OnPostGroups([FromBody]filterInvoice filter)
        {
            var token = HttpContext.Session.GetString("token");
            
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var groups = await this._aS.GetGroups(Int32.Parse(o), token,filter);
            
            return new JsonResult(groups);
        }

        public async Task<JsonResult> OnPostCountry()
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var country = await this._gS.ConsultaCountryPrograms(Int32.Parse(o), token);

            if (country == null || country.Count == 0)
            {
                return new JsonResult(country);
            }

            return new JsonResult(country[0]);
        }
        public async Task<IActionResult> OnPostCrear([FromBody]Group group)
        {
            var token = HttpContext.Session.GetString("token");
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            group.Country = new Country() { Id = Int32.Parse(o) };

            group = await this._aS.CreateGroup(group, token);

            return new JsonResult(group);
        }

        public async Task<IActionResult> OnPostActualizar([FromBody]Group group)
        {
            var token = HttpContext.Session.GetString("token");
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            group.Country = new Country() { Id = Int32.Parse(o) };

            group = await this._aS.UpdateGroup(group, token);

            return new JsonResult(group);
        }


    }
}