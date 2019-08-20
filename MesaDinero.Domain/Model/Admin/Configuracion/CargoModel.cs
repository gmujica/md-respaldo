using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model.Admin
{
    public class CargoResponse
    {
        public int codigo { get; set; }
        public string nombre { get; set; }
        public byte estado { get; set; }
        public  int total { get; set; }
        public string nombreEstado { get; set; }

        //public string nombreEstado { get{
        
        //    string resultado;

        //    if (estado == 1)
        //        resultado = "Activo";
        //    else if (estado == 2)
        //        resultado = "Inactivo";
        //    else
        //        resultado = "";

        //    return resultado;
        
        //}}

    }

    public class CargoRequest
    {
        public int codigo { get; set; }
        public string nombre { get; set; }
        public byte estado { get; set; }
 
    }

}
