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
    public class CommonController : ApiBaseController
    {
        [HttpPost]
        [Route("common/time-confirmacionMsM")]
        public IHttpActionResult getTimerConfirmacionMsM()
        {
            ConfirmacionMsMResponse result = new ConfirmacionMsMResponse();
            Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

            int time = common.getTiempoCofirmacionMsM();

            result.minutos = time / 60;
            result.segundos = time % 60;


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
        [Route("common/tipoCuentaBancarias")]
        public IHttpActionResult getAllTiposCuentasBancarias()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess common = new Domain.DataAccess.CommonDataAccess();

            result = common.getAllTipoCuentaBanacaria();

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
        [Route("common/getdatosPersona")]
        public IHttpActionResult getDatosPersonaNatural(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();

            result = _registroDataAccess.getDatosPersonaNatural(model);


            return Ok(result);
        }

       [HttpPost]
        [Route("common/getdatosPerAuth-registro")]
        public IHttpActionResult getPersonaAutorizadaForRegistro(RegistroCrearPassWord_Request model)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();

            result = _registroDataAccess.getPersonaAutorizadaForRegistro(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("common/getdatosEmpresa-for-registto")]
        public IHttpActionResult getDatosPersonaJuridica(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaReuest> result = new BaseResponse<PersonaJuridicaReuest>();
            Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();

            result = _registroDataAccess.getDatosPersonaJuridicaForRegistro(model);


            return Ok(result);

        }

        [HttpPost]
        [Route("common/getPaises")]
        public IHttpActionResult getPaises()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getAllPaises();


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getDepartatmentoXPais/{p}")]
        public IHttpActionResult getPaises(string p)
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getDepartamentoForPais(p);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getProvinciaXDepartamento/{p}/{d}")]
        public IHttpActionResult getPaises(string p, int d)
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getProvinciaForDepartamento(p, d);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getDistritoXProvincia/{p}/{d}/{pv}")]
        public IHttpActionResult getPaises(string p, int d, int pv)
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.getDistritoForProvincia(p, d, pv);


            return Ok(result);
        }

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
        [Route("common/getTipoCambioGarantizado")]
        public IHttpActionResult getTipoCambioGarantizado()
        {
            BaseResponse<List<SubastaCliente_PartnerPuja>> result = new BaseResponse<List<SubastaCliente_PartnerPuja>>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaClienteDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            //result = _subastaClienteDataAccess.getPartnerInit();

            return Ok(result);
        }

        [HttpPost]
        [Route("common/getTipoCambioSeleccionado")]
        public IHttpActionResult getTipoCambioSeleccionado(dynamic model)
        {
            BaseResponse<List<SubastaCliente_PartnerPuja>> result = new BaseResponse<List<SubastaCliente_PartnerPuja>>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaClienteDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            string accion = model.accion;
            string moneda = model.moneda;


            //result = _subastaClienteDataAccess.getPartnerTipoCambio(accion);

            return Ok(result);
        }



        [HttpPost]
        [Route("common/getTiempoSubastaCurrent")]
        public IHttpActionResult getTiempoCurrent(dynamic model)
        {
            string subasta = model.sid;

            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            BaseResponse<SubastaCurrentTime> result = _subastaDataAccess.getTiempoSubasta(subasta);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/getTiempoSubasta")]
        public IHttpActionResult getTipoCambioSeleccionado()
        {
            int result = 0;
            using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
            {
                result = context.Tb_MD_Tiempos.First(x => x.vCodTransaccion.Equals("T_Sb")).nTiempoStandar ?? 0;

            }

            return Ok(result);
        }

        [HttpPost]
        [Route("common/crearSubastaInit")]
        public IHttpActionResult postCrearSubastaInit(dynamic model)
        {
            string accion = model.accion;
            string moneda = model.moneda;

            BaseResponse<int> result = new BaseResponse<int>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaClienteDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            //  result = _subastaClienteDataAccess.crearSubastaInit(accion, moneda);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/subasta/actulizarTipoCambio")]
        public IHttpActionResult postSubastaActulizarTipoCambio(dynamic model)
        {
            string subasta = model.sid;

            BaseResponse<SubastaCliente_PartnerPuja> result = new BaseResponse<SubastaCliente_PartnerPuja>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaClienteDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            result = _subastaClienteDataAccess.getPartnerTipoCambioActualizado(subasta);


            return Ok(result);
        }

        [HttpPost]
        [Route("common/subasta/seleccionar-partner")]
        public IHttpActionResult seleccionarPartner(dynamic model)
        {
            string subasta = model.subasta;
            int codigo = model.codigo;

            BaseResponse<SubastaProceso_Response> result = new BaseResponse<SubastaProceso_Response>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            result = _subastaDataAccess.selecconionarPartnert(subasta, codigo);

            return Ok(result);
        }

        [HttpPost]
        [Route("common/compararPassword")]
        public IHttpActionResult compararPassword(dynamic model)
        {
            string clave = model.clave;

            BaseResponse<string> result = new BaseResponse<string>();
            Domain.DataAccess.CommonDataAccess _commonDataAccess = new Domain.DataAccess.CommonDataAccess();

            result = _commonDataAccess.compararPassword(clave, IdCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("subasta/saveConfirmacionSubasta")]
        public IHttpActionResult saveConfirmarSubasta(dynamic model)
        {
            string subasta = model.subasta;
            string password = model.clave;

            BaseResponse<string> result = new BaseResponse<string>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            result = _subastaDataAccess.ConfirmarOperacionSubasta(subasta, password, IdCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("subasta/saveEnvioPagoSubastaXCliente")]
        public IHttpActionResult pagoSubastaXCliente(dynamic model)
        {
            string subasta = model.subasta;
            string operacion = model.operacion;
            long cuentaOrigen = model.cuentaOrigen;
            long cuentaDestino = model.cuentaDestino;
            string bancoOrigenFideicomiso = model.bancoOrigenFideicomiso;
            long cuentaOrigenFideicomiso = model.cuentaOrigenFideicomiso;
            

            BaseResponse<string> result = new BaseResponse<string>();
            Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();

            result = _subastaDataAccess.ConfirmarPagoSubasta(subasta, operacion, cuentaOrigen, cuentaDestino, IdCurrenUser, bancoOrigenFideicomiso, cuentaOrigenFideicomiso);

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



    }
}
