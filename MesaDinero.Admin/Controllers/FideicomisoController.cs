using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Admin.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    public class FideicomisoController : BaseController
    {
        //
        // GET: /Fideicomiso/
        public ActionResult Index()
        {

            //if (RolCurrenUser != "Fideicomiso")
            //    throw new HttpException(404, "No tienes permiso para acceder a esta sección");


                //return new HttpStatusCodeResult(HttpStatusCode.Unauthorized,"No tienes permiso para acceder a esta sección");

            return View();
        }

        [AllowAnonymous]
        public ActionResult LiquidacionPartnerFideiComiso()
        {
            return View();
        }
	}
}