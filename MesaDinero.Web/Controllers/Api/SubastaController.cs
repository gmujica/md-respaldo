using System;
using System.Collections.Generic;
using System.Linq;
using MesaDinero.Domain;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using MesaDinero.Domain.Model;
using System.Threading.Tasks;
using MesaDinero.Domain.DataAccess;
using MesaDinero.Domain.Model.operaciones;
using MesaDinero.Domain.DataAccess.operaciones;

namespace MesaDinero.Web.Controllers.Api
{
    [RoutePrefix("api")]
    public class SubastaController : ApiBaseController
    {

        [HttpPost]
        [Route("subasta-datos-fideicomiso")]
        public IHttpActionResult listarDatosFideicomiso( string codBanco)
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<FideicomisoResponse> result = _registroDataAccess.ListarDatosFideicomiso(codBanco);
            return Ok(result);
        }

        [HttpPost]
        [Route("subasta-lista-comparar-proveedor")]
        public IHttpActionResult ListaCompararSubasta(SubastaCompararProveedorRequest model)
        {
            SubastaClienteDataAccess _operadorDataAccess = new SubastaClienteDataAccess();
            BaseResponse<List<SubastaCompararProveedorResponse>> result = new BaseResponse<List<SubastaCompararProveedorResponse>>();
            result = _operadorDataAccess.ListaSubastaComparar(model);
            return Ok(result);
        }


        [HttpPost]
        [Route("subasta-lista-operacion")]
        public IHttpActionResult ListaOperacionSubasta(PageResultParam model)
        {
            SubastaClienteDataAccess _operadorDataAccess = new SubastaClienteDataAccess();
            PageResultSP<ListaOperacionSubastaCliente> result = new PageResultSP<ListaOperacionSubastaCliente>();
            result = _operadorDataAccess.ListaOperacionesCliente(model, IdCurrenCliente);
            return Ok(result);
        }

        [HttpPost]
        [Route("subasta-verificar-bloqueo")]
        public IHttpActionResult subastaVerificarCliente()
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<bool> result = _registroDataAccess.VerificarCliente(IdCurrenCliente);
            return Ok(result);
        }

        [HttpPost]
        [Route("lista-notificaciones")]
        public IHttpActionResult listaNotificacion()
        {
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
            BaseResponse<List<ListaNotificacionesResponse>> result = _registroDataAccess.ListaNotificaciones(IdCurrenCliente);
            return Ok(result);
        }

        [HttpPost]
        [Route("subasta-instruccion-pago")]
        public IHttpActionResult operacionesInstruccionPago(SubastaRequest model)
        {
            OperacionDataAccess _operador = new OperacionDataAccess();

            var resultado = _operador.verListadoInstruccion(model);

            return Ok(resultado);
        }


        [HttpPost]
        [Route("subasta-estado-verifica")]
        public IHttpActionResult ListaCompararSubasta(SubastaRequest model)
        {
            SubastaClienteDataAccess _operadorDataAccess = new SubastaClienteDataAccess();
            BaseResponse<Subasta_Verificacion_Response> result = new BaseResponse<Subasta_Verificacion_Response>();
            result = _operadorDataAccess.getSubastaEstadoVerificia(model);
            return Ok(result);
        }
    }
}

