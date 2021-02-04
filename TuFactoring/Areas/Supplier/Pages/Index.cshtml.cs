using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TuFactoring.Services;
using Microsoft.AspNetCore.Identity;
using TuFactoringModels;
using Newtonsoft.Json;
using TuFactoringModels.nuevaVersion;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace TuFactoring.Areas.Supplier.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IInvoiceService _iS;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _aS;
        private readonly IGlobalService _globalService;
        private readonly IAuthorizationService _AuthorizationService;
        private readonly SignInManager<User> _signInManager;
        [BindProperty]
        public string ContratoJson { get; set; }
        private string Participant { get; set; }
        private string Owner { get; set; }
        private string Country { get; set; }
        private string TerminoCondiciones { get; set; }
        private string ContratoProveedor { get; set; }


        public IndexModel(IInvoiceService iS, SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IAuthService aS, IAuthorizationService AuthorizationService)
        {
            _peopleService = peopleService;
            _globalService = globalService;
            _signInManager = signInManager;
            _AuthorizationService = AuthorizationService;
            _iS = iS;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
         {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout");

            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            User Contrato = new User();
            Contrato.Person = new Prospecto();
            #region Supplier
            if (Participant == "SUPPLIER")
            {
                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                var contratoTerminos = await _peopleService.ConsultaContratoAsync(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                var Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = int.Parse(Country) });

                if (contratoTerminos != null)
                {
                    foreach (var contrato in contratoTerminos.Agreements)
                    {
                        if(Countries.Entities != null)
                        {
                            foreach (var banks in Countries.Entities)
                            {
                                if (contrato.Entity == banks.Id && contrato.AcceptedAt == null) Contrato.Person.Name = banks.Person.Name;
                            }
                            Contrato.Person.Agreements.Add(contrato);
                        }

                        if (contrato.Accepted == true && contrato.Abbreviation == "MEMBERSHIP" && contrato.Participant == "SUPPLIER") Contrato.State = "no_const";

                    }
                }
            }

            #endregion

            #region RefreshToken
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            #endregion

            Contrato.Participant = Participant;
            Contrato.IsAuthenticated = (await _AuthorizationService.AuthorizeAsync(User, "PolicyContracts")).Succeeded;
            ContratoJson = JsonConvert.SerializeObject(Contrato);
            return Page();
        }

        public async Task<JsonResult> OnPost([FromBody] AcceptanceAgreements contrato)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            contrato.Person = Owner;

            var respuesta = await _peopleService.MutacionContratoAsync(contrato, token);
            #region Respuesta
            if(respuesta != null)
            {
                Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
                Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
                Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
                User Contrato = new User();
                Contrato.Person = new Prospecto();

                if (Participant == "SUPPLIER")
                {
                    filterInvoice PeopleId = new filterInvoice();
                    PeopleId.Id = Owner;
                    var contratoTerminos = await _peopleService.ConsultaContratoAsync(new ParamProspecto { Filter = PeopleId, Country = int.Parse(Country) }, token);
                    var Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = int.Parse(Country) });

                    if (contratoTerminos != null)
                    {
                        foreach (var contr in contratoTerminos.Agreements)
                        {
                            if (Countries.Entities != null)
                            {
                                foreach (var banks in Countries.Entities)
                                {
                                    if (contrato.Entity == banks.Id && contr.AcceptedAt == null) Contrato.Person.Name = banks.Person.Name;
                                }
                                Contrato.Person.Agreements.Add(contr);
                            }
                            if (contrato.Accepted == true && contrato.Abbreviation == "MEMBERSHIP") Contrato.State = "no_const";
                        }
                    }

                }

                Contrato.Participant = Participant;
                return new JsonResult(Contrato);
            }

            #endregion

            #region RefreshToken
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "SUPPLIER", token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            #endregion


            return new JsonResult(respuesta);
        }
    }
}
