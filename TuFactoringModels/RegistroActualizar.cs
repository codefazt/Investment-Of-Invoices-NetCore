using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class RegistroActualizar
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("tipoRegistro", NullValueHandling = NullValueHandling.Ignore)]
        public string TipoRegistro { get; set; }
        [JsonProperty("documentoPrincipal", NullValueHandling = NullValueHandling.Ignore)]
        public DocumentoPrincipal DocumentoPrincipal { get; set; } = new DocumentoPrincipal();
        [JsonProperty("contacto", NullValueHandling = NullValueHandling.Ignore)]
        public RepresentantePrincipal Contacto { get; set; } = new RepresentantePrincipal();
        [JsonProperty("administrador", NullValueHandling = NullValueHandling.Ignore)]
        public Administrador Administrador { get; set; } = new Administrador();
        [JsonProperty("legalName", NullValueHandling = NullValueHandling.Ignore)]
        public string LegalName { get; set; }
        [JsonProperty("commercialName", NullValueHandling = NullValueHandling.Ignore)]
        public string CommercialName { get; set; }
        [JsonProperty("purpose", NullValueHandling = NullValueHandling.Ignore)]
        public int Purpose { get; set; }
        [JsonProperty("occupation", NullValueHandling = NullValueHandling.Ignore)]
        public int Occupation { get; set; }
        [JsonProperty("representante", NullValueHandling = NullValueHandling.Ignore)]
        public RepresentantePrincipal Representante { get; set; } = new RepresentantePrincipal();
        [JsonProperty("direccion", NullValueHandling = NullValueHandling.Ignore)]
        public Direccion Direccion { get; set; } = new Direccion();
        [JsonProperty("cuentas", NullValueHandling = NullValueHandling.Ignore)]
        public List<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
        [JsonProperty("proveedores", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActualizarAsociado> Proveedores { get; set; } = new List<ActualizarAsociado>();
        [JsonProperty("reviews", NullValueHandling = NullValueHandling.Ignore)]
        public List<Reviews> Reviews { get; set; } = new List<Reviews>();
        [JsonProperty("routing_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Routing_number { get; set; }
        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public string File { get; set; }
        [JsonProperty("dob", NullValueHandling = NullValueHandling.Ignore)]
        public string Dob { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("otherName", NullValueHandling = NullValueHandling.Ignore)]
        public string OtherName { get; set; }
        [JsonProperty("pais", NullValueHandling = NullValueHandling.Ignore)]
        public string Pais { get; set; }
        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        public string Participant { get; set; }

    }
}
