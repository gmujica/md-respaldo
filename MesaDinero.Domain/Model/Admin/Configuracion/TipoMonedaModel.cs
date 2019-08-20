using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model.Admin
{
    public class TipoMonedaResponse
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string simbolo { get;set; }
        public DateTime? fechaCreacion { get;set; }
        public DateTime? fechaModifica { get; set; }
        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }
    }

    public class TipoMonedaRequest
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string simbolo { get; set; }

        public byte estado { get; set; }

    }

}
