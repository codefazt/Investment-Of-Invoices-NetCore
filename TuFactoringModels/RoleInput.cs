using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class RoleInput
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("discriminator", NullValueHandling = NullValueHandling.Ignore)]
        public string Discriminator { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
    }
}
