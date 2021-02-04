using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TuFactoringModels;
using TuFactoring.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Newtonsoft.Json;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Backoffice.Pages
{
    public class MantenimientoBancosModel : PageModel
    {
        #region Data

        private readonly IAuthService _aS;

        private readonly IGlobalService _gS;

        private readonly IPeopleService _pS;

        private readonly SignInManager<User> _signInManager;
        #endregion

        public MantenimientoBancosModel(IAuthService aS, IGlobalService gS, SignInManager<User> signInManager, IPeopleService pS)
        {
            this._aS = aS;
            this._gS = gS;
            this._pS = pS;
            this._signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEntities()
        {
            var token = HttpContext.Session.GetString("token");

            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var entity = await this._gS.GetCountryEntities(o,token);

            return new JsonResult(entity);
        }

        public async Task<JsonResult> OnPostCatalogo()
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            if (HttpContext.Session.GetString("CountryInvoices") == null || JsonConvert.DeserializeObject<Country>(HttpContext.Session.GetString("CountryInvoices")).Id != Int32.Parse(o))
            {
                HttpContext.Session.SetString("CountryInvoices", JsonConvert.SerializeObject(await this._gS.ConsultasCountryTF(new ParamCountry { Id = Int32.Parse(o)})));
            }

            return new JsonResult(HttpContext.Session.GetString("CountryInvoices"));
        }

        public async Task<JsonResult> OnPostGetDataFromEntiy([FromBody] Entity Bank)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            filterInvoice Entity = new filterInvoice();

            Entity.Id = Bank.Person.Id;

            var respuesta = await _pS.RegisterById(new ParamProspecto { Filter = Entity, Country = Int32.Parse(o) });

            if(respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);
                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(respuesta);
        }

        public async Task<JsonResult> OnPostMakeAllied([FromBody] TuFactoringModels.nuevaVersion.Entity alliedBank)
        {
            var token = HttpContext.Session.GetString("token");
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());

            var bank = await this._pS.MakeEntityAllied(alliedBank,o,token);
            if (bank.Error == null || bank.Errors == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);
                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            return new JsonResult(bank);
        }

        public async Task<JsonResult> OnPostUpdateLegalRepresentant([FromBody] Contact Contactos)
        {
            var token = HttpContext.Session.GetString("token");
            var o = Int32.Parse(User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault());
            //Instancia Persons
            Persons updateLegal = new Persons();
            //Instancia Profiles
            Profiles newLegalRepresentative = new Profiles();
            //Valores a las Propiedades de Persons
            updateLegal.Id = Contactos.Id;
            updateLegal.Discriminator = "LEGAL";
            updateLegal.Participant = "CONFIRMANT";
            updateLegal.Contacts.Add(Contactos);
            //Valores a las Propiedades que serán enviadas al Consumer.
            newLegalRepresentative.Id = updateLegal.Id;
            newLegalRepresentative.Country = o;
            newLegalRepresentative.Discriminator = "LEGAL";
            newLegalRepresentative.Participant = "CONFIRMANT";
            newLegalRepresentative.Contacts = updateLegal.Contacts;

            var response = await _pS.UpdateProfileTF(newLegalRepresentative, token);

            if(response.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);
                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(response);
        }
    }
}
