using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain
{
    public class TipoCambioGarantizadoPartner_Response
    {
        public decimal rango { get; set; }
        public decimal montoMinimo { get; set; }
        public decimal montoMaximo { get; set; }
        public string moneda { get; set; }
        public decimal tcCompra { get; set; }
        public decimal tcVenta { get; set; }
        public decimal spread { get; set; }
    }

    public class TipoCambioMercadoPartner_Response
    {
        public string proveedor{get;set;}
        public decimal rango { get; set; }
        public decimal montoMinimo { get; set; }
        public decimal montoMaximo { get; set; }
        public string moneda { get; set; }
        public decimal tcCompra { get; set; }
        public decimal tcVenta { get; set; }
        public decimal spread { get; set; }
        public string logo { get; set; }
    }

    public class FiltroBanco_Response
    {
        public string codBanco { get; set; }
    }

    public class SubastasActivasResponse
    {
        public int tiemporestante { get { return (finTiempoSubasta - DateTime.Now).Seconds; } }
        public int codigo { get; set; }
        public string codigoTex { get { return string.Format("{0:0000000000}",codigo); } }
        public DateTime finTiempoSubasta { get; set; }
        public int nroSubasta { get; set; }
        public string usuario { get; set; }
        public string operacion { get; set; }
        public string operacionText { get {

            string result = string.Empty;

            if (operacion == "")
                result = "Comprar";
            else
                result = "Vender";

            return result;

        } }
        public string moneda { get; set; }
        public decimal monto { get; set; }
        public decimal tipoCambioPactado { get; set; }
    }


}
