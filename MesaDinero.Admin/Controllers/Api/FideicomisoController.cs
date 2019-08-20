using MesaDinero.Domain;
using MesaDinero.Domain.DataAccess.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class FideicomisoController : ApiController
    {
        [HttpPost]
        [Route("fideicomiso-usuarios")]
        public IHttpActionResult miNombre(PageResultParam model)
        {

            OperadorDataAccess _operador = new OperadorDataAccess();

             var resultado = _operador.getAllClientesRegistradosForOperado(model);


             return Ok(resultado);
        }

    }

 





}
