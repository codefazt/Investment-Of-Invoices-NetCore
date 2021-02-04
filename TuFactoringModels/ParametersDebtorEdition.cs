using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ParametersDebtorEdition
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public filterInvoice Filter { get; set; }
        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination Pagination { get; set; }
    }
}

