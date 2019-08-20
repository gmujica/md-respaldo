using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model.Admin
{
    public class TiempoResponse
    {
        public string codigo { get; set; }
        public int? tiempoStandar { get; set; }
        public int? tiempoPremiun { get; set; }
        public int? tiempoVip { get; set; }

        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }

        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }
    }

    public class TiempoRequest
    {
        public string codigo { get; set; }
        public int tiempoStandar { get; set; }
        public int tiempoPremiun { get; set; }
        public int tiempoVip { get; set; }
        public byte estado { get; set; }
    }

}
