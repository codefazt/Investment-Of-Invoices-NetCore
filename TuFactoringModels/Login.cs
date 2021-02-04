using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringModels
{
    public class Login
    {
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public Entity Confirmant { get; set; } 
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public Error Error { get; set; }
    }
}
