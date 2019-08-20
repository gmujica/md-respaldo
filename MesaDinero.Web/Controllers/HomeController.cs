using MesaDinero.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult quienesSomos()
        {
            return View();
        }


        public ActionResult empresas()
        {
            return View();
        }

        public ActionResult institucionesRegistradas()
        {
            return View();
        }

        public ActionResult ayuda()
        {
            return View();
        }

        public ActionResult contacto()
        {
            return View();
        }

        public ActionResult terminos()
        {
            return View();
        }

        public ActionResult privacidad()
        {
            return View();
        }

        public ActionResult libroReclamaciones()
        {
            return View();
        }

        public ActionResult corfid()
        {
            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}