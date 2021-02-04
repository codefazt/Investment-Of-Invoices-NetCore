using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class AcceptanceAgreements
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("participant")]
        public string Participant { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("person")]
        public string Person { get; set; }
    }
}
