using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ParamConsultaPeyments
    {
        [JsonProperty("payer", NullValueHandling = NullValueHandling.Ignore)]
        public string Payer { get; set; }
        [JsonProperty("entity", NullValueHandling = NullValueHandling.Ignore)]
        public string Entity { get; set; }
        [JsonProperty("receiver", NullValueHandling = NullValueHandling.Ignore)]
        public string Receiver { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("stateInvoice", NullValueHandling = NullValueHandling.Ignore)]
        public string StateInvoice { get; set; }
        [JsonProperty("stateInvoiceNot", NullValueHandling = NullValueHandling.Ignore)]
        public string StateInvoiceNot { get; set; }
        [JsonProperty("consult", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Consult { get; set; }
    }
}
