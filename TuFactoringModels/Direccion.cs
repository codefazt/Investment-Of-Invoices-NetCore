using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels
{
    public class Direccion
    {
        [JsonProperty("addressStreet", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressStreet { get; set; }
        [JsonProperty("addressBuilding", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressBuilding { get; set; }
        [JsonProperty("ciudad", NullValueHandling = NullValueHandling.Ignore)]
        public int Ciudad { get; set; }
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public PhonePrincipal Phone { get; set; } = new PhonePrincipal();
        [JsonProperty("zipCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ZipCode { get; set; }
    }
}
