using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TuFactoringModels.nuevaVersion
{
    public class Dashboard
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Widget> list { get; set; }
    }

    public class Widget
    {
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public double? Min { get; set; }
        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public double? Value { get; set; }
        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public double? Max { get; set; }
        [JsonProperty("ratio", NullValueHandling = NullValueHandling.Ignore)]
        public double? Ratio { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
        
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url{ get; set; }
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }
        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<Widget> Items{ get; set; }
    }
}
