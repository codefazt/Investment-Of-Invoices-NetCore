using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Subdivision
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("abbreviation", NullValueHandling = NullValueHandling.Ignore)]
        public string Abbreviation { get; set; }
        [JsonProperty("iso_3166_2", NullValueHandling = NullValueHandling.Ignore)]
        public string Iso_3166_2 { get; set; }
        [JsonProperty("cities", NullValueHandling = NullValueHandling.Ignore)]
        public List<City> Cities { get; set; }
        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updateAt", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }
    }
}
