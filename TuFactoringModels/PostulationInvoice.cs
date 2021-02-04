using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class PostulationInvoice
    {
        [JsonProperty("invoice", NullValueHandling = NullValueHandling.Ignore)]
        public string Invoice { get; set; }
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public string Confirmant { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("request_financing", NullValueHandling = NullValueHandling.Ignore)]
        public bool Financiated { get; set; }
    }
}
