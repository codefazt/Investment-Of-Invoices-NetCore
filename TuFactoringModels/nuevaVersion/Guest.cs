using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Guest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public int? Country { get; set; }
        [JsonProperty("identification", NullValueHandling = NullValueHandling.Ignore)]
        public int Identification { get; set; }
        [JsonProperty("prefix", NullValueHandling = NullValueHandling.Ignore)]
        public int Prefix { get; set; }
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public string Company { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("phone_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone_number { get; set; }
        [JsonProperty("invitedAt", NullValueHandling = NullValueHandling.Ignore)]
        public string InvitedAt { get; set; }
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }
    }
}
