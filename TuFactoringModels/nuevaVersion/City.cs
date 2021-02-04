using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class City
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("calling_code", NullValueHandling = NullValueHandling.Ignore)]
        public string Calling_code { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}
