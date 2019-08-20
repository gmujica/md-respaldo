using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;


namespace MesaDinero.Admin.Controllers.Api
{
    public class ApiBaseController : ApiController
    {
        /*ID Usuario*/
        protected int IdCurrenUser
        {
            get
            {
                int result = 0;
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.PostalCode)
                    .Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrEmpty(id)) { result = Convert.ToInt32(id); }

                return result;
            }
        }

        /*Ruc Empresa */
        protected string NroRucEmpresaCurrenUser
        {
            get
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.Country)
                    .Select(c => c.Value).SingleOrDefault();


                return id;
            }
        }

        /*Nombre Usuario EMail */
        protected string CurrentNombreUsuario
        {
            get
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();


                return id;
            }
        }
        /*Numero de documento Persona*/
        /*NUEVO CAMBIAR*/
        protected string NroDocumentoCurrenUser
        {
            get
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();

                return id;
            }
        }

    }
}
