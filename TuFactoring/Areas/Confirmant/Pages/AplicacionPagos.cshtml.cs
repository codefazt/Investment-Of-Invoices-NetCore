using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoring.Areas.Confirmant.Pages
{
    public class AplicacionPagosModel : PageModel
    {
        private Pagos pays { get; set; } = new Pagos();
        public string JsonPagosOverdue { get; set; }
        public string JsonPagosReconciled { get; set; }

        private readonly IPeopleService _peopleService;
        private readonly IGlobalService _globalService;
        private readonly IPaymentService _paymentService;
        private readonly SignInManager<User> _signInManager;
        private string Participant { get; set; }
        private string Owner { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }

        private readonly string __notAuthorized = "You are not authorised to perform this action";


        public AplicacionPagosModel(SignInManager<User> signInManager, IPeopleService peopleService, IGlobalService globalService, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _globalService = globalService;
            _peopleService = peopleService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");

            if (token == null)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var c = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var dataOverdue = await this._paymentService.GetAplicationPays(o, c,token,"OVERDUE");

            if (dataOverdue.Count > 0 && dataOverdue[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            var dataReconciled = await this._paymentService.GetAplicationPays(o, c, token, "RECONCILED");

            if (dataReconciled.Count > 0 && dataReconciled[0].Errors == this.__notAuthorized)
            {
                return RedirectToPage("/logout", "session expired", new { returnUrl = "~/index?error=sessionExpired" });
            }

            JsonPagosOverdue = JsonConvert.SerializeObject(dataOverdue);
            JsonPagosReconciled = JsonConvert.SerializeObject(dataReconciled);
            return Page();
        }

        public async Task<JsonResult> OnPostPay([FromBody]Payment pago)
        {
            var token = HttpContext.Session.GetString("token");
            var c = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var o = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var buttonPay = await _paymentService.MutationPayReceipt(pago,token);
            return new JsonResult(buttonPay);
        }
    }
}
