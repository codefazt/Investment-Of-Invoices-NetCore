using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TuFactoring.CustomProviders;
using System.Security.Claims;
using TuFactoringGraphql;
using TuFactoringModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuFactoring.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using FluentValidation;
using TuFactoringModels.Validation;

namespace TuFactoring.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {

        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _authService;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, IGlobalService globalService, IPeopleService peopleService, IAuthService authService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _globalService = globalService;
            _authService = authService;
            _peopleService = peopleService;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public string CustomError { get; set; }
        public string MaskEdit { get; set; }
        public string RegexExpre { get; set; }

        public int Digits { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public string Discriminator { get; set; }
        public string Participant { get; set; }

        public Country Country { get; set; }
        public Identification Identification { get; set; }
        public List<SelectListItem> Options { get; set; }

        public class InputModel
        {
            public int Country { get; set; }
            public string Discriminator { get; set; }
            public string Participant { get; set; }
            public int Identification { get; set; }
            

            public int Prefix { get; set; }
            //[Remote(controller: "validation", action: "RegExpValidation", ErrorMessage = "Formato invalido", HttpMethod = "post")]
            
            public string TokenReCap { get; set; }

            [BindProperty]
            [Required]
            public string Number { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "requirePasswordFieldLogin")]
            [StringLength(12, ErrorMessage = "minimumLengthLoginPassword", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {

            var _country = Request.Cookies["Country"];
            var _discriminator = "";
            var _participant = "";

            int _id = Convert.ToInt32(HttpContext.Request.Query["id"]);

            if (_id < 1 || _id > 6) return RedirectToPage("../Index");

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
                if (HttpContext.Session.GetString("CountryLogin") == null || HttpContext.Session.GetString("CountryLogin") == "null")
                {
                    Country = await _globalService.GetDocumentByCountry(Convert.ToInt32(_country));
                    HttpContext.Session.SetString("CountryLogin", JsonConvert.SerializeObject(Country));
                }
                else Country = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryLogin"));
            }
            catch (Exception e) {
                HttpContext.Session.Remove("CountryLogin");
                return RedirectToPage("../Index");
            }

            if (Country == null)
            {
                ModelState.AddModelError("Number", "Datos Invalidos");
                HttpContext.Session.Remove("CountryLogin");
                return RedirectToPage("../Index");
            }
            if (Country.Id != Convert.ToInt32(_country))
            {
                Country = await _globalService.GetDocumentByCountry(Convert.ToInt32(_country));
                HttpContext.Session.SetString("CountryLogin", JsonConvert.SerializeObject(Country));
            }

            Discriminator = _discriminator;
            Participant = _participant;

            Fill_Inputs();
            
            if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var _country = Request.Cookies["Country"];

            var respuesta = await new CaptchaValidation().Validate(Input.TokenReCap);
            
            if (HttpContext.Session.GetString("CountryLogin") == null)
            {
                Country = await _globalService.GetDocumentByCountry(Convert.ToInt32(_country));
                HttpContext.Session.SetString("CountryLogin", JsonConvert.SerializeObject(Country));
            }
            else
            {
                Country = JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryLogin"));
            }

            Discriminator = Request.Cookies["Discriminator"];
            Participant = Request.Cookies["Participant"];

            Fill_Inputs();

            if (respuesta.Errors != null)
            {
                CustomError = "notConexionDetected";

                return Page();
            }

            if (!respuesta.Success || respuesta.Score < 0.5)
            {
                CustomError = "errorRecaptchaBot";

                return Page();
            }
            
            if (ModelState.IsValid && (Discriminator == "PERSON" || Discriminator == "LEGAL" && Regex.Match(Input.Number, this.RegexExpre).Success) )
            {
                
                var culture = System.Globalization.CultureInfo.CurrentCulture.Name;

                Input.Email = Input.Email.ToLower();
                var credentials = await _authService.Login(Input.Email, culture, Input.Country, Input.Discriminator, Input.Participant, Input.Identification,Input.Prefix, Input.Number);

                if (credentials.Error != null) {

                    if (credentials.Error == "User not password")
                    {
                        CustomError = "errorActivacionCuenta";
                    }
                    else
                    {
                        CustomError = "errorNumberOfDocument";
                    }
                    return Page();
                }
                else
                {
                    try
                    {
                        var result = await _signInManager.PasswordSignInAsync(credentials, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            HttpContext.Session.Remove("CurrencyCountry");
                            HttpContext.Session.Remove("CountryInvoices");
                            var area = credentials.Participant.ToLower();

                            HttpContext.Session.SetString("token", credentials.Token);

                            _logger.LogInformation("User logged in.");
                            return LocalRedirect("~/" + culture + "/" + area + "/");
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }

                        if (!result.Succeeded)
                        {
                            if (credentials.Error == "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond" || credentials.Error == "An error occurred while sending the request.")
                            {
                                CustomError = "errorConnectionLogin";
                            }
                            else
                            {
                                CustomError = "errorCredentialsInvalid";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CustomError = "errorCredentialsInvalid";
                    }

                    if(CustomError == "errorCredentialsInvalid" && Discriminator == "PERSON")
                    {
                        CustomError = "errorCredentialsInvalidPerson";
                    }
                }

            }
            else
            {
                CustomError = "errorCredentialsInvalid";
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    
        private void Fill_Inputs()
        {
            

            // lo de Country iba aqui
            
            Options = new List<SelectListItem>();

            if (Country != null)
            {
                foreach (var iden in Country.Identifications)
                {
                    if (iden.Discriminator == Discriminator && iden.Default == true && iden.Status)
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
}

