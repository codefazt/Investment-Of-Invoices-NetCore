using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ParamConciliarArchivo
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
        [JsonProperty("idBank", NullValueHandling = NullValueHandling.Ignore)]
        public string IdBank { get; set; }

    }
}
