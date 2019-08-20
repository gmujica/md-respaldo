using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Web.Controllers
{
    [Authorize]
    public class InicioController : BaseController
    {
        //
        // GET: /Inicio/
        public ActionResult Index()
        {
            ViewBag.IconSideBar = "<div class='bars-icon' style='display:none;'></div>";
            return View();
        }



	}
}