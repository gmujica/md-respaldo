using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Admin.Controllers
{
    public class HomeController : BaseController
    {

        [Authorize]
        //[AllowAnonymous]
        public ActionResult Index()
        {

            string clave = MesaDinero.Domain.Helper.Encrypt.DecryptKey("cM2+PmeJKe+R4A3yGxPH0w==");


            if(User.Identity.IsAuthenticated)
            {
                if (RolCurrenUser == "Operador")
                    return RedirectToAction("", "Operador");
                else if (RolCurrenUser == "Fideicomiso")
                    return RedirectToAction("", "Fideicomiso");
                else if (RolCurrenUser == "Partner")
                    return RedirectToAction("SubastasActivas", "Partner");


            }

            return View();
        }
        
        [AllowAnonymous]
        public ActionResult Inicial() {
            return View();
        }

    }
}