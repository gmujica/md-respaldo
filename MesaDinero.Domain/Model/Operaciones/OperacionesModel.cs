using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model.operaciones
{
   
    public class VerificacionPagoResponse
    {

        public int idTransaccion { get; set; }
        public string tipoOperacion { get; set; }
        public string usuario { get; set; }
        public string accion { get; set; }
        public string tipoMoneda { get; set; }
        public decimal? monto { get; set; }
        public string monedaEnvia { get; set; }

        public string cuentaOrigen { get; set; }
        public string bancoOrigen { get; set; }
        public string nroOperacionPago { get; set; }
        public string estadoSubastaCodigo { get; set; }
        public bool checkPago {
            get
            {
                return false;
            }
        }
        public string estado { get; set; }
        public string estadoSubasta { get; set; }
        public int total { get; set; }
        public DateTime? fechaConf { get; set; }
        public string fechaShort { get; set; }
        public double horaFin { get; set; }
        public string estadoLMD { get; set; }
        public string usuarioLMD { get; set; }
        public string estadoFideicomiso { get; set; }
        public string usuarioFideicomiso { get; set; }
        public string hora
        {
            get
            {
                if (fechaConf.HasValue)
                    return fechaConf.Value.ToString("HH:mm");
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
      
  
    }



    public class ListaGenerarPagoResponse
    {

        public int idTransaccion { get; set; }
        public string tipoOperacion { get; set; }
        public string usuario { get; set; }
        public string accion { get; set; }
        public string tipoMoneda { get; set; }
        public decimal? monto { get; set; }
        public string monedaRecibe { get; set; }

        public string cuentaDestino { get; set; }
        public string bancoDestino { get; set; }
        public string estadoSubastaCodigo { get; set; }
        public bool checkPago
        {
            get;
            set;
        }

        public bool checkEnviar
        {
            get;
            set;
        }
        public string estado { get; set; }
        public string estadoSubasta { get; set; }
        public int total { get; set; }
        public string fechaShort { get; set; }
        public double horaFin { get; set; }
        public int estadoExportado { get; set; }
        public Byte?[] archivoExportado { get; set; }
        public DateTime? horaCorfid { get; set; }
       
        public string hora_Corfid
        {
            get
            {
                if (horaCorfid.HasValue)
                    return horaCorfid.Value.ToString("HH:mm");
                else
                    return "";
            }
        }

          public string montoFormat
        {
            get
            {
                if (monto.HasValue)
                    return String.Format("{0:###,###,###,##0.00##}", monto);
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

    }


    public class ListaAprobarPagoResponse
    {

        public int idTransaccion { get; set; }
        public string tipoOperacion { get; set; }
        public string usuario { get; set; }
        public string accion { get; set; }
        public string tipoMoneda { get; set; }
        public decimal? monto { get; set; }
        public string monedaRecibe { get; set; }

        public string cuenta { get; set; }
        public string banco { get; set; }
        public string estadoSubastaCodigo { get; set; }
        public bool checkPago
        {
            get;
            set;
        
        }
        public DateTime? horaCorfid { get; set; }
        public string hora_Corfid
        {
            get
            {
                if (horaCorfid.HasValue)
                    return horaCorfid.Value.ToString("HH:mm");
                else
                    return "";
            }
        }
        public string estado { get; set; }
        public string estadoSubasta { get; set; }
        public int total { get; set; }
        public string fechaShort { get; set; }
        public double horaFin { get; set; }
        public string IngresarIdPago { get; set; }
        public string formatoTransaccion
        {
            get
            {
                return String.Format("{0:000000000}", idTransaccion);
            }
        }

    }



    public class FiltroOperacionParam
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int idFilter { get; set; }
        public string textFilter { get; set; }
        public string searchFilter { get; set; }
        public string bancoFilter { get; set; }
        public string monedaFilter { get; set; }
    }

    public class SubastaRequest
    {

        public string idTransaccion { get; set; }
        public string idPago { get; set; }
        public string observacion { get; set; }
        public string tipoValidacion { get; set; } /* VO,VF*/
        public string  estado { get; set; }

    }


    public class ListaGenerarFormatoTxtResponse
    {
        public int idTransaccion { get; set; }
        public string codigoBancoDestino { get; set; }
        public string bancoDestino { get; set; }
        public string tipoMoneda { get; set; }
        public string usuario { get; set; }
        public string tipoCuenta { get; set; }
        public int tipoCliente { get; set; }
        
        public string tipoDocu { get; set; }
        public string nroDocu { get; set; }

        public decimal? monto { get; set; }
        public string monedaRecibe { get; set; }

        public string cuentaDestino { get; set; }
       
    }


    public class EstatusOperacionesConfirmadas
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

        public string fechaShort { get; set; }
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

        public string fechaHora { 
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
        public string nuevoFormato { get {
            return formatoTransaccion + "-" + fechaHora;
        } }
      
    }

    public class ExportFiles
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Byte[] FileBytes { get; set; }
    }

    public class InstruccionPagoResponse
    {

        public int idTransaccion { get; set; }
        public string partnersAdjuntado { get; set; }
        public string usuario { get; set; }
        public string tipoMoneda { get; set; }
        public decimal? monto { get; set; }
        public string monedaEnvia { get; set; }

        public decimal? montoRecibe { get; set; }
        public string monedaRecibe { get; set; }

        public decimal? precioPactado { get; set; }

        public string cuentaOrigen { get; set; }
        public string bancoOrigen { get; set; }
        public string cuentaDestino { get; set; }
        public string bancoDestino { get; set; }

        public string estadoSubastaCodigo { get; set; }

        public string logoOrigen { get; set; }
        public string logoDestino { get; set; }

        public string estado { get; set; }
        public string estadoSubasta { get; set; }
        public int total { get; set; }

        public string fechaShort { get; set; }
        public DateTime? fecha { get; set; }

        public string bancoFideicomiso { get; set; }
        public string monedaBancoFideicomiso { get; set; }
        public string nroCuentaFideicomiso { get; set; }
        public string RucFideicomiso { get; set; }
        public string nombreFideicomiso { get; set; }
        public string tipoCuentaFideicomiso { get; set; }
        
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

    }
}
