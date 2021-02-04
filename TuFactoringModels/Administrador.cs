using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels
{
    public class Administrador
    {
        [JsonProperty("documento", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentoPrincipal Documento { get; set; } = new DocumentoPrincipal();
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("discriminator", NullValueHandling = NullValueHandling.Ignore)]
        public string Discriminator { get; set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public PhonePrincipal Phone { get; set; } = new PhonePrincipal();
    }
}
