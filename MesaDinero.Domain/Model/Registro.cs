using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Model
{

    public class RegistroHeaderMenu
    {
        public int estadoNavegacion { get; set; }
        public string seguimiento { get; set; }
        public string secredId { get; set; }
        public int tipoCliente { get; set; }
    }


    public class RegistroPassWpord_Response2
    {
        public string email { get; set; }
        public string NombreCliente { get; set; }
        public string vNroDocumento { get; set; }
        public int IdUsuario { get; set; }
        public int IdCliente { get; set; }
        public int TipoCliente { get; set; }
        public string Iniciales { get; set; }
        public string RucEmpresa { get; set; }

    }


    public class RegistroPassWpord_Response 
    {
        public string ReturnUrl { get; set; }
        public string sid { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int cliente { get; set; }
        public int tipoCliente { get; set; }
        public string nroDocumento { get; set; }
        public string empresa { get; set; }
    }

    public partial class RegistroCrearPassWord_Request
    {
        public string email { get; set; }
        public string eror { get; set; }
        public string sid { get; set; }
        public string password { get; set; }
        public string rePassword { get; set; }
        public int opcionUsuario { get; set; }
    }

    public partial class EntidadFinancieraResponse 
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string icon { get; set; }
        public string formatoCCI { get; set; }
        public string formatoNroCuenta { get; set; }
    }

    public partial class ModificaionClienteResponse
    {
        public int idCliente { get; set; }
        public int idPreCliente { get; set; }
        public int? CodigoModificacionDatos { get; set; }
        public int? CodigoModificaionCuentasBancarias { get; set; }
        public int tipoCliente { get; set; }

        public string seguimiento { get; set; }
    }


    public partial class RegistroResponse
    {
        public RegistroResponse()
        {
            editar = false;
            header = new RegistroHeaderMenu();
            partners = new List<string>();
        }

        public bool editar { get; set; }
        public int tiempoSubasta { get; set; }
       
        public string sid { get; set; }
        public int tipoCliente { get; set; }
        public string nroDocumento { get; set; }
        public string nombres { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public DateTime? fdatosBasicos { get; set; }
        public DateTime? fdatosBancarios { get; set; }
        public string userName { get; set; }
        public string estado { get; set; }
        public string comentario { get; set; }
        public string seguimiento { get; set; }
        public RegistroHeaderMenu header { get; set; }
        public List<string> partners { get; set; }
        public int opcionUsuario { get; set; }

    }


    public class PerfilResponse
    {
        public int codigo { get; set; }
        public string nombre { get; set; }
        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }
        public bool checkActivo { get; set; }

    }

    public class PerfilRequest
    {
        public int codigo { get; set; }
        public string nombre { get; set; }
        public byte estado { get; set; }

    }

    public class UsuarioResponse
    {
        public int codigo { get; set; }
        public string email { get; set; }
        public string tipoUsuario { get; set; }
        public string rucEmpresa { get; set; }
        public string razonSocial { get; set; }
        public string nroDocumento { get; set; }
        public string tipoDocumento { get; set; }
        public string nombre { get; set; }
        public string apellidoPat { get; set; }
        public string apellidoMat { get; set; }
        public string preCelular { get; set; }
        public string celular { get; set; }
        public DateTime? fecha { get; set; }
        public byte estado { get; set; }
        public int total { get; set; }
        public string nombreEstado { get; set; }
    }

    public class UsuarioRequest
    {
        public int codigo { get; set; }
        public string tipoDocumento { get; set; }
        public string nombre { get; set; }
        public string apellidoPat { get; set; }
        public string apellidoMat { get; set; }
        public string prefijo { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string tipoUsuario { get; set; }
        public string rucEmpresa { get; set; }
        public string nroDocumento { get; set; }
        public string fecha { get; set; }
        public byte estado { get; set; }

    }
    public partial class ModificarPassWord_Request
    {
        public string sid { get; set; }
        public string passwordActual { get; set; }
        public string password { get; set; }
        public string rePassword { get; set; }
    }

    public class UsuarioPerfilRequest
    {
        public int codigoPerfil { get; set; }
        public int codigoUsuario { get; set; }
    }

    public class CambioPassWordAdmRequest
    {
        public string email { get; set; }
        public string sid { get; set; }
    }
}
