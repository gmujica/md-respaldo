using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Admin.Controllers
{
    public class BaseController : Controller
    {

        protected string RolCurrenUser
        {
            get
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                string rol = identity.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value).SingleOrDefault();


                return rol;
            }
        }

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

        /* Usuario EMail */
        protected string CurrentNombreUsuario
        {
            get
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.Actor)
                    .Select(c => c.Value).SingleOrDefault();


                return id;
            }
        }

        /*Nombre Usuario EMail */
        protected string CurrentNombreApellidoUsuario
        {
            get
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();


                return id;
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
        /*NUEVO CAMBIAR*/
        public BaseController()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string abreviatura = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault().ToString();
            ViewBag.inicialesCliente = "";
            if (abreviatura.Length > 0)
            {
                ViewBag.inicialesCliente = abreviatura.Substring(0, 1);
            }

        }



    }
}