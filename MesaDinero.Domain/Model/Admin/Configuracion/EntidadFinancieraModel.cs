using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MesaDinero.Domain.Model.Admin
{
    public class EntidadFinancieraResponses
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string logo { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModifica { get; set; }
        public string tipo { get; set; }
        public string formatoCCI { get; set; }
        public string formatoCB { get; set; }
        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }

    }


    public class EntidadFinancieraRequest
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string logo { get; set; }
        //public DateTime? fechaCreacion { get; set; }
        //public DateTime? fechaModifica { get; set; }
        public string tipo { get; set; }
        public string formatoCCI { get; set; }
        public string formatoCB { get; set; }
        public byte estado { get; set; }

        public HttpPostedFileBase archivo { get; set; }

    }
}
