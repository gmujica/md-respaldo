using System;
using System.Collections.Generic;
using System.Linq;
using MesaDinero.Domain;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.DataAccess.Admin;
using MesaDinero.Domain.DataAccess.Admin.Configuracion;
using MesaDinero.Domain.Model.Admin;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class ConfiguracionController : ApiController
    {
        [HttpPost]
        [Route("configuracion-listado-TipoDocumento")]
        public IHttpActionResult getAllTipoDocumento()
        {
            return Ok("hola mundo");
        }

        #region Cargos
        [HttpPost]
        [Route("configuracion-listado-cargos")]
        public IHttpActionResult getAllCargos(PageResultParam model)
        {
            CargoDataAccess _cargoDataAccess = new CargoDataAccess();
            PageResultSP<CargoResponse> result = new PageResultSP<CargoResponse>();

            result = _cargoDataAccess.getAllCargos(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-new-cargo")]
        public IHttpActionResult addNewCargo(CargoRequest model)
        {

            CargoDataAccess _cargoDataAccess = new CargoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _cargoDataAccess.insertNewCargo(model);



            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-cargo")]
        public IHttpActionResult editCargo(CargoRequest model)
        {

            CargoDataAccess _cargoDataAccess = new CargoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _cargoDataAccess.EditarCargo(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-eliminar-cargo")]
        public IHttpActionResult eliminarCargo(CargoRequest model)
        {

            CargoDataAccess _cargoDataAccess = new CargoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _cargoDataAccess.EliminarCargo(model);

            return Ok(result);
        }
        #endregion


        /* Tipo Documento*/
        [HttpPost]
        [Route("configuracion-listado-tipo-documento")]
        public IHttpActionResult getAllTipoDocumento(PageResultParam model)
        {
            TipoDocumentoDataAccess _tipoDocDataAccess = new TipoDocumentoDataAccess();
            PageResultSP<TipoDocumentoResponse> result = new PageResultSP<TipoDocumentoResponse>();

            result = _tipoDocDataAccess.getTipoDocumento(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-tipo-documento")]
        public IHttpActionResult addNewTipoDocumento(TipoDocumentoRequest model)
        {

            TipoDocumentoDataAccess _tipoDocDataAccess = new TipoDocumentoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoDocDataAccess.insertNewTipoDocumento(model);



            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-tipo-documento")]
        public IHttpActionResult editTipoDocumento(TipoDocumentoRequest model)
        {

            TipoDocumentoDataAccess _tipoDocDataAccess = new TipoDocumentoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoDocDataAccess.EditarTipoDocumento(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-eliminar-tipo-documento")]
        public IHttpActionResult eliminarTipoDocumento(TipoDocumentoRequest model)
        {

            TipoDocumentoDataAccess _tipoDocDataAccess = new TipoDocumentoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoDocDataAccess.EliminarTipoDocumento(model);

            return Ok(result);
        }

        /*Origen de Fondo*/


        [HttpPost]
        [Route("configuracion-listado-origen-fondo")]
        public IHttpActionResult getAllOrigenFondo(PageResultParam model)
        {
            OrigenFondoDataAccess _origenFondoDataAccess = new OrigenFondoDataAccess();
            PageResultSP<OrigenFondoResponse> result = new PageResultSP<OrigenFondoResponse>();

            result = _origenFondoDataAccess.getAllOrigenFondo(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-origen-fondo")]
        public IHttpActionResult addNewOrigenFondo(OrigenFondoRequest model)
        {
            OrigenFondoDataAccess _origenFondoDataAccess = new OrigenFondoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _origenFondoDataAccess.insertNewOrigenFondo(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-origen-fondo")]
        public IHttpActionResult editOrigenFondo(OrigenFondoRequest model)
        {

            OrigenFondoDataAccess _origenFondoDataAccess = new OrigenFondoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _origenFondoDataAccess.EditarOrigenFondo(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-eliminar-origen-fondo")]
        public IHttpActionResult eliminarOrigenFondo(OrigenFondoRequest model)
        {

            OrigenFondoDataAccess _origenFondoDataAccess = new OrigenFondoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _origenFondoDataAccess.EliminarOrigenFondo(model);

            return Ok(result);
        }

        /* Actividad Economica */

        [HttpPost]
        [Route("configuracion-listado-actividad-economica")]
        public IHttpActionResult getAllActividadEconomica(PageResultParam model)
        {
            ActividadEconomicaDataAccess _actividadEconomicaDataAccess = new ActividadEconomicaDataAccess();
            PageResultSP<ActividadEconomicaResponse> result = new PageResultSP<ActividadEconomicaResponse>();

            result = _actividadEconomicaDataAccess.getAllActividadEconomica(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-actividad-economica")]
        public IHttpActionResult addNewActividadEconomica(ActividadEconomicaRequest model)
        {
            ActividadEconomicaDataAccess _actividadEconomicaDataAccess = new ActividadEconomicaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _actividadEconomicaDataAccess.insertNewActividadEconomica(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-actividad-economica")]
        public IHttpActionResult editActividadEconomica(ActividadEconomicaRequest model)
        {
            ActividadEconomicaDataAccess _actividadEconomicaDataAccess = new ActividadEconomicaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _actividadEconomicaDataAccess.EditarActividadEconomica(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-eliminar-actividad-economica")]
        public IHttpActionResult eliminarActividadEconomica(ActividadEconomicaRequest model)
        {

            ActividadEconomicaDataAccess _actividadEconomicaDataAccess = new ActividadEconomicaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _actividadEconomicaDataAccess.EliminarActividadEconomica(model);

            return Ok(result);
        }


        /* Tipo Cuenta Bancaria */

        [HttpPost]
        [Route("configuracion-listado-tipo-cuenta")]
        public IHttpActionResult getAllTipoCuenta(PageResultParam model)
        {
            CuentaBancariaDataAccess _tipoCuentaDataAccess = new CuentaBancariaDataAccess();
            PageResultSP<CuentaBancariaResponse> result = new PageResultSP<CuentaBancariaResponse>();

            result = _tipoCuentaDataAccess.getAllCuentaBancaria(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-tipo-cuenta")]
        public IHttpActionResult addNewTipoCuenta(CuentaBancariaRequest model)
        {
            CuentaBancariaDataAccess _tipoCuentaDataAccess = new CuentaBancariaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoCuentaDataAccess.insertNewTipoCuentaBancaria(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-tipo-cuenta")]
        public IHttpActionResult editTipoCuenta(CuentaBancariaRequest model)
        {
            CuentaBancariaDataAccess _tipoCuentaDataAccess = new CuentaBancariaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoCuentaDataAccess.EditarTipoCuentaBancaria(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-tipo-cuenta")]
        public IHttpActionResult eliminarTipoCuenta(CuentaBancariaRequest model)
        {

            CuentaBancariaDataAccess _tipoCuentaDataAccess = new CuentaBancariaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoCuentaDataAccess.EliminarTipoCuentaBancaria(model);

            return Ok(result);
        }



        /* Tipo de Moneda */

        [HttpPost]
        [Route("configuracion-listado-tipo-moneda")]
        public IHttpActionResult getAllTipoMoneda(PageResultParam model)
        {
            TipoMonedaDataAccess _tipoMonedaDataAccess = new TipoMonedaDataAccess();
            PageResultSP<TipoMonedaResponse> result = new PageResultSP<TipoMonedaResponse>();

            result = _tipoMonedaDataAccess.getAllTipoMoneda(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-tipo-moneda")]
        public IHttpActionResult addNewTipoMoneda(TipoMonedaRequest model)
        {
            TipoMonedaDataAccess _tipoMonedaDataAccess = new TipoMonedaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoMonedaDataAccess.insertNewTipoMoneda(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-tipo-moneda")]
        public IHttpActionResult editTipoMoneda(TipoMonedaRequest model)
        {
            TipoMonedaDataAccess _tipoMonedaDataAccess = new TipoMonedaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoMonedaDataAccess.EditarTipoMoneda(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-tipo-moneda")]
        public IHttpActionResult eliminarTipoMoneda(TipoMonedaRequest model)
        {
            TipoMonedaDataAccess _tipoMonedaDataAccess = new TipoMonedaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tipoMonedaDataAccess.EliminarTipoMoneda(model);

            return Ok(result);
        }

        /* Tiempo */


        [HttpPost]
        [Route("configuracion-listado-tiempo")]
        public IHttpActionResult getAllTiempo(PageResultParam model)
        {
            TiempoDataAccess _tiempoDataAccess = new TiempoDataAccess();
            PageResultSP<TiempoResponse> result = new PageResultSP<TiempoResponse>();

            result = _tiempoDataAccess.getAllTiempo(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-tiempo")]
        public IHttpActionResult addNewTiempo(TiempoRequest model)
        {
            TiempoDataAccess _tiempoDataAccess = new TiempoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tiempoDataAccess.insertNewTiempo(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-tiempo")]
        public IHttpActionResult editTiempo(TiempoRequest model)
        {
            TiempoDataAccess _tiempoDataAccess = new TiempoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tiempoDataAccess.EditarTiempo(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-tiempo")]
        public IHttpActionResult eliminarTiempo(TiempoRequest model)
        {
            TiempoDataAccess _tiempoDataAccess = new TiempoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _tiempoDataAccess.EliminarTiempo(model);

            return Ok(result);
        }


        /* Entidad Financiera */


        [HttpPost]
        [Route("configuracion-listado-financiera")]
        public IHttpActionResult getAllEntidadFinanciera(PageResultParam model)
        {
            EntidadesFinancierasDataAccess _entidadFinancieraDataAccess = new EntidadesFinancierasDataAccess();
            PageResultSP<EntidadFinancieraResponses> result = new PageResultSP<EntidadFinancieraResponses>();
            result = _entidadFinancieraDataAccess.getAllEntidadesFinancieras(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-financiera")]
        public IHttpActionResult addNewEntidadFinanciera()
        {

            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            int indice = Convert.ToInt16(System.Web.HttpContext.Current.Request.Params["indice"]);
            System.Web.HttpPostedFile file = null;

            if (indice > 0)
            {
                file = files[0];
            }

            string json = System.Web.HttpContext.Current.Request.Params["jsBanco"];

            EntidadFinancieraRequest model = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<EntidadFinancieraRequest>(json);
            EntidadesFinancierasDataAccess _entidadFinancieraDataAccess = new EntidadesFinancierasDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _entidadFinancieraDataAccess.insertNewEntidadFinanciera(model, file);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-financiera")]
        public IHttpActionResult editEntidadFinanciera()
        {

            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            int indice = Convert.ToInt16(System.Web.HttpContext.Current.Request.Params["indice"]);
            System.Web.HttpPostedFile file = null;

            if (indice > 0)
            {
                file = files[0];
            }

            string json = System.Web.HttpContext.Current.Request.Params["jsBanco"];

            EntidadFinancieraRequest model = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<EntidadFinancieraRequest>(json);
            EntidadesFinancierasDataAccess _entidadFinancieraDataAccess = new EntidadesFinancierasDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _entidadFinancieraDataAccess.EditarEntidadFinanciera(model, file);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-financiera")]
        public IHttpActionResult eliminarEntidadFinanciera(EntidadFinancieraRequest model)
        {
            EntidadesFinancierasDataAccess _entidadFinancieraDataAccess = new EntidadesFinancierasDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _entidadFinancieraDataAccess.EliminarEntidadFinanciera(model);

            return Ok(result);
        }

        /* Pais */

        [HttpPost]
        [Route("configuracion-listado-pais")]
        public IHttpActionResult getAllPais(PageResultParam model)
        {
            PaisDataAccess _paisDataAccess = new PaisDataAccess();
            PageResultSP<PaisResponse> result = new PageResultSP<PaisResponse>();

            result = _paisDataAccess.getAllPais(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-pais")]
        public IHttpActionResult addNewPais(PaisRequest model)
        {
            PaisDataAccess _paisDataAccess = new PaisDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _paisDataAccess.insertNewPais(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-pais")]
        public IHttpActionResult editPais(PaisRequest model)
        {
            PaisDataAccess _paisDataAccess = new PaisDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _paisDataAccess.EditarPais(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-pais")]
        public IHttpActionResult eliminarPais(PaisRequest model)
        {

            PaisDataAccess _paisDataAccess = new PaisDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _paisDataAccess.EliminarPais(model);

            return Ok(result);
        }

        /* Departamento */

        [HttpPost]
        [Route("configuracion-listado-departamento")]
        public IHttpActionResult getAllDepartamento(PageResultParam model)
        {
            DepartamentoDataAccess _departamentoDataAccess = new DepartamentoDataAccess();
            PageResultSP<DepartamentoResponse> result = new PageResultSP<DepartamentoResponse>();

            result = _departamentoDataAccess.getAllDepartamento(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-departamento")]
        public IHttpActionResult addNewDepartamento(DepartamentoRequest model)
        {
            DepartamentoDataAccess _departamentoDataAccess = new DepartamentoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _departamentoDataAccess.insertNewDepartamento(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-departamento")]
        public IHttpActionResult editDepartamento(DepartamentoRequest model)
        {
            DepartamentoDataAccess _departamentoDataAccess = new DepartamentoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _departamentoDataAccess.EditarDepartamento(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-departamento")]
        public IHttpActionResult eliminarDepartamento(DepartamentoRequest model)
        {

            DepartamentoDataAccess _departamentoDataAccess = new DepartamentoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _departamentoDataAccess.EliminarDepartamento(model);

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



        /* Distrito */

        [HttpPost]
        [Route("configuracion-listado-distrito")]
        public IHttpActionResult getAllDistrito(PageResultParam model)
        {
            DistritoDataAccess _distritoDataAccess = new DistritoDataAccess();
            PageResultSP<DistritoResponse> result = new PageResultSP<DistritoResponse>();

            result = _distritoDataAccess.getAllDistrito(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-distrito")]
        public IHttpActionResult addNewDistrito(DistritoRequest model)
        {
            DistritoDataAccess _distritoDataAccess = new DistritoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _distritoDataAccess.insertNewDistrito(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-distrito")]
        public IHttpActionResult editDistrito(DistritoRequest model)
        {
            DistritoDataAccess _distritoDataAccess = new DistritoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _distritoDataAccess.EditarDistrito(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-distrito")]
        public IHttpActionResult eliminarDistrito(DistritoRequest model)
        {
            DistritoDataAccess _distritoDataAccess = new DistritoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _distritoDataAccess.EliminarDistrito(model);

            return Ok(result);
        }



        /* Provincia */

        [HttpPost]
        [Route("configuracion-listado-provincia")]
        public IHttpActionResult getAllProvincia(PageResultParam model)
        {
            ProvinciaDataAccess _provinciaDataAccess = new ProvinciaDataAccess();
            PageResultSP<ProvinciaResponse> result = new PageResultSP<ProvinciaResponse>();

            result = _provinciaDataAccess.getAllProvincia(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-provincia")]
        public IHttpActionResult addNewProvincia(ProvinciaRequest model)
        {
            ProvinciaDataAccess _provinciaDataAccess = new ProvinciaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _provinciaDataAccess.insertNewProvincia(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-provincia")]
        public IHttpActionResult editProvincia(ProvinciaRequest model)
        {
            ProvinciaDataAccess _provinciaDataAccess = new ProvinciaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _provinciaDataAccess.EditarProvincia(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-provincia")]
        public IHttpActionResult eliminarProvincia(ProvinciaRequest model)
        {
            ProvinciaDataAccess _provinciaDataAccess = new ProvinciaDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _provinciaDataAccess.EliminarProvincia(model);

            return Ok(result);
        }


        /* Ubigeo */

        [HttpPost]
        [Route("configuracion-listado-ubigeo")]
        public IHttpActionResult getAllUbigeo(PageResultParam model)
        {
            UbigeoDataAccess _ubigeoDataAccess = new UbigeoDataAccess();
            PageResultSP<UbigeoResponse> result = new PageResultSP<UbigeoResponse>();

            result = _ubigeoDataAccess.getAllUbigeo(model);

            return Ok(result);
        }


        [HttpPost]
        [Route("configuracion-new-ubigeo")]
        public IHttpActionResult addNewUbigeo(UbigeoRequest model)
        {
            UbigeoDataAccess _ubigeoDataAccess = new UbigeoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _ubigeoDataAccess.insertNewUbigeo(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-edit-ubigeo")]
        public IHttpActionResult editUbigeo(UbigeoRequest model)
        {
            UbigeoDataAccess _ubigeoDataAccess = new UbigeoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _ubigeoDataAccess.EditarUbigeo(model);

            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-eliminar-ubigeo")]
        public IHttpActionResult eliminarUbigeo(UbigeoRequest model)
        {
            UbigeoDataAccess _ubigeoDataAccess = new UbigeoDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            result = _ubigeoDataAccess.EliminarUbigeo(model);

            return Ok(result);
        }

        /* Departamento x Pais */
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
        [Route("configuracion-getProvincia")]
        public IHttpActionResult getProvincia()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            UbigeoDataAccess _ubigeoDataAccess = new UbigeoDataAccess();
            result = _ubigeoDataAccess.getProvincia();
            return Ok(result);
        }

        [HttpPost]
        [Route("configuracion-getDistrito")]
        public IHttpActionResult getDistrito()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            UbigeoDataAccess _ubigeoDataAccess = new UbigeoDataAccess();
            result = _ubigeoDataAccess.getDistrito();
            return Ok(result);
        }


    }
}
