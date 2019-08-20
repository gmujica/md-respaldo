using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model.Admin
{
    public class DistritoResponse
    {
        public int codigo { get; set; }
        public string nombre { get; set; }
        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }
    }

    public class DistritoRequest
    {
        public int codigo { get; set; }
        public string nombre { get; set; }
        public byte estado { get; set; }
    }

}
