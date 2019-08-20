using System;
using System.Collections.Generic;
using System.Linq;
using MesaDinero.Domain;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.DataAccess.Admin;
using MesaDinero.Domain.DataAccess.operaciones;
using MesaDinero.Domain.Model.operaciones;

namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class OConfirmadasTController : ApiBaseController
    {
        [HttpPost]
        [Route("operaciones-confirmadasT")]
        public IHttpActionResult operacionesConfirmadas(PageResultParam model)
        {
            OperadorDataAccess _operador = new OperadorDataAccess();

            var resultado = _operador.traerOperacionesConfirmadasRegistradosT(model);

            return Ok(resultado);
        }

        [HttpPost]
        [Route("operaciones-verificar-pago")]
        public IHttpActionResult ListaOperacionPago(PageResultParam model)
        {
            OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
            PageResultSP<VerificacionPagoResponse> result = new PageResultSP<VerificacionPagoResponse>();

            result = _operadorDataAccess.ListaVerificarPago(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("operaciones-lista-generar-pago")]
        public IHttpActionResult ListaGenerarPago(FiltroOperacionParam model)
        {
            OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
            PageResultSP<ListaGenerarPagoResponse> result = new PageResultSP<ListaGenerarPagoResponse>();

            result = _operadorDataAccess.ListaGenerarPago(model);
            return Ok(result);
        }

        [HttpPost]
        [Route("operaciones-actualiza-estado")]
        public IHttpActionResult actualizaVerificarPago(List<SubastaRequest> model)
        {
            
            OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();

            result = _operadorDataAccess.actualizarEstadoVerificado(model, NroDocumentoCurrenUser);
            return Ok(result);

        }

        //[HttpPost]
        //[Route("operaciones-generar-txt")]
        //public IHttpActionResult generarTxt(SubastaRequest model)
        //{
        //    OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
        //    BaseResponse<string> result = new BaseResponse<string>();

        //    result = _operadorDataAccess.generarTxt(model);
        //    return Ok(result);
        //}

        [HttpPost]
        [Route("operaciones-instruccion-pago")]
        public IHttpActionResult operacionesInstruccionPago(SubastaRequest model)
        {
            OperacionDataAccess _operador = new OperacionDataAccess();

            var resultado = _operador.verListadoInstruccion(model);

            return Ok(resultado);
        }


        [HttpPost]
        [Route("common/getEntidadBancaria")]
        public IHttpActionResult getBancos()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();
            result = _commonDataAccess.getAllEntidadBancaria();

            return Ok(result);
        }

        [HttpPost]
        [Route("common/getTipoMoneda")]
        public IHttpActionResult getMonedas()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();
            result = _commonDataAccess.getAllTipoMoneda();

            return Ok(result);
        }

        [HttpPost]
        [Route("operaciones-lista-aprobar-pago")]
        public IHttpActionResult ListaParaAprobarPago(PageResultParam model)
        {
            OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
            PageResultSP<ListaAprobarPagoResponse> result = new PageResultSP<ListaAprobarPagoResponse>();

            result = _operadorDataAccess.ListaAprobarPago(model);
            return Ok(result);
        }

    }


}

