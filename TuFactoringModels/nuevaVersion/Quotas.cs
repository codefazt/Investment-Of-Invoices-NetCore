using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Quotas
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("entity")]
        public Entity Entity { get; set; }

        [JsonProperty("usage")]
        public double Usage { get; set; }

        [JsonProperty("available")]
        public double Available { get; set; }

        [JsonProperty("currency")]
        public int Currency { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }
}
