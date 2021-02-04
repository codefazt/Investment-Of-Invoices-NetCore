using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Entity
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("routing_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Routing_number { get; set; }
        [JsonProperty("person", NullValueHandling = NullValueHandling.Ignore)]
        public Prospecto Person { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool Status { get; set; }
        [JsonProperty("related", NullValueHandling = NullValueHandling.Ignore)]
        public bool Related { get; set; }
        [JsonProperty("is_fintech", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFintech { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public Error Error { get; set; }
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
    }

    public partial class EntityResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Entity> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
