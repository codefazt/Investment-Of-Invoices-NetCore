using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Reviews
    {
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("acceptance", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean Acceptance { get; set; }
        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
    }
}
