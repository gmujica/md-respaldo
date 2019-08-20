using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;


namespace MesaDinero.Web.Controllers.Api
{
    public class ApiBaseController : ApiController
    {
        protected int IdCurrenUser
        {
            get
            {
                int result = 0;
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.PrimarySid)
                    .Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrEmpty(id)) { result = Convert.ToInt32(id); }

                return result;
            }
        }

        protected int IdCurrenCliente
        {
            get
            {
                int result = 0;
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.DenyOnlySid)
                    .Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrEmpty(id)) { result = Convert.ToInt32(id); }

                return result;
            }
        }

        protected string NroDocumentoCurrenCliente
        {
            get
            {
               string result = string.Empty;
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
                    .Select(c => c.Value).SingleOrDefault();
                result = id;

                return result;
            }
        }

        /*Ruc Empresa */
        protected string NroRucEmpresaCurrenUser
        {
            get
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
                    .Select(c => c.Value).SingleOrDefault();

                return id;
            }
        }

          /*Tipo Cliente */
        protected string TipoCurrenUser
        {
            get
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.PostalCode)
                    .Select(c => c.Value).SingleOrDefault();

                return id;
            }
        }
        
         /* Abreviatura */
        protected string AbreviaturaCurrenUser
        {
            get
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.GivenName)
                    .Select(c => c.Value).SingleOrDefault();

                return id;
            }
        }
        

    
    }
}
