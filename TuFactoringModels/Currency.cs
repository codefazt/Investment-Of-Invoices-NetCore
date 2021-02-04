using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Currency
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("iso_4217", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso_4217 { get; set; }
        [JsonProperty("symbol", NullValueHandling = NullValueHandling.Ignore)]
        public string Symbol { get; set; }
        [JsonProperty("digits", NullValueHandling = NullValueHandling.Ignore)]
        public int? Digits { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updateAt", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }
    }
}
