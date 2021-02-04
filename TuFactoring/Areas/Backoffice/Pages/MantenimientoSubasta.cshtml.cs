using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace TuFactoring.Areas.Backoffice.Pages
{
    public class MantenimientoSubastaModel : PageModel
    {
        #region Data

        private readonly IAuctionService _aS;

        private readonly IAuthService _authS;

        private readonly SignInManager<User> _signInManager;

        private readonly string __notAuthorized = "You are not authorised to perform this action";
        #endregion

        public MantenimientoSubastaModel(IAuctionService aS, SignInManager<User> signInManager, IAuthService authS)
        {
            this._aS = aS;
            this._signInManager = signInManager;
            this._authS = authS;
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

        public async Task<IActionResult> OnPostCurrent()
        {
            var token = HttpContext.Session.GetString("token");
            
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            
            var data = await this._aS.GetAuction(Int32.Parse(o),token);

            if (data.Count == 0 || data[0].Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data[0]);
        }

        public async Task<JsonResult> OnPostCreate()
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            Auctions data = await this._aS.CreateAuction(o,token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostOpen([FromBody]Response r)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            
            Auctions data = await this._aS.OpenAuction(r.Id, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostClose([FromBody]Response r)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            
            Auctions data = await this._aS.CloseAuctionAsync(r.Id, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostEnding([FromBody]Response r)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            var data = await this._aS.EndingAuction(r.Id, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            return new JsonResult(data);
        }

        public async Task<JsonResult> OnPostConciliation([FromBody]Response r)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var dataAuction = await this._aS.ConciliationAuction(r.Id, token);

            var dataJsonAuction = (string)JsonConvert.SerializeObject(dataAuction);

            return new JsonResult(dataJsonAuction);
        }

        public async Task<JsonResult> OnPostPayments([FromBody]Response r)
        {
            var token = HttpContext.Session.GetString("token");

            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var data = await this._aS.PaymentsAuction(r.Id, token);

            if (data.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await this._authS.RefreshToken(id, CultureInfo.CurrentCulture.Name, "BACKOFFICE", token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            var dataJsonAuction = (string)JsonConvert.SerializeObject(data);

            return new JsonResult(dataJsonAuction);
        }
    }
}