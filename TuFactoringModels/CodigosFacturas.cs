using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class CodigosFacturas
    {
        [JsonProperty("invoice_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Invoice_id { get; set; }
    }
}
