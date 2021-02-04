using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class LoginInput
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }
        [JsonProperty("confirmant", NullValueHandling = NullValueHandling.Ignore)]
        public Entity Entity { get; set; }
    }
}
