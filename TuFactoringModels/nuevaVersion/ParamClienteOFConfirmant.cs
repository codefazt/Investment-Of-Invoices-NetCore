using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ParamClienteOFConfirmant
    {
        [JsonProperty("bank_id", NullValueHandling = NullValueHandling.Ignore)]
        public string Bank_id { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public filterInvoice Filter { get; set; }
        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination Pagination { get; set; }
    }
}
