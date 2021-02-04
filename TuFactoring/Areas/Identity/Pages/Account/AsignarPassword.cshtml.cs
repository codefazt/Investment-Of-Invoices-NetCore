using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;
using System.Text.RegularExpressions;

namespace TuFactoring.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class AsignarPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _au;

        public AsignarPasswordModel(UserManager<User> userManager, IAuthService au)
        {
            _userManager = userManager;
            _au = au;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<string> errores { get; set; }

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
            public string ConfirmPassword { get; set; }

            public string Token { get; set; }

        }

        public async Task<IActionResult> OnGet(string token = null)
        {
            errores = new List<string>();
            
            if (token == null)
            {
                return RedirectToPage("/logout", "Expired Token", new { returnUrl = "~/index?error=expiredToken" });
            }
            else
            {

                var user = await this._au.ValidateTokenPassword(token);

                if (!String.IsNullOrEmpty(user.Error))
                {
                    return RedirectToPage("/logout", "Expired Token", new { returnUrl = "~/index?error=expiredToken" });
                }


                Input = new InputModel
                {
                    Email = user.Email,
                    Token = token
                };
                
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            errores = new List<string>();
            try
            {

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var user = await this._au.ValidateTokenPassword(Input.Token);
                
                user.UserName = user.Email;
                user.Token = Input.Token;

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

                var result = await _userManager.AddPasswordAsync(user, Input.Password);
                
                if (result != null && result.Errors.Count() == 0)
                {
                    return RedirectToPage("/logout", "Contraseña exitosa", new { returnUrl = "~/index?exito=passwordAssignLogin" });
                }

                foreach (var error in result.Errors)
                {
                    errores.Add(error.Code);
                }
            }
            catch (Exception){
                errores.Add("invalidUserResetPassword");
            }
          
            return Page();
        }
        

    }
}
