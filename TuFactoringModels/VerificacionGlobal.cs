
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class VerificacionGlobal
    {
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<User> Users { get; set; } = new List<User>();
        [JsonProperty("prospectoSegmentar", NullValueHandling = NullValueHandling.Ignore)]
        public List<consultaVerificacion> ProspectoSegmentar { get; set; } = new List<consultaVerificacion>();
    }
}
