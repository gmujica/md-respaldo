using MesaDinero.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Web.Controllers
{

    [Authorize]
    public class SubastaController : BaseController
    {

        #region Method's
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }



        #endregion

        //
        // GET: /Subasta/
        public ActionResult Index()
        {
            ViewBag.MenuSubasta = new MesaDinero.Domain.helpMenuSubasta();
            return View();
        }

        [ChildActionOnly]
        public ActionResult Inicio()
        {
            BaseResponse<Subasta_Init_Reponse> result = new BaseResponse<Subasta_Init_Reponse>();
            result.data = new Subasta_Init_Reponse();

            try
            {
                using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
                {
                    result.data.partners = context.Database.SqlQuery<string>("exec proc_sel_partners_inicio_subasta").ToList<string>();
                    result.data.tiempo = context.Tb_MD_Tiempos.FirstOrDefault(x => x.vCodTransaccion.Equals("T_Sb")).nTiempoStandar ?? 0;
                }

                string page = RenderRazorViewToString("Inicio", result.data);
                result.data.subastaHtml = page;

                result.success = true;
            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;
            }

            return PartialView(result.data);
        }

        [HttpPost]
        public ActionResult GetViewProceso(subastaRequest model)
        {

            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            decimal monto = 0;
            monto = Convert.ToDecimal(model.montotext.Replace('.', ','));
            MesaDinero.Domain.BaseResponse<SubastaProceso_Response> subasta = _subastaDataAccess.crearSubastaInit(model.operacion, monto, "USD", model.monedaEnvio, model.monedaRecibe, IdCurrenCliente);

            if (subasta.success == true)
            {

                return RedirectToAction("Activa", "Subasta", new { id = subasta.data.sid });
            }
            else
            {
                return Content(subasta.error);
            }

        }

        [HttpPost]
        public ActionResult GetViewConfirmar(ConfirmarRequest model)
        {

            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            MesaDinero.Domain.BaseResponse<string> subasta = _subastaDataAccess.ConfirmarPartnerSubasta(model.subasta, model.partner);

            if (subasta.success == true)
            {
                return RedirectToAction("Confirmar", "Subasta", new { id = model.subasta });
            }
            else
            {
                return RedirectToAction("Activa", "Subasta", new { id = model.subasta });
            }


        }

        public ActionResult Activa(string id)
        {
            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            MesaDinero.Domain.BaseResponse<SubastaProceso_Response> subasta = _subastaDataAccess.getModelLoadSubastaActiva(id);
            subasta.data.menu.menu = "SM1";
            subasta.data.menu.sid = id;
            ViewBag.MenuSubasta = subasta.data.menu;

            return View(subasta);
        }

        public ActionResult Confirmar(string id)
        {
            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            MesaDinero.Domain.BaseResponse<Subasta_ConfirmarOperacion_Response> subasta = _subastaDataAccess.getModelLoadConfirmarSubasta(id);

            subasta.data.menu.menu = "SM2";
            subasta.data.menu.sid = id;
            ViewBag.MenuSubasta = subasta.data.menu;

            return View(subasta.data);
        }

        public ActionResult Envio(string id)
        {
            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            BaseResponse<Subasta_ConfirmarPago_Response> subasta = _subastaDataAccess.getModelLoadConfirmarPago(id);

            if (subasta.success == false)
                return Content(subasta.error);

            subasta.data.menu.menu = "SM3";
            subasta.data.menu.sid = id;

            ViewBag.MenuSubasta = subasta.data.menu;

            ViewBag.CodigoSecrect = id;
            return View(subasta.data);
        }

        public ActionResult recibes(string id)
        {
            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            BaseResponse<Subasta_Recibe_Response> subasta = _subastaDataAccess.getModelRecibeSubasta(id);

            subasta.data.menu.menu = "SM5";
            subasta.data.menu.sid = id;
            ViewBag.MenuSubasta = subasta.data.menu;

            return View(subasta.data);
        }

        public ActionResult Verificacion(string id)
        {

            MesaDinero.Domain.DataAccess.SubastaClienteDataAccess _subastaDataAccess = new Domain.DataAccess.SubastaClienteDataAccess();
            BaseResponse<Subasta_Verificacion_Response> subasta = _subastaDataAccess.getModelVerificacionSubasta(id);

            subasta.data.menu.menu = "SM4";
            subasta.data.menu.sid = id;

            ViewBag.MenuSubasta = subasta.data.menu;

            return View(subasta.data);
        }

        public ActionResult ListaOperacionCliente()
        {
            return View();
        }
    }

    public class ConfirmarRequest
    {
        public string subasta { get; set; }
        public int partner { get; set; }
        public long cuentaOrigen { get; set; }
        public long cuentaDestino { get; set; }
        public string password { get; set; }

    }

    public class subastaRequest
    {
        public string operacion { get; set; }
        public decimal monto { get; set; }
        public string montotext { get; set; }
        public string monedaEnvio { get; set; }
        public string monedaRecibe { get; set; }

    }

    public class ConfirmarResponse
    {
        public decimal monto { get; set; }
        public string parnet { get; set; }
        public string nroSubasta { get; set; }
        public decimal recibe { get; set; }
        public decimal tipoCambio { get; set; }
        public string monedaEnvio { get; set; }
        public string monedaRecibe { get; set; }
        public int tiempo { get; set; }
        public int codigo { get; set; }
    }

}