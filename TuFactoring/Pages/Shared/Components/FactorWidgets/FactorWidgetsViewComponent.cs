using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoring.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Pages.Shared.Components.FactorWidgets
{
    public class FactorWidgetsViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly IPeopleService _peopleService;

        public FactorWidgetsViewComponent(SignInManager<User> signInManager, UserManager<User> userManager, IAuthService authService, IPeopleService peopleService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
            _peopleService = peopleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var token = HttpContext.Session.GetString("token");

            var dashboard = await _peopleService.GetDashboard("general", 0, token);

            return View("Default", dashboard);
        }
    }
}
