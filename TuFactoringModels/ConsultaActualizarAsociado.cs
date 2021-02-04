using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ConsultaActualizarAsociado
    {
        public string id { get; set; }
        public string tipoRegistro { get; set; }
       
        public List<ActualizarAsociado> proveedores { get; set; } = new List<ActualizarAsociado>();
       
        public string pais { get; set; }
        public string participant { get; set; }
    }
}
