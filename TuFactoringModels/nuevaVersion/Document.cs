using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Document
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("identification")]
        public int Identification { get; set; }
        [JsonProperty("prefix")]
        public int? Prefix { get; set; }

        [Display(Name = "Numero de Documento")]
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
