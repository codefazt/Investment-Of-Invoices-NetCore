using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ListDocuments
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public string Person { get; set; }
        [JsonProperty("Identification", NullValueHandling = NullValueHandling.Ignore)]
        public int identification { get; set; }
        [JsonProperty("Prefix", NullValueHandling = NullValueHandling.Ignore)]
        public object prefix { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("display_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Display_number { get; set; }
    }
}
