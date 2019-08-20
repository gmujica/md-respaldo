using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model
{

    public class ReanudarRegistroRequest
    {
        public string email { get; set; }
        public string nroDocumento { get; set; }
    }

    public class RegistroClientesRequest
    {
        public string nroDocumentoContacto { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int tipoPersona { get; set; }
        public string nroDocumento { get; set; }
        public string nombreEmpresa { get; set; }
    }

    public class ConfirmacionMsMResponse
    {
        public int minutos { get; set; }
        public int segundos { get; set; }
    }

    public class ValiarSMSRequest
    {
        public string sid { get; set; }
        public string clavemsm { get; set; }
    }

    public class PersonaAutorizadaRequest
    {
        public string sid { get; set; }
        public List<PersonaNatutalRequest> autorizados = new List<PersonaNatutalRequest>();
    }

    public class PersonaNatutalRequest
    {
        public string sid { get; set; }
        public string sictuacionLaboral { get; set; }
        public int cargo { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string nombres { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public int fnDia { get; set; }
        public int fnMes { get; set; }
        public int fnAnio { get; set; }
        public string email { get; set; }
        public string expuesto { get; set; }
        public string preCelular { get; set; }
        public string celular { get; set; }
        public string pais { get; set; }
        public int departamento { get; set; }
        public int provincia { get; set; }
        public int distrito { get; set; }
        public string direccion { get; set; }
        public int origenFondos { get; set; }

        public string entidadNombreExpuesto { get; set; }
        public string cargoExpuesto { get; set; }
        public byte? estado { get; set; }
    }



    public class PersonaNatutalAllResponse2
    {
        public string sid { get; set; }
        public string sictuacionLaboral { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string nombres { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string fechaNacimiento { get; set; }
        public string email { get; set; }
        public string expuesto { get; set; }
        public string celular { get; set; }
        public string pais { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string direccion { get; set; }
        public string origenFondos { get; set; }
    }

    public class PersonaNatutalAllResponse
    {
        public string sid { get; set; }
        public string sictuacionLaboral { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string nombres { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string fechaNacimiento { get; set; }
        public string email { get; set; }
        public string expuesto { get; set; }
        public string validacionOperador { get; set; }
        public string validacionFideicomiso { get; set; }
        public string celular { get; set; }
        public string pais { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string direccion { get; set; }
        public string msmFideicomiso { get; set; }
        public string msmOperador { get; set; }
        public string origenFondos { get; set; }
        public List<CuentaBancariaClienteResponse> cuentasBancarias = new List<CuentaBancariaClienteResponse>();
        public List<ObservacionesClienteResponse> observacionesCliente = new List<ObservacionesClienteResponse>();

    }

    public class ObservacionesClienteResponse {
        public DateTime? fechaCreacion { get; set; }
        public string mensaje { get; set; }
        public string observador { get; set; }
        public string nombre { get; set; }
        public string nombreEstado { get; set; }
        public int total { get; set; }
        public string fechaOperador
        {
            get
            {
                if (fechaCreacion.HasValue)
                    return fechaCreacion.Value.ToString("dd/MM/yyyy");
                else
                    return "";
            }
        }



    }

    public class CuentaBancariaClienteResponse
    {
        public string banco { get; set; }
        public long? codigo { get; set; }
        public string logo { get; set; }
        public string monedatext { get; set; }
        public string moneda { get; set; }
        public string tipoCuentaText { get; set; }
        public int? tipoCuenta { get; set; }
        public string nroCuenta { get; set; }
        public string nroCCI { get; set; }
        public byte? estado { get; set; }
        public string nombreEstado { get; set; }
    }

    public class DatoBancariaCliente_Request
    {
        public DatoBancariaCliente_Request()
        {
            cuentas = new List<DatoBancariaRequest>();
        }

        public string sid { get; set; }
        public List<DatoBancariaRequest> cuentas { get; set; }
    }

    public class DatoBancariaRequest
    {
        public long codigo { get; set; }
        public string banco { get; set; }
        public string moneda { get; set; }
        public string monedatext { get; set; }
        public int tipoCuenta { get; set; }
        public string tipoCuentaText { get; set; }
        public string nroCuenta { get; set; }
        public string nroCCI { get; set; }
        public string logo { get; set; }
        public int estado { get; set; }
        public string estadoNombre { get; set; }
    }


    public class PersonaJuridicaAllResponse
    {
        public PersonaJuridicaAllResponse()
        {
            repreLegal = new PersonaNatutalAllResponse();
            accionistas = new List<PersonaNatutalAllResponse2>();
            personaAutorizada = new List<PersonaNatutalAllResponse2>();
        }
        public int sid { get; set; }
        public string ruc { get; set; }
        public string nombre { get; set; }
        public string actividadEconomica { get; set; }
        public string origenFondos { get; set; }

        // datos de direccion
        public string pais { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string direccion { get; set; }
        public string rucArchivo { get; set; }
        public string estadoValidacion { get; set; }
        public string estadoValidacionFideicomiso { get; set; }

        public List<ObservacionesClienteResponse> observacionesCliente = new List<ObservacionesClienteResponse>();
        public PersonaNatutalAllResponse repreLegal { get; set; }
        public List<PersonaNatutalAllResponse2> personaAutorizada { get; set; }
        public List<PersonaNatutalAllResponse2> accionistas { get; set; }

        public List<CuentaBancariaClienteResponse> cuentasBancarias = new List<CuentaBancariaClienteResponse>();
        public List<ClienteOperacionesResponse> clienteOperaciones = new List<ClienteOperacionesResponse>();
    }



    public class PersonaJuridicaReuest
    {
        public PersonaJuridicaReuest()
        {
            repreLegal = new PersonaNatutalRequest();
            accionistas = new List<PersonaNatutalRequest>();
        }

        public string ruc { get; set; }
        public string nombre { get; set; }
        public int? actividadEconomica { get; set; }
        public int? origenFondos { get; set; }

        // datos de direccion
        public string pais { get; set; }
        public int? departamento { get; set; }
        public int? provincia { get; set; }
        public int? distrito { get; set; }
        public string direccion { get; set; }
        public string estadoValidacion { get; set; }
        public string rucArchivo { get; set; }
        public int modoEdicion { get; set; }
        public PersonaNatutalRequest repreLegal { get; set; }
        public IList<PersonaNatutalRequest> accionistas { get; set; }

    }

    public class PersonaJuridicaRegistroRequest
    {
        public PersonaJuridicaRegistroRequest()
        {

            empresa = new PersonaJuridicaReuest();
            repreLegal = new PersonaNatutalRequest();
            accionistas = new List<PersonaNatutalRequest>();
        }
        public string sid { get; set; }
        public PersonaJuridicaReuest empresa { get; set; }
        public PersonaNatutalRequest repreLegal { get; set; }
        public IList<PersonaNatutalRequest> accionistas { get; set; }

    }

    public class ClienteRegsitradosOperador
    {
        public string tipoRegistro { get; set; }
        public string tipoRegistroText { get; set; }
        public int sid { get; set; }
        public DateTime fechaRegistro { get; set; }

        public string fechaRegistroShort { get { return fechaRegistro.ToString("dd/MM/yyyy"); } }
        public string horaRegistro { get { return fechaRegistro.ToString("HH:MM"); } }

        public string tipoClienteText { get; set; }
        public int tipocCliente { get; set; }
        public string nombre { get; set; }
        public string tipoDocumento { get; set; }

        public string nroDocumento { get; set; }
        public string estadoValidacion_Operador { get; set; }
        public string estadoValidacion_Fideicomiso { get; set; }
        public string operadorValidador { get; set; }
        public string fideicomisoValidador { get; set; }
        public DateTime? fechaValidacion_Operador { get; set; }
        public DateTime? fechaValidacion_Fideicomiso { get; set; }
        public string tipoValidacion { get; set; }
        public string fechaOperador
        {
            get
            {
                if (fechaValidacion_Operador.HasValue)
                    return fechaValidacion_Operador.Value.ToString("dd/MM/yyyy HH:mm");
                else
                    return "";
            }
        }

        public string fechaFideicomiso
        {
            get
            {
                if (fechaValidacion_Fideicomiso.HasValue)
                    return fechaValidacion_Fideicomiso.Value.ToString("dd/MM/yyyy HH:mm");
                else
                    return "";
            }
        }

        public int total { get; set; }
    }

    #region Param
    public class GetDatosPersonaNatural
    {
        public string nroDocumento { get; set; }
        public string sid { get; set; }
        public int? cid { get; set; }
    }
    #endregion


    /*cliente*/

    public class PersonaJuridicaFideicomisoRegistroRequest
    {
        public PersonaJuridicaFideicomisoRegistroRequest()
        {
            tipoEmpresa = "";
            empresa = new PersonaJuridicaReuest();
            repreLegal = new PersonaNatutalRequest();
            personaAutorizada = new PersonaNatutalRequest();
            accionistas = new List<PersonaNatutalRequest>();
            cuentas = new List<DatoBancariaRequest>();
        }
        public string tipoEmpresa { get; set; }
        public PersonaJuridicaReuest empresa { get; set; }
        public PersonaNatutalRequest repreLegal { get; set; }
        public PersonaNatutalRequest personaAutorizada { get; set; }
        public IList<PersonaNatutalRequest> accionistas { get; set; }
        public List<DatoBancariaRequest> cuentas { get; set; }
    }

    public class EmpresaRegistroRequest
    {
        public EmpresaRegistroRequest()
        {
            tipoEmpresa = "";
            empresa = new PersonaJuridicaReuest();
            repreLegal = new PersonaNatutalRequest();
            personaAutorizadas = new List<PersonaNatutalRequest>();
            accionistas = new List<PersonaNatutalRequest>();
            cuentas = new List<DatoBancariaRequest>();
        }
        public string tipoEmpresa { get; set; }
        public PersonaJuridicaReuest empresa { get; set; }
        public PersonaNatutalRequest repreLegal { get; set; }
        public IList<PersonaNatutalRequest> personaAutorizadas { get; set; }
        public IList<PersonaNatutalRequest> accionistas { get; set; }
        public List<DatoBancariaRequest> cuentas { get; set; }

    }

    public class ClientePersonaNatutalAllResponse
    {
        public string sid { get; set; }
        public string sictuacionLaboral { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string nombres { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string fechaNacimiento { get; set; }
        public string email { get; set; }
        public string expuesto { get; set; }
        public string validacionOperador { get; set; }
        public string validacionFideicomiso { get; set; }
        public string celular { get; set; }
        public string pais { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string direccion { get; set; }
        public string msmFideicomiso { get; set; }
        public string msmOperador { get; set; }
        public string origenFondos { get; set; }
        public List<CuentaBancariaClienteResponse> cuentasBancarias = new List<CuentaBancariaClienteResponse>();
        public List<ObservacionesClienteResponse> observacionesCliente = new List<ObservacionesClienteResponse>();
        public List<ClienteOperacionesResponse> clienteOperaciones = new List<ClienteOperacionesResponse>();

    }

    public class ClienteOperacionesResponse
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


        public string fechaShort
        {
            get
            {
                if (fecha.HasValue)
                    return fecha.Value.ToString("dd/MM/yyyy");
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

        public string tipoCambioText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
               
                if (precioPactado.HasValue)
                    return precioPactado.Value.ToString(specifier, CultureInfo.InvariantCulture);
                else
                    return "0.0";
            }
        }

        public string montoEnviaText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                
                if (monto.HasValue)
                    return monto.Value.ToString(specifier, CultureInfo.InvariantCulture);
                else
                    return "0.0";
            }
        }
        //public string cambioVenta { get; set; }
        public string montoRecibeText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                
                if (montoRecibe.HasValue)
                    return montoRecibe.Value.ToString(specifier, CultureInfo.InvariantCulture);
                else
                    return "0.0";
            }
        }

    }






    public class ClienteRegsitradosResponse
    {
        public int sid { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public string tipoCliente { get; set; }
        public string nombre { get; set; }
        public string tipoDocumento { get; set; }

        public string nroDocumento { get; set; }
        public string departamento{get;set;}
        public string nombreEstado{get;set;}
        public byte estado { get; set; }
        public string fechaReg
        {
            get
            {
                if (fechaRegistro.HasValue)
                    return fechaRegistro.Value.ToString("dd/MM/yyyy");
                else
                    return "";
            }
        }
        public int total { get; set; }
    }


    public class PartnerListadoResponse
    {
        public string empresa { get; set; }
        public string ruc { get; set; }
        public string contacto { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string celular { get; set; }

        public DateTime? fechaRegistro { get; set; }
        public string departamento { get; set; }
        public string nombreEstado { get; set; }
        public string fechaReg
        {
            get
            {
                if (fechaRegistro.HasValue)
                    return fechaRegistro.Value.ToString("dd/MM/yyyy");
                else
                    return "";
            }
        }
        public int total { get; set; }
    }

    public class PartnerLiquidacionResponse
    {
        public int idTransaccion { get; set; }
        public DateTime? fecha { get; set; }
        public DateTime? fechaLiquidacion { get; set; }
        public string numPartner { get; set; }
        public string partnersAdjuntado { get; set; }
        public string usuarioPartnerAp { get; set; }
        public string cliente { get; set; }
        public string usuario { get; set; }
        public string quiere { get; set; }
        public string estadoLiquidacion { get; set; }
        public string estadoNumLiq { get; set; }
        public decimal cambioVenta { get;set;}
        public decimal montoUSDCompra{get;set;}
        public decimal cambioCompra{get;set;}
        public decimal montoSolesCompra{get;set;}
        public decimal montoUSDventa{get;set;}
        
        public decimal montoSolesventa{get;set;}
        public decimal precioPactado{get;set;}
        public decimal montoUsd{get;set;}
        public decimal montoPen{get;set;}

        public string estado{get;set;}
        public string numVoucher { get; set; }
        public string estadoSubasta{get;set;}
        public string numLiquidacion { get; set; }
        public string estadoSubastaCodigo{get;set;}
        public bool checkEstado { get; set; }
        public string fechaReg
        {
            get
            {
                if (fecha.HasValue)
                    return fecha.Value.ToString("dd/MM/yyyy");
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


        public string montoUSDCompraText { get {
            CultureInfo culture;
            string specifier;
            specifier = "N2";
            culture = CultureInfo.CreateSpecificCulture("eu-ES");
            return montoUSDCompra.ToString(specifier, CultureInfo.InvariantCulture);
        } }

        public string montoUSDText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoUsd.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }

        public string montoPENText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoPen.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }

        public string precioPactadoText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return precioPactado.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }

        //public string cambioCompra { get; set; }
        public string montoSolesCompraText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoSolesCompra.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }
        public string montoUSDventaText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoUSDventa.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }
        //public string cambioVenta { get; set; }
        public string montoSolesventaText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return montoSolesventa.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }

        public string cambioVentaText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return cambioVenta.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }

        public string cambioCompraText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                return cambioCompra.ToString(specifier, CultureInfo.InvariantCulture);
            }
        }
        //public string precioPactadoText { get; set; }
        //public string montoUsd { get; set; }
        //public string montoPen { get; set; }


        public int total { get; set; }
    }


    public class ListaNotificacionesResponse
    {
        public int IdNotificacion { get; set; }
        public string IdUsuario { get; set; }
        public int? IdCliente { get; set; }
        public int? Tipo { get; set; }
        public string Url { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public byte estado { get; set; }
    }


    public class ListaCodigosLiquidacionRequest
    {
        public int nroSubasta { get; set; }
        public int estado { get; set; }
        public bool checkear { get; set; }
        public string numVoucher { get; set; }


    }

    /*Partner Lista Liquidacion*/

    public class PartnerListaAdjudicacionResponse
    {
        public int idTransaccion { get; set; }
        public DateTime? fecha { get; set; }
        public string numPartner { get; set; }
        public string partnersAdjuntado { get; set; }
        public string cliente { get; set; }
        public string quiere { get; set; }
        public string estadoLiquidacion { get; set; }
        public string estadoNumLiq { get; set; }
        public decimal? precioPactado { get; set; }
        public decimal? montoUsd { get; set; }
        public decimal? montoPen { get; set; }

        public string estado { get; set; }
        public string numVoucher { get; set; }
        public string estadoSubasta { get; set; }
        public string numLiquidacion { get; set; }
        public string estadoSubastaCodigo { get; set; }
        public bool checkEstado { get; set; }

        public string tipoCambioText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                if (precioPactado.HasValue)
                    return precioPactado.Value.ToString(specifier, CultureInfo.InvariantCulture);
                else
                    return "0.0";
            }
        }

        public string montoUSDventaText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
                if (montoUsd.HasValue)
                    return montoUsd.Value.ToString(specifier, CultureInfo.InvariantCulture);
                else
                    return "0.0";
            }
        }
        //public string cambioVenta { get; set; }
        public string montoSolesventaText
        {
            get
            {
                CultureInfo culture;
                string specifier;
                specifier = "N2";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");
               
                if (montoPen.HasValue)
                    return montoPen.Value.ToString(specifier, CultureInfo.InvariantCulture);
                else
                    return "0.0";
            }
        }

        public string fechaReg
        {
            get
            {
                if (fecha.HasValue)
                    return fecha.Value.ToString("dd/MM/yyyy");
                else
                    return "";
            }
        }

        public string HoraConfirmacion
        {
            get
            {
                if (fecha.HasValue)
                    return fecha.Value.ToString("HH:mm");
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


        //public string montoUSDCompraText
        //{
        //    get
        //    {
        //        CultureInfo culture;
        //        string specifier;
        //        specifier = "N3";
        //        culture = CultureInfo.CreateSpecificCulture("eu-ES");
        //        return montoUSDCompra.ToString(specifier, CultureInfo.InvariantCulture);
        //    }
        //}
        //public string cambioCompra { get; set; }
 
        //public string precioPactadoText { get; set; }
        //public string montoUsd { get; set; }
        //public string montoPen { get; set; }


        public int total { get; set; }
    }




}
