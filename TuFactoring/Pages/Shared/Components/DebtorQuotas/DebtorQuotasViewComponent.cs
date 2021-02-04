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

namespace TuFactoring.Pages.Shared.Components.DebtorQuotas
{
    public class DebtorQuotasViewComponent: ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly IPeopleService _peopleService;

        public class Modelo
        {
            public Country Country { get; set; }
            public Prospecto Person { get; set; }
        }
        
        public DebtorQuotasViewComponent(SignInManager<User> signInManager, UserManager<User> userManager, IAuthService authService, IPeopleService peopleService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
            _peopleService = peopleService;
        }
        public async Task<IViewComponentResult> InvokeAsync(Country country, string owner)
        {
            var token = HttpContext.Session.GetString("token");

            filterInvoice PeopleId = new filterInvoice();
            PeopleId.Id = owner;

            var countryID = country == null ? 0 : country.Id;

            var person = await _peopleService.GetDetailQuotas(new ParamProspecto { Filter = PeopleId, Country = (int)countryID }, token);

            var modelo = new Modelo();
            modelo.Country = country;
            modelo.Person = person;
            
            return View("Default", modelo);
        }
    }
}
