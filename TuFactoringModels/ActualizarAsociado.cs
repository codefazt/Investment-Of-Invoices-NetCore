using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class ActualizarAsociado
    {
        public DocumentoPrincipal documento { get; set; } = new DocumentoPrincipal();
        public string legalName { get; set; }
        public Contacto contacto { get; set; } = new Contacto();
        public RepresentantePrincipal representante { get; set; } = new RepresentantePrincipal();
        public int status { get; set; }
        public bool afiliado { get; set; }
    }
}
