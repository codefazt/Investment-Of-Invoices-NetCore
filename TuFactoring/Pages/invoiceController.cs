using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoring.Services;

namespace TuFactoring.Pages
{
    public class invoiceController : Controller
    {
        private readonly IGlobalService _gs;

        public invoiceController(IGlobalService gs)
        {
            _gs = gs;
        }

        [HttpPost]
        public async Task<JsonResult> invoiceNumber()
        {
            //if (Regex.Match(Request.Form["filter.Number"], "^A{1}[0-9]{5}\\-{1}[0-9]{8}$").Success) return new JsonResult(true);

            return new JsonResult(false);
        }

        [HttpPost]
        public async Task<JsonResult> expirationFrom()
        {
            return new JsonResult(false);
        }

        [HttpPost]
        public async Task<JsonResult> expirationTo()
        {
            return new JsonResult(false);
        }
    }
}