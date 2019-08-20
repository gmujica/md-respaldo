using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MesaDinero.Domain;

using MesaDinero.Domain.Model;
using MesaDinero.Domain.DataAccess.Admin;
using MesaDinero.Domain.DataAccess;


namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class CorfidController : ApiController
    {
        [HttpPost]
        [Route("corfid-operaciones-historicas")]
        public IHttpActionResult operacionesConfirmadas(PageResultParam model)
        {
            CorfidDataAcces _operador = new CorfidDataAcces();

            var resultado = _operador.getOperacionesHistoricas(model);

            return Ok(resultado);
        }
    }
}
