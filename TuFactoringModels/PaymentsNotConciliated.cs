using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class PaymentsNotConciliated
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Movements> List { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

    }
}
