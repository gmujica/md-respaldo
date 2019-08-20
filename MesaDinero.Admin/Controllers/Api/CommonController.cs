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
using System.Threading.Tasks;

namespace MesaDinero.Admin.Controllers.Api
{
     [RoutePrefix("api")]
    public class CommonController : ApiBaseController
    {

        [HttpPost]
        [Route("common/getAllOrigenFondos")]
        public IHttpActionResult getOrigenFondos()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getAllOrigenFondos();


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getAllActividadEconomica")]
        public IHttpActionResult getActividadesEConomicas()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getAllActividadEconomica();


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getTipoDocumentoPersona")]
        public IHttpActionResult getAllTipoDocumentoPersona()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getAllTipoDocumentoPersona();


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getAllCargos")]
        public IHttpActionResult getAllCargos()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getAllCargos();


            return Ok(result);
        }

        [HttpPost]
        [Route("combo/tipoMoneda")]
        public IHttpActionResult getAllComboTipoMoneda()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

            result = common.getAllTipoMoneda();

            return Ok(result);
        }

        [HttpPost]
        [Route("common/tipoCuentaBancarias")]
        public IHttpActionResult getAllTiposCuentasBancarias()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

            result = common.getAllTipoCuentaBanacaria();

            return Ok(result);
        }

        [HttpPost]
        [Route("common/entidadesFinacieras")]
        public IHttpActionResult getEntidadesFinancieras()
        {
            BaseResponse<List<EntidadFinancieraResponse>> result = new BaseResponse<List<EntidadFinancieraResponse>>();
            Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

            result = common.getEntidadesFinancieras();

            return Ok(result);
        
        
        }

        [HttpPost]
        [Route("combo/empresa")]
        public IHttpActionResult getAllComboEmpresas(string filtro)
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

            result = common.getAllComboEmpresas(filtro);

            return Ok(result);
        }

        [HttpPost]
        [Route("common-getdatospersonasimple")]
        public async Task<IHttpActionResult> getDatosPersonaSimple(dynamic model)
        {
            string nro = model.nro;

            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();

            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();
            result = await _commonDataAccess.getDatosPersonaSimple(nro);

            return Ok(result);
        }


        [HttpPost]
        [Route("common-getdatosempresasimple")]
        public async Task<IHttpActionResult> getDatosEmpresaSimple(dynamic model)
        {
            string nro = model.nro;

            BaseResponse<PersonaJuridicaReuest> result = new BaseResponse<PersonaJuridicaReuest>();

            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();
            result = await _commonDataAccess.getDatosPersonaJuridicaSimple(nro);

            return Ok(result);
        }
        //[HttpPost]
        //[Route("combo/empresa-partner")]
        //public IHttpActionResult getAllPartner()
        //{
        //    BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
        //    Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

        //    result = common.getAllComboEmpresas();

        //    return Ok(result);
        //}

    }
}
