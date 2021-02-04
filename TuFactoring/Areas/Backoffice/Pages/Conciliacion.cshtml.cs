using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TuFactoring.Services;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;
using Payments = TuFactoringModels.nuevaVersion.Payments;

namespace TuFactoring.Areas.Backoffice.Pages
{
    public class ConciliacionModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IGlobalService _globalService;
        private readonly IPeopleService _peopleService;
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _aS;
        public string verificarUsuarios { get; set; }
        private ListPayments VerficarPagos { get; set; } = new ListPayments();
        private string Participant { get; set; }
        private string Confirmant { get; set; }
        private string Country { get; set; }
        private ListCountry Countries { get; set; }
        public List<SelectListItem> Banks_Options { get; set; }
        public List<SelectListItem> Currency_Options { get; set; }
        [BindProperty]
        public List<filterInvoice> filter { get; set; }
        [BindProperty]
        public List<string> dataFilter { get; set; }
        [BindProperty]
        public string dataJsonCurrencies { get; set; }
        [BindProperty]
        public IFormFile file { get; set; }
        [BindProperty]
        public string IdBank { get; set; }
        [BindProperty]
        public string IdMoneda { get; set; }

        [BindProperty]
        public Datos Prueba { get; set; }
        public List<bool?> financiedNull { get; set; }
        private IHostingEnvironment _environment;

        public ConciliacionModel(IHostingEnvironment environment, SignInManager<User> signInManager, IPaymentService paymentService, IGlobalService globalService, IPeopleService peopleService, IAuthService aS)
        {
            _signInManager = signInManager;
            _globalService = globalService;
            _peopleService = peopleService;
            _paymentService = paymentService;
            _environment = environment;
            _aS = aS;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (token == null || token == "" || token == "null") return RedirectToPage("/logout");

            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Banks_Options = new List<SelectListItem>();
            Currency_Options = new List<SelectListItem>();
            Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = Int32.Parse(Country) });

            //----------- Llenado de Country
            if (Countries.Entities != null)
            {
                foreach (var bank in Countries.Entities)
                {
                    Banks_Options.Add(new SelectListItem() { Value = bank.Id, Text = bank.Person.Name });
                }
            }
            if (Countries.Currencies != null)
            {
                foreach (var moneda in Countries.Currencies)
                {
                    Currency_Options.Add(new SelectListItem() { Value = moneda.Id.ToString(), Text = moneda.Name });
                }
            }
            filter = new List<filterInvoice>();
            financiedNull = new List<bool?>();
            dataFilter = new List<string>();

            for (var i = 0; i < Countries.Currencies.Count; i++)
            {
                filter.Add(new filterInvoice()
                {
                    Financied = null
                });
                financiedNull.Add(null);
                dataFilter.Add(JsonConvert.SerializeObject(filter[i]));
            }

            //filter = new filterInvoice() { Financied = false };

            dataJsonCurrencies = JsonConvert.SerializeObject(Countries.Currencies);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {

            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Banks_Options = new List<SelectListItem>();
            Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = Int32.Parse(Country) });
            //----------- Llenado de Country
            if (Countries.Entities != null)
            {
                foreach (var bank in Countries.Entities)
                {
                    Banks_Options.Add(new SelectListItem() { Value = bank.Id, Text = bank.Person.Name });
                }
            }
            financiedNull = new List<bool?>();
            for (var i = 0; i < filter.Count; i++)
            {
                dataFilter.Add(JsonConvert.SerializeObject(filter[i]));
                financiedNull.Add(filter[i].Financied);

            }
            return Page();
        }

        public async Task<JsonResult> OnPostLlenarTabla([FromBody]ParamPayments pag)
        {
            if(pag == null) pag = new ParamPayments();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            List<Payments> ListPayment = new List<Payments>();
            var Id_Currency = pag.Currency;
            pag.Currency = null;
            pag.Country = int.Parse(Country);
            pag.State = "draft";
            var token = HttpContext.Session.GetString("token");
            var Estados = await _globalService.ConsultaIdentificationsAndCitiesTF(new ParamCountry { Id = Int32.Parse(Country) });
            VerficarPagos = await _paymentService.ConsultaConciliacionsAsync(pag, token);

            if (VerficarPagos.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }

            foreach(var pago in VerficarPagos.List)
            {
                if(pago.Currency != null)
                {
                    if(pago.Currency.Id == Id_Currency)
                    {
                        ListPayment.Add(pago);
                    }
                }
            }
            if (VerficarPagos != null) return new JsonResult(new { concilar = ListPayment, error = VerficarPagos.Error});
            else return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostConciliar([FromBody]Prospecto conciliar)
        {
            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var token = HttpContext.Session.GetString("token");
            var respuesta = await _paymentService.ConciliarUsuarioAsync(conciliar.Id, conciliar.Name, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta != null) return new JsonResult(new { Concilar = respuesta, accion = "conciliar", error = respuesta.Error });
            else return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostBloquear([FromBody]Prospecto conciliar)
        {

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var token = HttpContext.Session.GetString("token");
            var respuesta = await _paymentService.BloquearUsuarioAsync(conciliar.Id, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta != null) return new JsonResult(new { Concilar = respuesta, accion = "bloq", error = respuesta.Error });
            else return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostMovementReject([FromBody]Prospecto conciliar)
        {

            Participant = User.Claims.Where(x => x.Type == "Participant").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();

            var token = HttpContext.Session.GetString("token");
            var respuesta = await _paymentService.MovementAsync(conciliar.Id, token);
            if (respuesta.Error == null)
            {
                var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                if (l.Error == null)
                {
                    HttpContext.Session.SetString("token", l.Token);
                }
            }
            if (respuesta != null) return new JsonResult(new { Concilar = respuesta, accion = "movement", error = respuesta.Error });
            else return new JsonResult(null);
        }

        public async Task<JsonResult> OnPostArchivo()
        {
            string respuesta = null;
            string codigoBanco = "";
            string tipoMoneda = "";
            var fecha = DateTime.Today.ToString("yyyyMMdd");

            var token = HttpContext.Session.GetString("token");
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Countries = await _globalService.ConsultaBanksTF(new ParamCountry { Id = Int32.Parse(Country) });

            if (Countries != null)
            {
                foreach(var bank in Countries.Entities)
                {
                    if (bank.Id == IdBank) codigoBanco = bank.Routing_number;
                }

                foreach (var moneda in Countries.Currencies)
                {
                    if (moneda.Id.ToString() == IdMoneda) tipoMoneda = moneda.Iso_4217;
                }
            }

            var NameFile = codigoBanco  + "_" + fecha + "_" + tipoMoneda;
            //var dataFile = Server.MapPath("~/App_Data/data.txt");
            try
            {
                string pathToSave = Path.Combine(_environment.WebRootPath, "conciliados");
                Directory.CreateDirectory(pathToSave);

                var directorio = Directory.GetFiles(pathToSave).Length;
                var files = Path.Combine(pathToSave, NameFile + ".txt");
                var result = await _paymentService.GuardarArchivoAsync(new ParamConciliarArchivo { IdBank = IdBank, Url = files }, token);
                if (result != "You are not authorised to perform this action")
                {
                    var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                    var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                    if (l.Error == null)
                    {
                        HttpContext.Session.SetString("token", l.Token);
                    }
                }
                if (result == "successfull")
                {
                    using (var fileStream = new FileStream(files, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                respuesta = result;
            }
            catch
            {
                respuesta = "No se a Encontrado el directorio";
            }
            
            return new JsonResult(respuesta);
        }

        public async Task<JsonResult> OnPostMovements()
        {
            Country = User.Claims.Where(x => x.Type == "Country").Select(x => x.Value).SingleOrDefault();
            Confirmant = User.Claims.Where(x => x.Type == "Confirmant").Select(x => x.Value).SingleOrDefault();
            var token = HttpContext.Session.GetString("token");
            var movements = await _paymentService.ConsultaMovementsAsync(new ParamConciliarMovements() { Country=Int16.Parse(Country), State= "draft" }, token);

            if (movements != null)
            {
                if (movements.Error != "You are not authorised to perform this action")
                {
                    var id = User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).SingleOrDefault();
                    var l = await _aS.RefreshToken(id, CultureInfo.CurrentCulture.Name, Participant, token);

                    if (l.Error == null)
                    {
                        HttpContext.Session.SetString("token", l.Token);
                    }
                }

            }

            return new JsonResult(movements);
        }
    }

    public class Datos
    {
        public IFormFile Upload { get; set; }
        public string IdBank { get; set; }
    }
}
