using MesaDinero.Domain;
using MesaDinero.Domain.DataAccess.Registro;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class RegistroController : ApiBaseController
    {

        [HttpPost]
        [Route("registro-datos-fideicomiso")]
        public IHttpActionResult RegistroDatosPrincipalesEmpresa(PersonaJuridicaFideicomisoRegistroRequest model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = _registroDataAccess.registrarPersonaFideicomiso(model);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("listar-datos-empresa")]
        //public IHttpActionResult RegistroDatosPrincipalesEmpresa(PersonaJuridicaFideicomisoRegistroRequest model)
        //{
        //    Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
        //    BaseResponse<string> result = _registroDataAccess.registrarPersonaFideicomiso(model);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("listar-datos-empresa")]
        public IHttpActionResult ListarDatosEmpresa()
        {
            Domain.DataAccess.RegistroCliente _operadorDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<EmpresaRegistroRequest> result = new BaseResponse<EmpresaRegistroRequest>();

            result = _operadorDataAccess.ListarDatosEmpresa(NroRucEmpresaCurrenUser);
            return Ok(result);
        }


        [HttpPost]
        [Route("listar-datos-cuentas-bancarias")]
        public IHttpActionResult ListarCuentasBancarias()
        {
            Domain.DataAccess.RegistroCliente _operadorDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<EmpresaRegistroRequest> result = new BaseResponse<EmpresaRegistroRequest>();

            result = _operadorDataAccess.ListarDatosCuentasBancarias(NroRucEmpresaCurrenUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("modificar-datos-empresa")]
        public IHttpActionResult ModificarDatosEmpresa(EmpresaRegistroRequest model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = _registroDataAccess.EditarDatosPersonaJuridica(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("modificar-cuentas-bancarias")]
        public IHttpActionResult ModificarCuentasBancarias(EmpresaRegistroRequest model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = _registroDataAccess.EditarDatosCuentaBancarias(model, NroRucEmpresaCurrenUser);
            return Ok(result);
        }

        /*Perfil*/

        [HttpPost]
        [Route("listado-perfil")]
        public IHttpActionResult getAllPerfil(PageResultParam model)
        {
            PerfilDataAccess _cargoDataAccess = new PerfilDataAccess();
            PageResultSP<PerfilResponse> result = new PageResultSP<PerfilResponse>();

            result = _cargoDataAccess.getAllPerfil(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("new-perfil")]
        public IHttpActionResult addNewPerfil(PerfilRequest model)
        {

            PerfilDataAccess _cargoDataAccess = new PerfilDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _cargoDataAccess.insertNewPerfil(model, NroDocumentoCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("edit-perfil")]
        public IHttpActionResult editCargo(PerfilRequest model)
        {

            PerfilDataAccess _cargoDataAccess = new PerfilDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _cargoDataAccess.EditarPerfil(model, NroDocumentoCurrenUser);

            return Ok(result);
        }


        [HttpPost]
        [Route("eliminar-perfil")]
        public IHttpActionResult eliminarCargo(PerfilRequest model)
        {

            PerfilDataAccess _cargoDataAccess = new PerfilDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _cargoDataAccess.EliminarPerfil(model, NroDocumentoCurrenUser);

            return Ok(result);
        }



        /*Usuario*/

        [HttpPost]
        [Route("listado-usuario")]
        public IHttpActionResult getAllUsuario(PageResultParam model)
        {
            UsuarioDataAccess _usuarioDataAccess = new UsuarioDataAccess();
            PageResultSP<UsuarioResponse> result = new PageResultSP<UsuarioResponse>();

            result = _usuarioDataAccess.getAllUsuario(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("new-usuario")]
        public async Task<IHttpActionResult> addNewUsuario(UsuarioRequest model)
        {

            UsuarioDataAccess _usuarioDataAccess = new UsuarioDataAccess();
            BaseResponse<string> result;
            result = await _usuarioDataAccess.insertUsuario(model, NroDocumentoCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("edit-usuario")]
        public IHttpActionResult editUsuario(UsuarioRequest model)
        {

            UsuarioDataAccess _usuarioDataAccess = new UsuarioDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _usuarioDataAccess.EditarUsuario(model, NroDocumentoCurrenUser);

            return Ok(result);
        }


        [HttpPost]
        [Route("eliminar-usuario")]
        public IHttpActionResult eliminarUsuario(UsuarioRequest model)
        {

            UsuarioDataAccess _usuarioDataAccess = new UsuarioDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _usuarioDataAccess.EliminarUsuario(model, NroDocumentoCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("new-perfil-usuario")]
        public IHttpActionResult addPerfilUsuario(List<UsuarioPerfilRequest> model)
        {

            UsuarioDataAccess _usuarioDataAccess = new UsuarioDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _usuarioDataAccess.insertPerfilUsuario(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("listado-perfil-usuario")]
        public IHttpActionResult getAllPerfilUsuario(PageResultParam model)
        {
            PerfilDataAccess _cargoDataAccess = new PerfilDataAccess();
            PageResultSP<PerfilResponse> result = new PageResultSP<PerfilResponse>();

            result = _cargoDataAccess.getAllPerfilUsuario(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("recuperar-password")]
        public IHttpActionResult RecuperarMiPassword(dynamic model)
        {
            string email = model.email;
            Domain.DataAccess.ClienteDataAccess _clienteDataAccess = new Domain.DataAccess.ClienteDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _clienteDataAccess.sendCorreoRecuperarPasswordAdmin(email);

            return Ok(result);
        }

        [HttpPost]
        [Route("cambiar-password-recuperacion")]
        public IHttpActionResult CambiarPasswordRecuperacion(dynamic model)
        {
            string sid = model.sid;
            string password = model.clave;

            BaseResponse<string> result = new BaseResponse<string>();
            Domain.DataAccess.ClienteDataAccess _clienteDataAccess = new Domain.DataAccess.ClienteDataAccess();

            result = _clienteDataAccess.CambioPasswordXCorreoAdmin(sid, password);

            return Ok(result);
        }


    }
}
