using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ListVerification
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("person")]
        public Prospecto Person { get; set; }

        [JsonProperty("entity")]
        public Entity Entity { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("acceptedAt")]
        public string ScceptedAt { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
