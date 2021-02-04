using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class RequestPagination
    {
        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination Pagination { get; set; }

        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public filterInvoice Filter { get; set; }

        [JsonProperty("filterCliente", NullValueHandling = NullValueHandling.Ignore)]
        public FilterBackOffice FilterCliente { get; set; }
        
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }

        [JsonProperty("group", NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
}
