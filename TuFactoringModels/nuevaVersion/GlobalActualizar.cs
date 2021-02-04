using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class GlobalActualizar
    {
        [JsonProperty("registrarse", NullValueHandling = NullValueHandling.Ignore)]
        public PesonProfile Registrarse { get; set; }
        [JsonProperty("dataPaises", NullValueHandling = NullValueHandling.Ignore)]
        public ListCountry DataPaises { get; set; }
        [JsonProperty("rol", NullValueHandling = NullValueHandling.Ignore)]
        public int Rol { get; set; }
        [JsonProperty("authRol", NullValueHandling = NullValueHandling.Ignore)]
        public bool AuthRol { get; set; }
        [JsonProperty("contratAuth", NullValueHandling = NullValueHandling.Ignore)]
        public bool ContratAuth { get; set; }
        [JsonProperty("nombresBancos", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> NombresBancos { get; set; } = new List<string>();
        [JsonProperty("cuentaActual", NullValueHandling = NullValueHandling.Ignore)]
        public Account CuentaActual { get; set; } = new Account();
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User User { get; set; }
        public int State { get; set; }

        //Tipos de Contact
        [JsonProperty("representante", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Representante { get; set; } = new Contact();
        [JsonProperty("contacto", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contacto { get; set; } = new Contact();
        [JsonProperty("administrador", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Admin { get; set; } = new Contact();
        [JsonProperty("asociadoActual", NullValueHandling = NullValueHandling.Ignore)]
        public Associate AsociadoActual { get; set; } = new Associate();

        //Modelo Para Actualizar Perfil (Prospecto)
        [JsonProperty("perfil", NullValueHandling = NullValueHandling.Ignore)]
        public Prospecto Perfil { get; set; }

        [JsonProperty("registrarseA", NullValueHandling = NullValueHandling.Ignore)]
        public PesonProfile RegistrarseA { get; set; }

        [JsonProperty("listaAsociados", NullValueHandling = NullValueHandling.Ignore)]
        public ConsultaActualizarAsociado ListaAsociados = new ConsultaActualizarAsociado();

        [JsonProperty("cities")]
        public ListCountry Cities { get; set; } = new ListCountry();
    }
}
