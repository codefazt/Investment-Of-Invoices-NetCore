using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ResponseConciliacionFile
    {
        [JsonProperty("response", NullValueHandling = NullValueHandling.Ignore)]
        public Response Response { get; set; }
    }
}
