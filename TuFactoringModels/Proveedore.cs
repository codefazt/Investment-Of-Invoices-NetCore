using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels
{
    public class Proveedore
    {
        [JsonProperty("documento", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentoPrincipal Documento { get; set; } = new DocumentoPrincipal();
        [JsonProperty("legalName", NullValueHandling = NullValueHandling.Ignore)]
        public string LegalName { get; set; }
        [JsonProperty("contacto", NullValueHandling = NullValueHandling.Ignore)]
        public Contacto Contacto { get; set; } = new Contacto();
        [JsonProperty("representante", NullValueHandling = NullValueHandling.Ignore)]
        public RepresentantePrincipal Representante { get; set; } = new RepresentantePrincipal();
    }
}
