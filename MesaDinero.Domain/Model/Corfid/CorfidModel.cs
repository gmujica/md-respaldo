using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model
{
    public class OperacionesHistoricas
    {

        public int idTransaccion { get; set; }
        public int? tiempoRestante { get; set; }
        public string partnersAdjuntado { get; set; }
        public string usuario { get; set; }
        public string quiere { get; set; }
        public string tipoMoneda { get; set; }
        public decimal? monto { get; set; }
        public decimal? montoRecibe { get; set; }
        public string monedaEnvia { get; set; }
        public string monedaRecibe { get; set; }
        public decimal? precioPactado { get; set; }
        public string cuentaOrigen { get; set; }
        public string bancoOrigen { get; set; }
        public string cuentaDestino { get; set; }
        public string bancoDestino { get; set; }
        public decimal? montoUsd { get; set; }
        public decimal? montoPen { get; set; }
        public string estadoSubastaCodigo { get; set; }
        public string logoOrigen { get; set; }
        public string logoDestino { get; set; }

        public DateTime? fechaFinPago { get; set; }
        public decimal? totalm { get; set; }
        public string estado { get; set; }
        public string estadoSubasta { get; set; }
        public int total { get; set; }

       
        public string horaFin { get; set; }
        public DateTime? fecha { get; set; }
        public string hora
        {
            get
            {
                if (fecha.HasValue)
                    return fecha.Value.ToString("HH:mm");
                else
                    return "";
            }
        }

         public string fechaShort { 
             get{
              if (fecha.HasValue)
                        return fecha.Value.ToString("dd/MM/yyyy");
                    else
                        return "";
             } 
         }

     

        public string fechaHora
        {
            get
            {
                if (fecha.HasValue)
                    return fecha.Value.ToString("d MMMM") + ' ' + fecha.Value.ToString("HH:mm");
                else
                    return "";
            }
        }

        public string formatoTransaccion
        {
            get
            {
                return String.Format("{0:000000000}", idTransaccion);
            }
        }
        public string nuevoFormato
        {
            get
            {
                return formatoTransaccion + "-" + fechaHora;
            }
        }

        public bool checkPago
        {
            get
            {
                return false;
            }
        }



    }

    public class FideicomisoResponse
    {

        public string numeroDocumento { get; set; }
        public string nombreEmpresa { get; set; }
        public string codigobanco { get; set; }
        public string banco { get; set; }
        public string codigoMoneda { get; set; }
        public string moneda { get; set; }
        public string codigoTipoCuenta { get; set; }
        public string tipoCuenta { get; set; }
        public string numeroCuenta { get; set; }
        public string numeroCuentaInter { get; set; }
      
    

    }


}
