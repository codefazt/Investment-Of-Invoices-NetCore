using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Group
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public Country Country { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }

        [JsonProperty("program", NullValueHandling = NullValueHandling.Ignore)]
        public Program Program { get; set; }

        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviattion { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
        public List<Details> Details { get; set; }
        
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }
    }

    public class GroupResponse
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<Group> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
