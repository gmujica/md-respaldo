using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain
{

    public class Subasta_Verificacion_Response
    {
        public helpMenuSubasta menu = new helpMenuSubasta();
        public decimal montoEnvia { get; set; }
        public decimal montoRecibe { get; set; }
        public string monedaEnvia { get; set; }
        public string monedaRecibe { get; set; }
        public string nroSubasta { get; set; }
        public string nroOperacion { get; set; }
        public string bancoOriegn { get; set; }
        public string cliente { get; set; }
        public DateTime fechaConfirmacion { get; set; }
        public DateTime fechaPago { get; set; }
        public string sid { get; set; }
        public string estado { get; set; }
        public string valorEnvioText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoEnvia.ToString(specifier, CultureInfo.InvariantCulture);
            }

        }

        public string valorRecibeText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoRecibe.ToString(specifier, CultureInfo.InvariantCulture);
            }

        }
    }

    public class Subasta_Recibe_Response
    {
        public helpMenuSubasta menu = new helpMenuSubasta();
        public string montoEnvia { get; set; }
        public string monedaEnvia { get; set; }
        public string montoRecibe { get; set; }
        public string nroOperacion { get; set; }
        public string monedaRecibe { get; set; }
        public string cliente { get; set; }
        public string bancoOriegn { get; set; }
        public string bancoDestino { get; set; }
        public string bancoDestinoLogo { get; set; }
        public string numeroCuentaDestino { get; set; }
        public DateTime fechaConfirmacion { get; set; }
        public DateTime fechaPago { get; set; }
        public DateTime fechaValidaRecibio { get; set; }
        public DateTime fechaInformePago { get; set; }
        public string numOpBancoCliente { get; set; }
        public string sid { get; set; }
    }


    public class Subasta_ConfirmarPago_Response
    {
        public helpMenuSubasta menu = new helpMenuSubasta();
        public string nombreTitular { get; set; }
        public int tiempo { get; set; }
        public int transaccion { get; set; }
        public DateTime fechaConfirmacion { get; set; }
        public string nombrePartner { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal valorEnvio { get; set; }
        public decimal valorRecibe { get; set; }
        public string monedaEnvio { get; set; }
        public string monedaRecibe { get; set; }
        public int subasta { get; set; }

        public string sid { get; set; }

        public List<ComboCuentasBancarias> cuentaBancos = new List<ComboCuentasBancarias>();
        public List<ComboCuentasBancarias> cuentaBancosDestino = new List<ComboCuentasBancarias>();

        public string cuentaOrigen { get; set; }
        public string logo_cuentaOrigen { get; set; }
        public string cuentaDestino { get; set; }
        public string logo_cuentaDestino { get; set; }

    }

    public class helpMenuSubasta
    {
        public string sid { get; set; }
        public string menu { get; set; }


    }

    public class Subasta_ConfirmarOperacion_Response
    {
        public helpMenuSubasta menu = new helpMenuSubasta();
        public int initSubasta { get; set; }
        public int tiempo { get; set; }
        public int transaccion { get; set; }
        public string nombrePartner { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal valorEnvio { get; set; }
        public decimal valorRecibe { get; set; }
        public string monedaEnvio { get; set; }
        public string monedaRecibe { get; set; }
        public string subasta { get; set; }
        public List<ComboCuentasBancarias> cuentaBancos = new List<ComboCuentasBancarias>();
    }

    public class SubastaProceso_Response
    {
        public int initSubasta { get; set; }
        public helpMenuSubasta menu = new helpMenuSubasta();
        public string sid { get; set; }
        public decimal monto { get; set; }
        public string opr { get; set; }
        public int tiempoConfirmacion { get; set; }
        public int tiempo { get; set; }
        public int codigo { get; set; }
        public string monedaEnvio { get; set; }
        public string monedaRecive { get; set; }
        public decimal valorEnvio { get; set; }
        //public string valorEnvioText { get { return string.Format("{0:n}", valorEnvio); } }
        public string valorEnvioText { get {
            CultureInfo culture;
            string specifier;
            specifier = "N2";
            culture = CultureInfo.CreateSpecificCulture("eu-ES");
            return  valorEnvio.ToString(specifier,CultureInfo.InvariantCulture); }

        }
        public decimal valorRecibe { get; set; }
        public string valorRecibeText { get { 
              CultureInfo culture;
            string specifier;
            specifier = "N2";
            culture = CultureInfo.CreateSpecificCulture("eu-ES");
            return  valorRecibe.ToString(specifier,CultureInfo.InvariantCulture); }

            //return string.Format("{0:n}", valorRecibe); } 
        
        }
        public string partner { get; set; }
        public decimal tipoCambio { get; set; }
        public string tipoCambioText { get {
            CultureInfo culture;
            string specifier;
            specifier = "N4";
            culture = CultureInfo.CreateSpecificCulture("eu-ES");
             return  tipoCambio.ToString(specifier,CultureInfo.InvariantCulture); 
            //return string.Format("{0:#.000}", tipoCambio); 
        } 
        }
        public int codigoSeleccion { get; set; }
        public int subasta { get; set; }
        public List<Subasta_PartnerSubastaPuja> partners = new List<Subasta_PartnerSubastaPuja>();
    }

    public class SubastaCurrentTime
    {
        public int tiempo { get; set; }
        public string estado { get; set; }
        public string sid { get; set; }
    }

    public class Subasta_PartnerSubastaPuja
    {
        public string classCss { get; set; }
        public int codigo { get; set; }
        public string id { get; set; }
        public string nombre { get; set; }
        public decimal? tipoCambio { get; set; }
        //public string tipoCambioText { get; set; }
        public string tipoCambioText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                if (tipoCambio.HasValue)
                {
                    return tipoCambio.Value.ToString(specifier, CultureInfo.InvariantCulture);
                }
                else {
                    return "";
                }
                
                //return string.Format("{0:#.000}", tipoCambio); 
            }
            set { }
        }
        public string subasta { get; set; }
        public string indice { get; set; }
    }

    public class SubastaCliente_PartnerPuja
    {
        public SubastaCliente_PartnerPuja()
        {
            puja = new List<Subasta_PartnerSubastaPuja>();
            seleccion = new SubastaProceso_Response();
        }

        public List<Subasta_PartnerSubastaPuja> puja { get; set; }

        public SubastaProceso_Response seleccion { get; set; }

    }


    public class Subasta_Init_Reponse
    {
        public string menuHtml { get; set; }
        public string subastaHtml { get; set; }
        public int tiempo { get; set; }
        public List<string> partners = new List<string>();
    }

    public class CambioPassWordRequest
    {
        public string email { get; set; }
        public string sid { get; set; }
    }


    public class TiposCambioGarantizado_SubastaProceso_Request
    {
        public string nroDocumento { get; set; }
        public decimal? valorMinimo { get; set; }
        public decimal? valorMaximo { get; set; }
        public string tipoMoneda { get; set; }
        public decimal valorCompra { get; set; }
        public decimal valorVenta { get; set; }
        public string razonSocial { get; set; }
        public decimal tipoCambio { get; set; }

    }

    public class SubastaCompararProveedorResponse
    {
        string host = ConfigurationManager.AppSettings["HostAdmin"];
        public int codigo { get; set; }
        public string codBanco { get; set; }
        public string banco { get; set; }
        public string logo { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal monto { get; set; }
        public string rutaLogo { get {
            return host + logo;
        } }

        public string montoText
        {
            get
            { 
              CultureInfo culture;
            string specifier;
            specifier = "N2";
            culture = CultureInfo.CreateSpecificCulture("eu-ES");
            return "S/ "+monto.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }
        public string tipoCambioText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return tipoCambio.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }

  
    }

    public class SubastaCompararProveedorRequest
    {
        public string venta { get; set; }
        public decimal monto { get; set; }
    }


    public class ListaOperacionSubastaCliente
    {

        public int idTransaccion { get; set; }
        public string partnersAdjuntado { get; set; }
        public string usuario { get; set; }
        public string quiere { get; set; }
        public string tipoMoneda { get; set; }
        public decimal? monto { get; set; }
        public decimal? montoRecibe { get; set; }
        public string monedaEnvia { get; set; }
        public string monedaRecibe { get; set; }
        public decimal? precioPactado { get; set; }

        public decimal? montoUsd { get; set; }
        public decimal? montoPen { get; set; }


        public string estadoSubastaCodigo { get; set; }

        public DateTime? fechaFinPago { get; set; }
        public decimal? totalm { get; set; }
        public string estado { get; set; }
        public string estadoSubasta { get; set; }
        public int total { get; set; }

       
        public string horaFin { get; set; }
        public DateTime? fecha { get; set; }

        public string fechaShort
        {
            get
            {
                if (fecha.HasValue)
                {
                    return fecha.Value.ToString("dd/MM/yyyy");
                }
                else {
                    return "";
                }
                
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
