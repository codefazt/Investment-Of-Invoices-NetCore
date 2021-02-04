using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels;
using Newtonsoft.Json;
namespace TuFactoringModels
{
    public class Identification
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [JsonProperty("discriminator", NullValueHandling = NullValueHandling.Ignore)]
        public string Discriminator { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Prefix { get; set; }
        [JsonProperty("digits", NullValueHandling = NullValueHandling.Ignore)]
        public int? Digits { get; set; }
        [JsonProperty("regexp", NullValueHandling = NullValueHandling.Ignore)]
        public string Regexp { get; set; }
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Default { get; set; }
        [JsonProperty("prefixes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Prefix> Prefixes { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public bool Status { get; set; }
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }

        [JsonProperty("mask_edit", NullValueHandling = NullValueHandling.Ignore)]
        public string MaskEdit { get; set; }
    }
}
