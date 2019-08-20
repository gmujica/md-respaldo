using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Web.Controllers
{

    [Authorize]
    public class BaseController : Controller
    {


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

        protected int TipoClienteCurrentUser
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
        /*Email Usuario*/
        protected string EmailCurrenUser
        {
            get
            {
                string result = "";
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var id = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrEmpty(id)) { result = id; }

                return result;
            }
        }

         /*Nombre Cliente*/
        protected string NombreCurrenUser
        {
            get
            {
                string result = "";
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var nombre = identity.Claims.Where(c => c.Type == ClaimTypes.Actor)
                    .Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrEmpty(nombre)) { result = nombre; }

                return result;
            }
        }



     






     

        public BaseController()
        {
            if (IdCurrenUser > 0)
            {
                MesaDinero.Data.PersistenceModel.Tb_MD_ClienteUsuario usuario = null;
                using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
                {
                    usuario = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.IdUsuario == IdCurrenUser);
                }

                ViewBag.nombreCliente = usuario.NombreCliente;
                ViewBag.inicialesCliente = usuario.Iniciales;
                
            }else{
                ViewBag.nombreCliente = "";
                ViewBag.inicialesCliente = "";
            }
            ViewBag.IconSideBar = "<div class='bars-icon'></div>";

        }




       
	}
}