using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ResponseProveedores
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("pais", NullValueHandling = NullValueHandling.Ignore)]
        public int? Pais { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
    }
}
