﻿using System;
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

namespace TuFactoring.Areas.Debtor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IInvoiceService _iS;
        private readonly IPeopleService _peopleService;
        private readonly IAuthService _aS;
        private readonly IGlobalService _globalService;
        private readonly SignInManager<User> _signInManager;
        [BindProperty]
        public string ContratoJson { get; set; }
        private string Participant { get; set; }
        public string Owner { get; set; }
        public Country Country { get; set; }
        public string country { get; set; }
        private string TerminoCondiciones { get; set; }
        private string ContratoProveedor { get; set; }


        public IndexModel(IInvoiceService iS, SignInManager<User> signInManager, IGlobalService globalService, IPeopleService peopleService, IAuthService aS)
        {
            _peopleService = peopleService;
            _globalService = globalService;
            _signInManager = signInManager;
            _iS = iS;
            this._aS = aS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout");

            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            User Contrato = new User();
            Contrato.Person = new Prospecto();

            if (Participant == "DEBTOR")
            {
                filterInvoice PeopleId = new filterInvoice();
                PeopleId.Id = Owner;
                var contratoTerminos = await _peopleService.ConsultaContratoAsync(new ParamProspecto { Filter = PeopleId, Country = int.Parse(country), Participant = Participant }, token);

                if (contratoTerminos != null && contratoTerminos.Agreements != null)
                {
                    foreach (var contrato in contratoTerminos.Agreements)
                    {
                        Contrato.Person.Agreements.Add(contrato);
                    }
                }
            }

            var data = await this._globalService.GetDataCountryInvoices(Int32.Parse(country), token);
            Country = data;

            Contrato.Participant = Participant;
            ContratoJson = JsonConvert.SerializeObject(Contrato);

            #region RefreshToken
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            #endregion
            return Page();
        }

        public async Task<JsonResult> OnPost([FromBody] AcceptanceAgreements contrato)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            //country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Owner = User.Claims.Where(x => x.Type == "Owner").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            contrato.Person = Owner;

            var respuesta = await _peopleService.MutacionContratoAsync(contrato, token);

            #region RefreshToken
            var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
            var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "DEBTOR", token);

            if (l.Error == null)
            {
                HttpContext.Session.SetString("token", l.Token);
            }
            #endregion

            return new JsonResult(respuesta);
        }
    }
}
