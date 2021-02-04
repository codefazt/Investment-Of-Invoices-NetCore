using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TuFactoring.Areas.Profile.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _aS;
        [BindProperty]
        public InputModel Input { get; set; }

        public List<string> errores { get; set; }

        public string exito { get; set; } = "";
        public string TipoParticipante { get; set; }

        public ChangePasswordModel(IAuthService aS, UserManager<User> userManager)
        {
            this._aS = aS;
            this._userManager = userManager;
        }
        
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "requirePasswordField")]
            [Display(Name = "newPassword")]
            [StringLength(12, ErrorMessage = "validatePasswordLength", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "requirePasswordField")]
            [Display(Name = "confirmPassword")]
            [Compare("Password", ErrorMessage = "validatePasswordMatch")]
            [StringLength(12, ErrorMessage = "validatePasswordLength", MinimumLength = 8)]
            public string ConfirmPassword { get; set; }


        }

        public async Task<IActionResult> OnGet()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            TipoParticipante = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();

            errores = new List<string>();
           
            Input = new InputModel();

            Input.Email = User.Claims.Where(x => x.Type == "Email").Select(x => x.Value).SingleOrDefault();

            #region RefreshToken
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, TipoParticipante, token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            #endregion

            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            TipoParticipante = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();

            errores = new List<string>();

            if (!ModelState.IsValid)
            {
                return Page();
            }


            if (Regex.Match(Input.Password, @"([\D])\1+\1|([\d])\2+\2").Success)
            {
                errores.Add("errorRepitCharacter");

            }

            if (Regex.Match(Input.Password, @"[ñÑ]{1,}").Success)
            {
                errores.Add("errorEnie");
            }

            if (errores.Count > 0)
            {
                return Page();
            }

            try
            {
                

                User user = new User()
                {
                    Id = new Guid(id),
                    Country = o,
                    Person = new Prospecto() { Id = c }
                };

                user.Email = Input.Email;
                user.UserName = Input.Email;
                user.AuthenticationType = token;
                
                var result = await _userManager.AddPasswordAsync(user, Input.Password);

                if (result != null && result.Errors.Count() == 0)
                {
                    exito = "changePassword";

                    return Page();
                }

                foreach (var error in result.Errors)
                {
                    errores.Add(error.Code);
                }
            }
            catch (Exception)
            {
                errores.Add("invalidUserChangePassword");
            }

            #region RefreshToken
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, TipoParticipante, token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            #endregion

            return Page();
        }





    }
}