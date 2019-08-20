using System;
using System.Collections.Generic;
using System.Linq;
using MesaDinero.Domain;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.DataAccess.Admin;

namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class OperadorController : ApiBaseController
    {
        [Route("operador/registro-clientes-all")]
        [HttpPost]
        public IHttpActionResult getAllRegistroUsuarios(PageResultParam model)
        {
            OperadorDataAccess _operadorDataAccess = new OperadorDataAccess();
            PageResultSP<ClienteRegsitradosOperador> result = new PageResultSP<ClienteRegsitradosOperador>();

            result = _operadorDataAccess.getAllClientesRegistradosForOperado(model);


            return Ok(result);
        }

     
        [HttpPost]
        [Route("common/datosPersonaAll")]
        public IHttpActionResult getAllDatosPersona(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaNatutalAllResponse> result = new BaseResponse<PersonaNatutalAllResponse>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();

            result = _operadorDataAccess.getDatosPrePersonaNaturalAll(model);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/datosClienteRegistradoAll")]
        public IHttpActionResult getAllDatosClienteRegistrado(GetDatosPersonaNatural model)
        {
            BaseResponse<ClientePersonaNatutalAllResponse> result = new BaseResponse<ClientePersonaNatutalAllResponse>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();

            result = _operadorDataAccess.getDatosClienteRegistradosAll(model);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/datosPerJuridicaXRegistroValidacion")]
        public IHttpActionResult getDatosPerJuridicaXValidacion(GetDatosPersonaNatural model)
        {
               BaseResponse<PersonaJuridicaAllResponse> result = new BaseResponse<PersonaJuridicaAllResponse>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();

            result = _operadorDataAccess.getDatosPersonaJuridicaAllXValidacion(model);
            

            return Ok(result);
        }

        [HttpPost]
        [Route("common/datosJuridicaAll")]
        public IHttpActionResult getAllDatosEmpresa(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaAllResponse> result = new BaseResponse<PersonaJuridicaAllResponse>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();

            result = _operadorDataAccess.getDatosPersonaJuridicaAll(model);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/datosClienteRegistrarJuridicaAll")]
        public IHttpActionResult getAllDatosClienteRegistradoEmpresa(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaAllResponse> result = new BaseResponse<PersonaJuridicaAllResponse>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();
            result = _operadorDataAccess.getDatosClienteJuridicaAll(model);
            return Ok(result);
        }


        [HttpPost]
        [Route("operador/validarCliente")]
        public IHttpActionResult validarCllineteForOperador(validarModelRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();

            result = _operadorDataAccess.ValidarCuentaClienteForOperador(model.estado, model.sid, model.observacion, IdCurrenUser);


            return Ok(result);
        }


        [HttpPost]
        [Route("operador/validarClienteFideicomizo")]
        public IHttpActionResult validarCllineteForFideicomizo(validarModelRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            Domain.DataAccess.Admin.OperadorDataAccess _operadorDataAccess = new Domain.DataAccess.Admin.OperadorDataAccess();

            result = _operadorDataAccess.ValidarCuentaClienteForFideicomiso(model.estado, model.sid, model.observacion, IdCurrenUser);

            return Ok(result);
        }

        [Route("operador/lista-clientes-registrado")]
        [HttpPost]
        public IHttpActionResult getListadoClientes(PageResultParam model)
        {
            OperadorDataAccess _operadorDataAccess = new OperadorDataAccess();
            PageResultSP<ClienteRegsitradosResponse> result = new PageResultSP<ClienteRegsitradosResponse>();

            result = _operadorDataAccess.getListadoClientesRegistrados(model);


            return Ok(result);
        }

        [Route("operador/lista-partners")]
        [HttpPost]
        public IHttpActionResult getListadoPartners(PageResultParam model)
        {
            OperadorDataAccess _operadorDataAccess = new OperadorDataAccess();
            PageResultSP<PartnerListadoResponse> result = new PageResultSP<PartnerListadoResponse>();
            result = _operadorDataAccess.getListadoPartnerAll(model);
            return Ok(result);
        }

        [Route("operador/lista-liquidacion-partners")]
        [HttpPost]
        public IHttpActionResult getListadoLiquidacionPartners(PageResultParam model)
        {
            OperadorDataAccess _operadorDataAccess = new OperadorDataAccess();
            PageResultSP<PartnerLiquidacionResponse> result = new PageResultSP<PartnerLiquidacionResponse>();
            result = _operadorDataAccess.getListadoLiquidacionPartner(model,"");
            return Ok(result);
            //NroRucEmpresaCurrenUser
        }

        [Route("operador/lista-liquidacion-soloPartner")]
        [HttpPost]
        public IHttpActionResult getListadoLiquidacionSoloPartner(PageResultParam model)
        {
            OperadorDataAccess _operadorDataAccess = new OperadorDataAccess();
            PageResultSP<PartnerLiquidacionResponse> result = new PageResultSP<PartnerLiquidacionResponse>();
            result = _operadorDataAccess.getListadoLiquidacionPartner(model, NroRucEmpresaCurrenUser);
            return Ok(result);
            //NroRucEmpresaCurrenUser
        }

        /*Generar*/
        [Route("liquidacion-generar-codigo")]
        [HttpPost]
        public IHttpActionResult generarLiquidacion(List<ListaCodigosLiquidacionRequest> model)
        {
            OperadorDataAccess _liquidacionDataAccess = new OperadorDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _liquidacionDataAccess.GenerarLiquidacion(model,IdCurrenUser);
            return Ok(result);
            
        }
        /*Partner Aprueba Liquidacion*/
        [Route("liquidacion-aprueba-partner")]
        [HttpPost]
        public IHttpActionResult AprobarLiquidacionPartner(List<ListaCodigosLiquidacionRequest> model)
        {
            OperadorDataAccess _liquidacionDataAccess = new OperadorDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _liquidacionDataAccess.AprobarLiquidacionPartner(model, IdCurrenUser);
            return Ok(result);
        }

        [Route("liquidacion-rechaza-partner")]
        [HttpPost]
        public IHttpActionResult RechazaLiquidacionPartner(List<ListaCodigosLiquidacionRequest> model)
        {
            OperadorDataAccess _liquidacionDataAccess = new OperadorDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _liquidacionDataAccess.RechazaLiquidacionPartner(model, IdCurrenUser);
            return Ok(result);
        }

        [Route("liquidacion-aprueba-fideicomiso")]
        [HttpPost]
        public IHttpActionResult AprobarLiquidacionFideicomiso(List<ListaCodigosLiquidacionRequest> model)
        {
            OperadorDataAccess _liquidacionDataAccess = new OperadorDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _liquidacionDataAccess.AprobarLiquidacionFideicomiso(model, IdCurrenUser);
            return Ok(result);
        }


        [Route("liquidacion-pagar-partner")]
        [HttpPost]
        public IHttpActionResult PagarLiquidacion_Partner(List<ListaCodigosLiquidacionRequest> model)
        {
            OperadorDataAccess _liquidacionDataAccess = new OperadorDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _liquidacionDataAccess.PagarLiquidacionPartner(model, IdCurrenUser);
            return Ok(result);
        }
        

    }

    public class validarModelRequest
    {
        public string estado { get; set; }
        public int sid { get; set; }
        public string observacion { get; set; }
    }

     

}
