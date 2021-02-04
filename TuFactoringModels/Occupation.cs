using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Occupation
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updatedAt", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }
    }
}
