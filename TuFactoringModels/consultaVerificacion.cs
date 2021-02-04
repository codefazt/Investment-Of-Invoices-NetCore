using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class consultaVerificacion
    {
        public string id { get; set; }
        public string name { get; set; }
        public string prefijo1 { get; set; }
        public string documento1 { get; set; }
        public string representante { get; set; }
        public string category { get; set; }
        public string prefijo2 { get; set; }
        public string documento2 { get; set; }
        public string direccion { get; set; }
        public string estado { get; set; }
        public string nombrecontacto { get; set; }
        public string documentocontacto { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string monto_riesgo { get; set; }
        public List<Segmentadores> segmentadores { get; set; } = new List<Segmentadores>();
    }
}
