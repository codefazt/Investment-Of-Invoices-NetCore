
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class RegistroGlobal
    {
        [JsonProperty("registrarse", NullValueHandling = NullValueHandling.Ignore)]
        public Registrarse Registrarse { get; set; }
        [JsonProperty("registrarseA", NullValueHandling = NullValueHandling.Ignore)]
        public RegistroActualizar RegistrarseA { get; set; }
        [JsonProperty("dataPaises", NullValueHandling = NullValueHandling.Ignore)]
        public Country DataPaises { get; set; }
        [JsonProperty("cuentaActual", NullValueHandling = NullValueHandling.Ignore)]
        public Cuenta CuentaActual { get; set; } = new Cuenta();
        [JsonProperty("asociadoActual", NullValueHandling = NullValueHandling.Ignore)]
        public Proveedore AsociadoActual { get; set; } = new Proveedore();
        [JsonProperty("nombresBancos", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> NombresBancos { get; set; } = new List<string>();

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        //public Credentials credentials { get; set; }
        public User User { get; set; }
        [JsonProperty("rol", NullValueHandling = NullValueHandling.Ignore)]
        public int Rol { get; set; }

        [JsonProperty("listaAsociados", NullValueHandling = NullValueHandling.Ignore)]
        public ConsultaActualizarAsociado ListaAsociados = new ConsultaActualizarAsociado();
    }
}
