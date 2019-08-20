using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Web.Controllers
{
    [Authorize]
    public class ClienteController : BaseController
    {
        //
        // GET: /Cliente/

        public MesaDinero.Domain.Model.ModificaionClienteResponse GetDatosClientes()
        {
            MesaDinero.Domain.Model.ModificaionClienteResponse result = new Domain.Model.ModificaionClienteResponse();

            MesaDinero.Data.PersistenceModel.Tb_MD_Clientes cliente = null;
            MesaDinero.Data.PersistenceModel.Tb_MD_Pre_Clientes preCliente = null;
            using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
            {
                 cliente = context.Tb_MD_Clientes.FirstOrDefault(x => x.iIdCliente == IdCurrenCliente);
                 if (cliente.codigoModificacionDatos.HasValue)
                     preCliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.idPreCliente == cliente.codigoModificacionDatos);

            }

            result.idCliente = cliente.iIdCliente;
            result.CodigoModificacionDatos = cliente.codigoModificacionDatos;
            result.tipoCliente = cliente.vTipoCliente ?? 0;

            if(preCliente != null)
            {
                result.seguimiento = preCliente.Seguimiento;
            }

            return result;
        }

        public ActionResult ModificarDatos()
        {
           

            MesaDinero.Domain.Model.ModificaionClienteResponse model = GetDatosClientes();;


            if (model.tipoCliente == 2) {
                return RedirectToAction("ModificarDatosEmpresa");
            }
           

            return View(model);
        }

        public ActionResult ModificarDatosEmpresa() {
            return View();
        }

        public ActionResult CuentasBancarias()
        {
            return View();
        }

        public ActionResult ModificarPassword()
        {
            return View();
        }

	}
}