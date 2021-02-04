using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
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

        [JsonProperty("mask_edit", NullValueHandling = NullValueHandling.Ignore)]
        public string Mask_edit { get; set; }
        [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Default { get; set; }
        [JsonProperty("prefixes", NullValueHandling = NullValueHandling.Ignore)]
        public List<Prefix> Prefixes { get; set; }

    }
}
