using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ListCountry
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("calling_code", NullValueHandling = NullValueHandling.Ignore)]
        public string Calling_code { get; set; }
        [JsonProperty("currencies", NullValueHandling = NullValueHandling.Ignore)]
        public List<Currency> Currencies { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public Currency Currency { get; set; }
        [JsonProperty("identifications", NullValueHandling = NullValueHandling.Ignore)]
        public List<Identification> Identifications { get; set; }
        [JsonProperty("categories", NullValueHandling = NullValueHandling.Ignore)]
        public List<Category> Categories { get; set; }
        [JsonProperty("entities", NullValueHandling = NullValueHandling.Ignore)]
        public List<Entity> Entities { get; set; }
        [JsonProperty("allies", NullValueHandling = NullValueHandling.Ignore)]
        public List<Entity> Allies { get; set; }
        [JsonProperty("regions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Region> Regions { get; set; }
        [JsonProperty("settings", NullValueHandling = NullValueHandling.Ignore)]
        public List<Settings> Settings { get; set; }
    }
}
