using System;
using System.Collections.Generic;
using System.Linq;
using MesaDinero.Domain;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using MesaDinero.Domain.Model;
using System.Threading.Tasks;

namespace MesaDinero.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class RegistroController : ApiBaseController
    {

        [HttpPost]
        [Route("registro-reanudar")]
        public IHttpActionResult ReanudarRegistro(dynamic model)
        {
            ReanudarRegistroRequest mModel = new ReanudarRegistroRequest();
            mModel.email = model.email;
            mModel.nroDocumento = model.nroDocumento;

            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = new BaseResponse<string>();

            result = _registroDataAccess.reanudarRegistro(mModel);

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/basico")]
        public IHttpActionResult RegistroClienteBasico( dynamic model)
        {
            try
            {
                Domain.DataAccess.RegistroCliente registro = new Domain.DataAccess.RegistroCliente();
                BaseResponse<string> result = registro.crearCuenta(new RegistroClientesRequest
                {
                    nombre = model.nombre,
                    apellido = model.apellido,
                    email = model.email,
                    phone = model.celular,
                    tipoPersona = model.tipoPersona,
                    nroDocumento = model.nroDocumento,
                    nroDocumentoContacto = model.nroDocumentoContacto,
                    nombreEmpresa = model.nombreEmpresa
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                BaseResponse<string> result = new BaseResponse<string>();
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.ex = ex;
                return Ok(result);
            }
            
        }
        [HttpPost]
        [Route("registro/enviarMsM/{sid}")]
        public IHttpActionResult SendMsMRegistroBasico(string sid)
        {
            Domain.DataAccess.RegistroCliente registro = new Domain.DataAccess.RegistroCliente();
            Guid secredID = Guid.Parse(sid);
            BaseResponse<string> result = registro.generarSMSCliente(secredID);

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/validar-sms")]
        public IHttpActionResult ValidarSMSRegistro(dynamic model)
        {
            Domain.DataAccess.RegistroCliente registro = new Domain.DataAccess.RegistroCliente();

            BaseResponse<string> result = registro.validarSMS(new ValiarSMSRequest { 
             sid = model.sid,
             clavemsm = model.clavemsm
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/datospersona")]
        public IHttpActionResult RegistroDatosPrincipalesPersona(dynamic model)
        {
            Domain.DataAccess.RegistroCliente registro = new Domain.DataAccess.RegistroCliente();

            BaseResponse<string> result = registro.registroPeronaNatural(new PersonaNatutalRequest
            {
                sid = model.sid,
                tipoDocumento = model.tipoDocumento,
                nroDocumento = model.nroDocumento,
                nombres = model.nombres,
                apePaterno = model.apePaterno,
                apeMaterno = model.apeMaterno,
                fnDia = model.fnDia,
                fnMes = model.fnMes,
                fnAnio = model.fnAnio,
                email = model.email,
                preCelular = model.preCelular,
                celular = model.celular,
                pais = model.pais,
                departamento = model.departamento,
                provincia = model.provincia,
                distrito = model.distrito,
                direccion = model.direccion,
                origenFondos = model.origenFondos,
                expuesto = model.expuesto,
                sictuacionLaboral = model.sictuacionLaboral,
                entidadNombreExpuesto = model.entidadNombreExpuesto,
                cargoExpuesto = model.cargoExpuesto
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/enviar-validacion")]
        public IHttpActionResult enviarValidacion(GetDatosPersonaNatural model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = new BaseResponse<string>();

            result = _registroDataAccess.enviarAprobacionRegistroCliente(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/getdatosBancarios-Registro")]
        public IHttpActionResult getDatosBancariosRegistro(GetDatosPersonaNatural model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<List<CuentaBancariaClienteResponse>> result = new BaseResponse<List<CuentaBancariaClienteResponse>>();

            result =  _registroDataAccess.getDatosBancariosXRegistro(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("registro/datoBancario")]
        public IHttpActionResult RegistrarDatosBancarios(DatoBancariaCliente_Request model)
        {

            Domain.DataAccess.RegistroCliente registro = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = registro.registroDatosBancarios(model);

            return Ok(result);
        }
        //PersonaJuridicaRegistroRequest model
        [HttpPost]
        [Route("registro/datosemperesa")]
        public IHttpActionResult RegistroDatosPrincipalesEmpresa()
        {
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            int indice = Convert.ToInt16(System.Web.HttpContext.Current.Request.Params["indice"]);
            System.Web.HttpPostedFile file = null;

            if (indice > 0)
            {
                file = files[0];
            }

            var json = System.Web.HttpContext.Current.Request.Params["jsData"];

            PersonaJuridicaRegistroRequest model = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<PersonaJuridicaRegistroRequest>(json);

            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();

            BaseResponse<string> result = _registroDataAccess.registrarPersonaJuridica(model,file);

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/empresa-personaautorizada")]
        public IHttpActionResult RegistrarPersonaAutorizadaEmpresa(PersonaAutorizadaRequest model)
        {
            Domain.DataAccess.RegistroCliente _regsitroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = _regsitroDataAccess.registrarPersonaAutorizada(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("registro/crear-password")]
        public IHttpActionResult CreacionPassword(RegistroCrearPassWord_Request model)
        {
            Domain.DataAccess.RegistroCliente _regsitroDataAccess = new Domain.DataAccess.RegistroCliente();
            //BaseResponse<RegistroPassWpord_Response> result = _regsitroDataAccess.CrearPasswprd(model);



            return Ok();

        }

        [HttpPost]
        [Route("registro/getdatosPersona-init")]
        public async Task<IHttpActionResult> GetDatosPersonaInit(GetDatosPersonaNatural model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();

            result = await _registroDataAccess.getDatosPersonaNaturalRegistroInit(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("registro/getdatosPersona-registro")]
        public IHttpActionResult GetDatosPersonaXRegistro(GetDatosPersonaNatural model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();

            result =  _registroDataAccess.getDatosPersonaNaturalXRegistro(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("recuperar-password")]
        public IHttpActionResult RecuperarMiPassword(dynamic model)
        {
            string email = model.email;

            Domain.DataAccess.ClienteDataAccess _clienteDataAccess = new Domain.DataAccess.ClienteDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();

            result = _clienteDataAccess.sendCorreoRecuperarPassword(email);

  
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

            result = _clienteDataAccess.CambioPasswordXCorreoExterno(sid, password);

            return Ok(result);
        }

        /*Listado Empresa*/
        [HttpPost]
        [Route("listar-datos-cuentas-bancarias_cliente")]
        public IHttpActionResult ListarCuentasBancarias()
        {
            Domain.DataAccess.RegistroCliente _operadorDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<EmpresaRegistroRequest> result = new BaseResponse<EmpresaRegistroRequest>();

            result = _operadorDataAccess.ListarDatosCuentasBancarias_Clientes(IdCurrenCliente);
            return Ok(result);
        }

        [HttpPost]
        [Route("modificar-cuentas-bancarias-cliente")]
        public IHttpActionResult ModificarCuentasBancariasCliente(EmpresaRegistroRequest model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = _registroDataAccess.EditarDatosCuentaBancariasCliente(model, IdCurrenCliente, Convert.ToInt16(TipoCurrenUser));
            return Ok(result);
        }

        [HttpPost]
        [Route("listar-datos-empresa")]
        public IHttpActionResult ListarDatosEmpresa()
        {
            Domain.DataAccess.RegistroCliente _operadorDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<EmpresaRegistroRequest> result = new BaseResponse<EmpresaRegistroRequest>();

            result = _operadorDataAccess.ListarDatosEmpresaCliente(NroRucEmpresaCurrenUser, TipoCurrenUser);
            return Ok(result);
        }

        [HttpPost]
        [Route("modificar-datos-empresa")]
        public IHttpActionResult ModificarDatosEmpresa(EmpresaRegistroRequest model)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<string> result = _registroDataAccess.EditarDatosPersonaJuridicaCliente(model, IdCurrenCliente, AbreviaturaCurrenUser);
            return Ok(result);
        }


    }
}

