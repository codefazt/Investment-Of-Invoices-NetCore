using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Invoice_types
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; }
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int Country_id { get; set; }
    }
}
