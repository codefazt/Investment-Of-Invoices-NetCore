using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class People
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Last_name { get; set; }
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country_id { get; set; }
        [JsonProperty("discriminator", NullValueHandling = NullValueHandling.Ignore)]
        public string Discriminator { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }
        [JsonProperty("routing_number", NullValueHandling = NullValueHandling.Ignore)]
        public string RoutingNumber { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int? Status { get; set; }
        [JsonProperty("createAt", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }
        [JsonProperty("updateAt", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public string Errors { get; set; }

        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public List<Document> Documents { get; set; }
    }

    public partial class PeopleResponse
    {

        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<People> List { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}
