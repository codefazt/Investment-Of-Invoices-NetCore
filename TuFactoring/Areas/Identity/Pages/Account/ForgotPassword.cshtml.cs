using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuFactoring.CustomProviders;
using TuFactoringModels;
using TuFactoring.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TuFactoring.Utilities;
using TuFactoringModels.Validation;

namespace TuFactoring.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _au;
        private readonly IGlobalService _globalService;
        private readonly Microsoft.AspNetCore.Identity.UI.Services.IEmailSender _emailSender;
        [BindProperty]
        public string CustomError { get; set; }
        public string MaskEdit { get; set; }
        public int Digits { get; set; }
        public string RegexExpre { get; set; }
        public List<SelectListItem> Options { get; set; }
        

        public ForgotPasswordModel(UserManager<User> userManager, Microsoft.AspNetCore.Identity.UI.Services.IEmailSender emailSender, IAuthService au, IGlobalService globalService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _au = au;
            _globalService = globalService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Country Country { get; set; }
        public Identification Identification { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public int Identification { get; set; }
            public int Prefix { get; set; }
            [Required]
            public string Number { get; set; }
            public string TokenReCap { get; set; }

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var _country = Request.Cookies["Country"];
            var _discriminator = "";
            var _participant = "";

            int _id = Convert.ToInt32(HttpContext.Request.Query["id"]);

            switch (_id)
            {
                case 1:
                    _discriminator = "LEGAL";
                    _participant = "DEBTOR";
                    break;
                case 2:
                    _discriminator = "LEGAL";
                    _participant = "SUPPLIER";
                    break;
                case 3:
                    _discriminator = "LEGAL";
                    _participant = "FACTOR";
                    break;
                case 4:
                    _discriminator = "PERSON";
                    _participant = "FACTOR";
                    break;
                case 5:
                    _discriminator = "LEGAL";
                    _participant = "CONFIRMANT";
                    break;
                case 6:
                    _discriminator = "LEGAL";
                    _participant = "BACKOFFICE";
                    break;
            }

            HttpContext.Response.Cookies.Append("Discriminator", _discriminator);
            HttpContext.Response.Cookies.Append("Participant", _participant);

            try
            {
                if (HttpContext.Session.GetString("CountryLogin") == null )
                {
                    Country = await _globalService.GetDocumentByCountry(Convert.ToInt32(_country));
                    HttpContext.Session.SetString("CountryLogin", JsonConvert.SerializeObject(Country));
                }
                else
                {
                    Country = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryLogin"));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            llenarOption(_discriminator);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var discriminator = Request.Cookies["Discriminator"];
            var respuesta = await new CaptchaValidation().Validate(Input.TokenReCap);

            llenarOption(discriminator);

            if (!respuesta.Success || respuesta.Score < 0.5)
            {
                CustomError = "errorRecaptchaBot";

                return Page();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await this._au.emailResetPassword(Input.Email, Input.Number, Input.Prefix, Request.Cookies["Participant"], Int32.Parse(Request.Cookies["Country"]), discriminator);   

                    if(result.Error != null)
                    {
                        
                        if (result.Error == "failed to perform event forgot from state forgotten")
                        {
                            CustomError = "forgotAlready";
                        }else if (discriminator == "LEGAL")
                        {
                            CustomError = "invalidDocumentOrEmail";

                        }
                        else
                        {
                            CustomError = "invalidEmail";

                        }

                        return Page();
                    }

                    return RedirectToPage("/logout", "Contraseña exitosa", new { returnUrl = "~/index?exito=forgotSend" });
                }
                catch (Exception)
                {
                    CustomError = "errorBaseDatos";
                    return Page();
                }
            }

            llenarOption(Request.Cookies["Discriminator"]);

            return Page();
        }

        private async void llenarOption(string _discriminator)
        {
            if(Country == null)
            {
                if (HttpContext.Session.GetString("CountryLogin") == null)
                {
                    Country = await _globalService.GetDocumentByCountry(Convert.ToInt32(Request.Cookies["Country"]));
                    HttpContext.Session.SetString("CountryLogin", JsonConvert.SerializeObject(Country));
                }
                else
                {
                    Country = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryLogin"));
                }
            }

            Options = new List<SelectListItem>();
            
            foreach (var iden in Country.Identifications)
            {
                if (iden.Discriminator == _discriminator && iden.Default == true && iden.Status)
                {
                    Identification = iden;
                    RegexExpre = iden.Regexp;
                    MaskEdit = iden.MaskEdit;

                    if (iden.Digits.HasValue)
                    {
                        Digits = iden.Digits.Value;
                    }
                    else
                    {
                        Digits = 0;
                    }

                    if (iden.Prefix == true)
                    {
                        foreach (var prefix in iden.Prefixes)
                        {
                            if (prefix.Status)
                            {
                                SelectListItem selListItem = new SelectListItem() { Value = prefix.Id.ToString(), Text = prefix.Abbreviation };

                                Options.Add(selListItem);
                            }
                        }
                    }
                }
            }
        }

    }
}
