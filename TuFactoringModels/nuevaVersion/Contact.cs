using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Contact
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("identification")]
        public int Identification { get; set; }

        [Display(Name = "Numero de Documento")]
        [JsonProperty("documentNumber")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Nombres y Apellidos")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display(Name = "Correo")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Display(Name = "Numero de Teléfono")]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("prefix")]
        public int? Prefix { get; set; }
    }
}
