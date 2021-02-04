using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Pagination
    {
        [JsonProperty("take", NullValueHandling = NullValueHandling.Ignore)]
        public int? Take { get; set; }
        [JsonProperty("skip", NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }
    }
}
