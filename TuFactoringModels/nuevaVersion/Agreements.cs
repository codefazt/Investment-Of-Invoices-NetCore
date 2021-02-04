using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Agreements
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("acceptedAt")]
        public string AcceptedAt { get; set; }

        [JsonProperty("participant")]
        public string Participant { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

    }
}
