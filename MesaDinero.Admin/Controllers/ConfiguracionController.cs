using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Admin.Controllers
{
    [Authorize]
    public class ConfiguracionController : BaseController
    {

        public ActionResult TipoDocumento()
        {
            return View();
        }

        public ActionResult Cargo()
        {
            return View();
        }

        public ActionResult OrigenFondo()
        {
            return View();
        }

        public ActionResult ActividadEconomica()
        {
            return View();
        }

        public ActionResult CuentaBancaria()
        {
            return View();
        }

        public ActionResult TipoMoneda()
        {
            return View();
        }

        public ActionResult Tiempos()
        {
            return View();
        }

        public ActionResult EntidadesFinancieras()
        {
            return View();
        }

        public ActionResult Pais()
        {
            return View();
        }

        public ActionResult Departamento()
        {
            return View();
        }

        public ActionResult Distrito()
        {
            return View();
        }

        public ActionResult Provincia()
        {
            return View();
        }

        public ActionResult Ubigeo()
        {
            return View();
        }

    }
}