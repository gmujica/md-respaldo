using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MesaDinero.Domain
{

    public class PaginationResult
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public KeyValuePair<int, bool>[] numbersPages { get; set; }
        public int itemperpage { get; set; }
        public int total { get; set; }
        public int pageCount { get; set; }
    }

    public static class Utilities
    {
        public static PaginationResult ResultadoPagination(int pageNo, int pageSize, int totalRecordCount)
        {
            PaginationResult result = new PaginationResult();

            var limit = totalRecordCount <= (pageNo * pageSize) ? totalRecordCount : (pageNo * pageSize);
            var pageCount = totalRecordCount > 0 ? (int)Math.Ceiling(totalRecordCount / (double)pageSize) : 0;
            var numberPages = GetNumberPages(pageNo, pageCount);

            result.offset = ((pageNo - 1) * pageSize) + 1;
            result.limit = limit;
            result.page = pageNo;
            result.itemperpage = pageSize;
            result.total = totalRecordCount;
            result.pageCount = pageCount;
            result.numbersPages = numberPages;

            return result;
        }

        private static KeyValuePair<int, bool>[] GetNumberPages(int pageNo, int pageCount)
        {
            var numberPages = new Dictionary<int, bool>();
            var startPage = (int)(Math.Ceiling((decimal)pageNo / 10 - 1) * 10) + 1;
            var endPage = (int)Math.Min(startPage + 9, pageCount);
            for (int i = startPage; i <= endPage; i++)
            {
                numberPages.Add(i, i == pageNo);
            }
            return numberPages.ToArray();
        }


    }

    public static class RolPersonaEmpresa
    {
        public const string RepresentanteLegal = "RL";
        public const string Accionista = "ACC";
        public const string Autorizado = "AUTH";
    }

    public static class EstadoRegistroTabla
    {
        public const byte Activo = 1;
        public const byte NoActivo = 2;
        public const byte PorVerificar = 3;
        public const byte Observado = 4;
        public const byte Eliminado = 9;
    }

    public static class TipoCliente
    {
        public const int PersonaNatural = 1;
        public const int PersonaJuridica = 2;
    }

    public static class Tiempos
    {
        public const string ConfirmacionSms = "T_CSmS";
    }

    public static class TipoRegistroPreCliente
    {
        public const string NuevoRegistro = "A";
        public const string ModificacionDatosBasicos = "B";
        public const string ModificacionDatosBancarios = "C";
    }

    public static class SeguimientoRegistro
    {
        public const string IngresoClaveSMS = "IGMSM";
        public const string RegistroBatosPrincipales = "RDPR";
        public const string RegistroPersonaAutorizada = "RPATH";
        public const string RegistroDatosBancarios = "RDB";
        public const string PreProcesoValidacion = "PPV";
        public const string RegistroProcesoValidacion = "RPV";
        public const string CrearPassword = "CPW";
        public const string RecuperarPassword = "RPW";
        public const string Finalizado = "FNZ";
        public const string PostCrearPasswords = "NUP";
    }

    public enum EstadoRegistroCliente
    {
        preRegistro = 0,
        ingresoDeSmS = 1,
        datosBasicos = 2,
        datosBancarios = 3,
        creacionPassword = 4,
    }

    public static class EstadoSubasta
    {
        public const string Abandonada = "Z";
        public const string Activa = "A";
        public const string Confirmada = "C";
        public const string PendientePago = "D";
        public const string PagadaXCliente = "E";
        public const string PendienteVerificacion = "F";
        public const string PagoClienteVerificacion = "G";
        public const string OperacionObservada = "H";
        public const string OperacionIncumplida = "Y";
        public const string PagadaALCliente = "I";
    }

    public static class TipoUsuario
    {
        public const string Lmd = "LMD";
        public const string Fideicomiso = "FID";
        public const string Partner = "PAR";
        public const string Canal = "CAN";

    }

    public static class ErroresValidacion 
    {
        public const string ClienteNoExiste = "No se encuentran los datos de el cliennte.";
    }

    public class BaseResponse<T>
    {
        public T data { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
        public Exception ex { get; set; }
        public string other { get; set; }
        public int other2 { get; set; }
    }

    public class PageResultParam
    {
        public int pageIndex { get; set; }
        public int itemPerPage { get; set; }
        public int idFilter { get; set; }
        public string filter { get; set; }
        public string textFilter { get; set; }
        public string searchFilter { get; set; }

    }
    
    public class PageResultSP<T>
    {
        public List<T> data = new List<T>();

        public int offset { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public KeyValuePair<int, bool>[] numbersPages { get; set; }
        public int itemperpage { get; set; }
        public int? total { get; set; }
        public int PageCount { get; set; }
        public string error { get; set; }
        public bool success { get; set; }

    }

    public static class MesaDineroHelper
    {
        public static string getNombreCliente(MesaDinero.Data.PersistenceModel.Tb_MD_Pre_Clientes cliente)
        {
            string result = string.Empty;

            if(cliente.vTipoCliente == TipoCliente.PersonaNatural)
            {
                string apellido = string.Empty, nombre = string.Empty;

                if (cliente.vNombre.Split(' ').Length > 0)
                    nombre = cliente.vNombre.Split(' ')[0];
                else
                    nombre = cliente.vNombre;

                if(cliente.vApellido.Split(' ').Length > 1)                
                    apellido = cliente.vApellido.Split(' ')[0];                
                else                
                    apellido = cliente.vApellido;
                
                //result = string.Format("{0} {1}",nombre,apellido);
                result = string.Format("{0}", cliente.vNombre);
            }
            else
            {
                result = cliente.nombreEmpresa;
            }

            return result;
        }

        public static string getIniciales(MesaDinero.Data.PersistenceModel.Tb_MD_Pre_Clientes cliente)
        {
            string result = string.Empty;

            if(cliente.vTipoCliente == TipoCliente.PersonaNatural)
            {
                //result = string.Format("{0}{1}",cliente.vNombre.Substring(0,1),cliente.vApellido.Substring(0,1));
                result = string.Format("{0}", cliente.vNombre.Substring(0, 1));
            }
            else
            {
                result = string.Format("{0}", cliente.nombreEmpresa.Substring(0,1));
            }

            return result.ToUpper();
        }

        public static bool hashLetter(string text)
        {
            text = text.ToLower();
            string filtro = "0,1,2,3,4,5,6,7,8,9,☺.☻,♥,♦,♣,♠,•,◘,○,◙,♂,♀,♫,☼,►,◄,↕,‼,¶,§,▬,↨,↑,↓,→,←,∟,↔,▲,▼, ,!,#,$,%,&,',(,),*,+,-,.,/,:,;,<,=,>,?,@,[,],^,_,`,{,|,},~,⌂,Ç";
            
            foreach (var item in filtro.Split(','))
                text = text.Replace(item,string.Empty);

            return text.Length > 0;
        }

        public static bool hashNumber(string text)
        {
            text = text.ToLower();
            string filtro = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,ñ,o,p,q,r,s,t,u,w,y,z,☺.☻,♥,♦,♣,♠,•,◘,○,◙,♂,♀,♫,☼,►,◄,↕,‼,¶,§,▬,↨,↑,↓,→,←,∟,↔,▲,▼, ,!,#,$,%,&,',(,),*,+,-,.,/,:,;,<,=,>,?,@,[,],^,_,`,{,|,},~,⌂,Ç";
            foreach (var item in filtro.Split(','))
                text = text.Replace(item, string.Empty);

            return text.Length > 0;
        }


    }

    #region Envio MsM

    public static class SendMsMPhone
    {
        public static void VerificacionMsM_RegistroBasico(string phone,string claveSMS)
        {
            string accountSid = ConfigurationManager.AppSettings["TwillioSid"]; //"AC586fb9502cebc49b63ee601315776982";
            string authToken = ConfigurationManager.AppSettings["TwillioToken"]; // "ecd31a54fcad4c6a449582867a7fc332";
            TwilioClient.Init(accountSid, authToken);
            try
            {
                var message = MessageResource.Create(
                body: string.Format("La Mesa De Dinero -  Su código de verificación es : {0}", claveSMS),
                from: new Twilio.Types.PhoneNumber(ConfigurationManager.AppSettings["TwillioNumber"]), // +19166190520
                to: new Twilio.Types.PhoneNumber(string.Format("+51{0}",phone))
                );                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    #endregion


}
