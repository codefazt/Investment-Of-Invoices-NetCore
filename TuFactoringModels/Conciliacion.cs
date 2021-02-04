using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class Conciliacion
    {
        public string Inversionistas { get; set; }
        public string Teléfono { get; set; }
        public string Email { get; set; }
        public List<Invoices> Invoices { get; set; }

    }
}
