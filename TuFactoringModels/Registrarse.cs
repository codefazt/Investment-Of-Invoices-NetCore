using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TuFactoringModels
{
    public class Registrarse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("tipoRegistro", NullValueHandling = NullValueHandling.Ignore)]
        public string TipoRegistro { get; set; }
        [JsonProperty("documentoPrincipal")]
        public DocumentoPrincipal DocumentoPrincipal { get; set; } = new DocumentoPrincipal();
        [JsonProperty("contacto")]
        public RepresentantePrincipal Contacto { get; set; } = new RepresentantePrincipal();
        [JsonProperty("administrador")]
        public Administrador Administrador { get; set; } = new Administrador();
        [JsonProperty("legalName")]
        public string LegalName { get; set; }
        [JsonProperty("commercialName")]
        public string CommercialName { get; set; }
        [JsonProperty("purpose")]
        public int Purpose { get; set; }
        [JsonProperty("occupation")]
        public int Occupation { get; set; }
        [JsonProperty("representante")]
        public RepresentantePrincipal Representante { get; set; } = new RepresentantePrincipal();
        [JsonProperty("direccion")]
        public Direccion Direccion { get; set; } = new Direccion();
        [JsonProperty("cuentas")]
        public List<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
        [JsonProperty("proveedores")]
        public List<Proveedore> Proveedores { get; set; } = new List<Proveedore>();
        [JsonProperty("reviews")]
        public List<Reviews> Reviews { get; set; } = new List<Reviews>();
        [JsonProperty("routing_number")]
        public string Routing_number { get; set; }
        [JsonProperty("logo")]
        public string Logo { get; set; }
        [JsonProperty("file")]
        public string File { get; set; }
        [JsonProperty("dob")]
        public string Dob { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("otherName")]
        public string OtherName { get; set; }
        [JsonProperty("pais")]
        public string Pais { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }

    }
}
