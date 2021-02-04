using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TuFactoringModels;

namespace TuFactoring.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        public int Country { get; set; }

        [BindProperty(Name = "error", SupportsGet = true)]
        public string mensajeError { get; set; }

        [BindProperty(Name = "exito", SupportsGet = true)]
        public string mensajeExito { get; set; }

        public IndexModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult OnGet()
        {
            var _country = Request.Cookies["Country"];
            if ( _country == null || _country.Trim().Equals("") )
            {
                _country = "862";
                HttpContext.Response.Cookies.Append("Country", _country);
            }
            Country = Convert.ToInt32(_country);

            var _particpant = Request.Cookies["Participant"];
            if (_particpant == null)
            {
                _particpant = "CONFIRMANT";
                HttpContext.Response.Cookies.Append("Participant", _particpant);
            }

            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("/Identity/Account/Logout");
            }

            var _param = Request.Cookies["Param"];

            if(_param != null)
            {
                var type = _param.Substring(0, 5);

                if (_param.Substring(0,5) == "error")
                {
                    mensajeError = _param.Substring(6, _param.Length - 6);
                }
                else if(_param.Substring(0, 5) == "exito")
                {
                    mensajeExito = _param.Substring(6, _param.Length -6);
                }

                HttpContext.Response.Cookies.Delete("Param");
            }


            return Page();
        }

        public async Task<IActionResult> OnGetSetCountryAsync()
        {
            string country = HttpContext.Request.Query["country"];
            HttpContext.Response.Cookies.Append("Country", country);
            var returnUrl = Request.Headers["Referer"].ToString();

            Country = Convert.ToInt32(country);
            ViewData["Country"] = country;
            return Redirect(returnUrl);
        }
    }
}
