using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TuFactoring.CustomProviders;
using TuFactoringModels;

namespace TuFactoring.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string culture, string returnUrl = null)
        {
            try
            {
                var _country = Request.Cookies["Country"];

                returnUrl = returnUrl ?? Url.Content("~/");
                await _signInManager.SignOutAsync();

                if (returnUrl != null)
                {
                    var param = returnUrl.Split("?");

                    returnUrl = param[0];

                    if (param.Length > 1)
                    {
                        HttpContext.Response.Cookies.Append("Param", param[1]);
                    }
                }

                HttpContext.Session.Remove("CurrencyCountry");
                HttpContext.Session.Remove("CountryInvoices");
                HttpContext.Session.Remove("CountryLogin");
                HttpContext.Session.Remove("CountryPerfil");
                HttpContext.Session.Remove("token");
                HttpContext.Session.Remove("PaisesBanks");

                _logger.LogInformation("User logged out.");
                if (returnUrl != null)
                {

                    return LocalRedirect(returnUrl + culture);
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception e)
            {
                return LocalRedirect("/" + culture);
            }

        }

        public async Task<IActionResult> OnPost(string culture, string returnUrl = null)
        {
            try
            {
                var _country = Request.Cookies["Country"];

                returnUrl = returnUrl ?? Url.Content("~/");
                await _signInManager.SignOutAsync();

                if (returnUrl != null)
                {
                    var param = returnUrl.Split("?");

                    returnUrl = param[0];

                    if (param.Length > 1)
                    {
                        HttpContext.Response.Cookies.Append("Param", param[1]);
                    }
                }

                HttpContext.Session.Remove("CurrencyCountry");
                HttpContext.Session.Remove("CountryInvoices");
                HttpContext.Session.Remove("CountryLogin");
                HttpContext.Session.Remove("CountryPerfil");
                HttpContext.Session.Remove("token");
                HttpContext.Session.Remove("PaisesBanks");

                _logger.LogInformation("User logged out.");
                if (returnUrl != null)
                {

                    return LocalRedirect(returnUrl + culture);
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception e)
            {
                return LocalRedirect("/"+culture);
            }
        }
    }
}