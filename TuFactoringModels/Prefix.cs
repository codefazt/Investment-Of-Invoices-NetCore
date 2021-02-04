using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TuFactoringModels
{
    public class Prefix
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool Status { get; set; }
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }
    }
}
