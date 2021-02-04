using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels.nuevaVersion
{
    public class Documents
    {
        [JsonProperty("list", NullValueHandling = NullValueHandling.Ignore)]
        public List<ListDocuments> List { get; set; }
    }
}
