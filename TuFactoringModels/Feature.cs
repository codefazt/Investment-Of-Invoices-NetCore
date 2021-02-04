using Newtonsoft.Json;

namespace TuFactoringModels
{
    public class Feature
    {

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string ID { get; set; }

        [JsonProperty("program", NullValueHandling = NullValueHandling.Ignore)]
        public Program Program { get; set; }

        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviattion { get; set; }

        [JsonProperty("type_content", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeContent { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }
    }
}