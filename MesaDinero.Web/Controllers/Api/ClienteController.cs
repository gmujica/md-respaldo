using System;
using System.Collections.Generic;
using System.Linq;
using MesaDinero.Domain;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.DataAccess;
namespace MesaDinero.Web.Controllers.Api
{

    [RoutePrefix("api")]
    public class ClienteController : ApiBaseController
    {
        [HttpPost]
        [Route("cliente/mis-datosbasicos")]
        public IHttpActionResult getDatosBasicosCurrentUser()
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            ClienteDataAccess _dataAccess = new ClienteDataAccess();
            result = _dataAccess.getDatosBasicosCurrentUser(IdCurrenCliente);

            return Ok(result);
        }

        [HttpPost]
        [Route("cliente/update-datosbasicos")]
        public IHttpActionResult upadteDatosBasicosCurrentUser(PersonaNatutalRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            ClienteDataAccess _dataAccess = new ClienteDataAccess();
            result = _dataAccess.updateDatosBasicosCurrentUser(model,IdCurrenCliente);

            return Ok(result);
        }

        [HttpPost]
        [Route("cliente/mis-datosBancarios")]
        public IHttpActionResult getDatosBancarios()
        {
            BaseResponse<List<CuentaBancariaClienteResponse>> result = new BaseResponse<List<CuentaBancariaClienteResponse>>();
            ClienteDataAccess _dataAccess = new ClienteDataAccess();
            result = _dataAccess.getDatosBancariosCurrentClient(IdCurrenCliente);


            return Ok(result);
        }

        [HttpPost]
        [Route("cliente/update-mis-datosBancarios")]
        public IHttpActionResult getDatosBancarios(List<CuentaBancariaClienteResponse> model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            ClienteDataAccess _dataAccess = new ClienteDataAccess();
            result = _dataAccess.updateCuentasBancarias(model,IdCurrenCliente);


            return Ok(result);
        }


    }
}
