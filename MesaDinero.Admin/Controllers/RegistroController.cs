using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesaDinero.Admin.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    public class RegistroController : BaseController
    {

        public ActionResult Empresa()
        {
            return View();
        }

        public ActionResult ModificarDatosEmpresa()
        {
            return View();
        }

        public ActionResult ModificarCuentasBancarias()
        {
            return View();
        }

        public ActionResult RegistrarUsuario()
        {
            return View();
        }

        public ActionResult Perfiles()
        {
            return View();
        }
    
        public ActionResult ModificarPassword()
        {
            return View();
        }
        
        public ActionResult PasswordAdmin(string id)
        {

            Tb_MD_Mae_Usuarios mUsuario = null;
            MesaDinero.Domain.DataAccess.CommonDataAccess _common = new Domain.DataAccess.CommonDataAccess();
            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();

            Guid sid = Guid.NewGuid();
            try
            {
                sid = Guid.Parse(id);
                mUsuario = _common.getClienteAdmBySecredId(sid);
                model.sid = id;
                model.tipoCliente = 1;
                model.email = mUsuario.vEmailUsuario;
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult RecuperarPassword()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CambiarPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpNotFoundResult();

            Guid secretId = Guid.NewGuid();

            try { secretId = Guid.Parse(id); }
            catch (Exception) { return new HttpNotFoundResult(); }

            CambioPassWordAdmRequest model = new CambioPassWordAdmRequest();
            using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
            {
                MesaDinero.Data.PersistenceModel.Tb_MD_RecuperarPassword recuperar = null;
                recuperar = context.Tb_MD_RecuperarPassword.FirstOrDefault(x => x.SecredId == secretId);

                if (recuperar == null)
                    return new HttpNotFoundResult();

                if (DateTime.Now > recuperar.FechaExpiracion)
                    return new HttpNotFoundResult();

                model.email = recuperar.Email;
                model.sid = recuperar.SecredId.ToString();
            }
            return View(model);
        }
        

        /*comentario*/
    }
}