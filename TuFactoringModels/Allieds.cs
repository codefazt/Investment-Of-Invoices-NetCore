using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Allieds
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("routing_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Routing_number { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
        [JsonProperty("createAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreateAt { get; set; }
        [JsonProperty("updateAt", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateAt { get; set; }

    }
}
