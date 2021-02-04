using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.Interface
{
    public interface IStatus
    {
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        string State { get; set; }

        [JsonProperty("state_mostrar", NullValueHandling = NullValueHandling.Ignore)]
        string StateMostrar { get; set; }

        [JsonProperty("participant", NullValueHandling = NullValueHandling.Ignore)]
        string Participant { get; set; }
    }
}
