using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model.Admin
{
    public class UbigeoResponse
    {
        public int codigo { get; set; }

        public string codigoPais { get; set; }
        public string pais { get; set; }

        public int codigoDepartamento { get; set; }
        public string departamento { get; set; }

        public int codigoProvincia{ get; set; }
        public string provincia { get; set; }

        public int codigoDistrito { get; set; }
        public string distrito { get; set; }

        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }

    }

    public class UbigeoRequest
    {
        public int codigo { get; set; }
        public string codigoPais { get; set; }
        public int codigoDepartamento { get; set; }
        public int codigoProvincia { get; set; }
        public int codigoDistrito { get; set; }
        public string codigoUbigeo { get; set; }
        public byte estado { get; set; }
    }
}
