using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class ApproveVerification
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("person")]
        public string Person { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("accepted")]
        public bool? Accepted { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }

    }
}
