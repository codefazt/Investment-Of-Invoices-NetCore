using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Region
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int Country { get; set; }
        [JsonProperty("cities")]
        public List<City> Cities { get; set; }
    }
}
