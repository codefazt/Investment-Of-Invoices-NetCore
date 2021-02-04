using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TuFactoringModels
{
    public class DocumentoPrincipal
    {
        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public int Identification { get; set; }
        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public int Prefix { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

    }
}
