using Newtonsoft.Json;

namespace TuFactoringModels
{
    public class Details
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string ID { get; set; }

        [JsonProperty("group", NullValueHandling = NullValueHandling.Ignore)]
        public string Group { get; set; }

        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string participant { get; set; }

        [JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; }

        [JsonProperty("concept", NullValueHandling = NullValueHandling.Ignore)]
        public string Concept { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }
    }
}