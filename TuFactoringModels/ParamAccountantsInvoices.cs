using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ParamAccountantsInvoices
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int Country { get; set; }
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public string Confirmant { get; set; }
    }
}
