using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;

namespace TuFactoring.Areas.Identity.Pages
{
    public class validationController : Controller
    {
        public Country Country { get; set; }
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _authService;

        public validationController(SignInManager<User> signInManager, ILogger<LoginModel> logger, IGlobalService globalService, IPeopleService peopleService, IAuthService authService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _globalService = globalService;
            _authService = authService;
            _peopleService = peopleService;
        }

        [HttpPost]
        public async Task<JsonResult> RegExpValidation()
        {
            var input = Request.Form["Input.Number"];

            var _country = Request.Cookies["Country"];
            string regexExpre = "";

            if (HttpContext.Session.GetString("CountryLogin") == null)
            {
                Country = await _globalService.GetDocumentByCountry(Convert.ToInt32(_country));
                HttpContext.Session.SetString("CountryLogin", JsonConvert.SerializeObject(Country));
            }
            else Country = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryLogin"));

            foreach (var iden in Country.Identifications)
            {
                if (iden.Default.Value && iden.Discriminator == "LEGAL") regexExpre = iden.Regexp;
            }

            if (!Regex.Match(input, regexExpre).Success) return new JsonResult(false);
            else return new JsonResult(true);

        }

        [HttpPost]
        public async Task<JsonResult> ValidacionRegistros()
        {




            return new JsonResult(false);
        }
    }
}