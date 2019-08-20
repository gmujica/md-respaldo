using MesaDinero.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MesaDinero.Web.Models;
using MesaDinero.Domain.Model;

namespace MesaDinero.Web.Controllers
{
    public class AccesoController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //
        // GET: /Acceso/
        public ActionResult Index()
        {

            string clave = MesaDinero.Domain.Helper.Encrypt.DecryptKey("DGgLc0s8vi8=");

            if (User.Identity.IsAuthenticated)
                return RedirectToAction("", "Inicio");

            Domain.Model.RegistroPassWpord_Response model = new Domain.Model.RegistroPassWpord_Response();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Domain.Model.RegistroPassWpord_Response model)
        {
            try
            {
                bool error = false;

                if (string.IsNullOrEmpty(model.email))
                {
                    ModelState.AddModelError("", "El email es una campo requerido");
                    error = true;
                }

                if (string.IsNullOrEmpty(model.password))
                {
                    ModelState.AddModelError("", "El password es una campo requerido");
                    error = true;
                }

                if (error)
                {
                    throw new Exception("");
                }

                //  Data.PersistenceModel.Tb_MD_Pre_Clientes cliente = null;
                Data.PersistenceModel.Tb_MD_ClienteUsuario login_result = null;

                using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
                {
                    string clave = MesaDinero.Domain.Helper.Encrypt.EncryptKey(model.password);
                    //     string clave = model.password;


                    login_result = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.Email.Equals(model.email) && x.Password.Equals(clave));


                    if (login_result == null)
                    {
                        ModelState.AddModelError("", "El usuario o password es incorrecto.");
                        error = true;
                    }

                    if (error)
                    {
                        throw new Exception("");
                    }


                    // cliente = login_result.Tb_MD_Pre_Clientes;

                }




                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.WindowsAccountName, model.email),
                    new Claim(ClaimTypes.Name, model.email),
                    new Claim(ClaimTypes.Actor, login_result.NombreCliente),
                    new Claim(ClaimTypes.SerialNumber, login_result.vNroDocumento),
                    new Claim(ClaimTypes.Role, ""),
                    new Claim(ClaimTypes.PrimarySid,login_result.IdUsuario.ToString()),
                    new Claim(ClaimTypes.DenyOnlySid,login_result.IdCliente.ToString()),
                    new Claim(ClaimTypes.PostalCode,login_result.TipoCliente.ToString()),     
                    new Claim(ClaimTypes.NameIdentifier, model.email),
                    new Claim(ClaimTypes.Email, model.email),
                    new Claim(ClaimTypes.GivenName, login_result.Iniciales),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",model.email)                    
                };
                var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                AuthenticationManager.SignIn(id);

                if(!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("", "Inicio");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CrearUsuario(RegistroCrearPassWord_Request model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            try
            {
                result.success = true;
                Domain.DataAccess.RegistroCliente _regsitroDataAccess = new Domain.DataAccess.RegistroCliente();
                BaseResponse<RegistroPassWpord_Response2> result_ = _regsitroDataAccess.CrearPasswprd(model);

                if (result_.success == true)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.WindowsAccountName, result_.data.email),
                    new Claim(ClaimTypes.Name, result_.data.email),
                    new Claim(ClaimTypes.Actor, result_.data.NombreCliente),
                    new Claim(ClaimTypes.SerialNumber, result_.data.vNroDocumento),
                    new Claim(ClaimTypes.Role, ""),
                    new Claim(ClaimTypes.PrimarySid,result_.data.IdUsuario.ToString()),
                    new Claim(ClaimTypes.DenyOnlySid,result_.data.IdCliente.ToString()),
                    new Claim(ClaimTypes.PostalCode,result_.data.TipoCliente.ToString()),     
                    new Claim(ClaimTypes.NameIdentifier, result_.data.email),
                    new Claim(ClaimTypes.Email, result_.data.email),
                    new Claim(ClaimTypes.GivenName, result_.data.Iniciales),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",result_.data.email)                    
                };
                    var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = Request.GetOwinContext();
                    AuthenticationManager.SignIn(id);
                }
                else
                {
                    throw new Exception(result_.error);
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Login(Domain.Model.RegistroPassWpord_Response model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            string error = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(model.email))
                    error = "(*) El email es una campo requerido <br/>";

                if (string.IsNullOrEmpty(model.password))
                    error = "(*) El password es una campo requerido";

                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                string clave = MesaDinero.Domain.Helper.Encrypt.EncryptKey(model.password);
                Data.PersistenceModel.Tb_MD_ClienteUsuario login_result = null;
                using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
                {
                    login_result = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.Email.Equals(model.email) && x.Password.Equals(clave));
                 
                    if (login_result == null)
                        throw new Exception("El email o el password son incorrectos.");

                }
                //ViewBag.nombreCliente = login_result.NombreCliente;
                //ViewBag.inicialesCliente = login_result.Iniciales;
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.WindowsAccountName, model.email),
                    new Claim(ClaimTypes.Name, model.email),
                    new Claim(ClaimTypes.Actor, login_result.NombreCliente),
                    new Claim(ClaimTypes.SerialNumber, login_result.vNroDocumento),
                    new Claim(ClaimTypes.Role, ""),
                    new Claim(ClaimTypes.PrimarySid,login_result.IdUsuario.ToString()),
                    new Claim(ClaimTypes.DenyOnlySid,login_result.IdCliente.ToString()),
                    new Claim(ClaimTypes.PostalCode,login_result.TipoCliente.ToString()),     
                    new Claim(ClaimTypes.NameIdentifier, model.email),
                    new Claim(ClaimTypes.Email, model.email),
                    new Claim(ClaimTypes.GivenName, login_result.Iniciales),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",model.email)                    
                };
                var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                AuthenticationManager.SignIn(id);



                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecuperarPassword()
        {
            return View();
        }


        public ActionResult ConfirmarSMS(string id)
        {


            if (string.IsNullOrEmpty(id))
                return new HttpNotFoundResult();

            Domain.DataAccess.RegistroCliente registro = new Domain.DataAccess.RegistroCliente();

            Guid secretId = Guid.NewGuid();

            try { secretId = Guid.Parse(id); }
            catch (Exception) { return new HttpNotFoundResult(); }

            Domain.BaseResponse<string> result = registro.envioMsMCliente(secretId);

            if (!result.success)
            {
                if (result.error.Equals("Mensaje ya usado."))
                    return RedirectToAction("", "Home");
                else
                    return Content(result.error);
            }

            MesaDinero.Domain.Model.RegistroClientesRequest model = new Domain.Model.RegistroClientesRequest();
            model.email = id;
            model.phone = result.data;

            return View(model);
        }

        public ActionResult CambiarPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpNotFoundResult();

            Guid secretId = Guid.NewGuid();

            try { secretId = Guid.Parse(id); }
            catch (Exception) { return new HttpNotFoundResult(); }

            MesaDinero.Domain.CambioPassWordRequest model = new CambioPassWordRequest();
            using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
            {
                MesaDinero.Data.PersistenceModel.Tb_MD_RecuperarPassword recuperar = null;
                recuperar = context.Tb_MD_RecuperarPassword.FirstOrDefault(x => x.SecredId == secretId);

                if(recuperar == null)
                    return new HttpNotFoundResult();

                if (DateTime.Now > recuperar.FechaExpiracion)
                    return new HttpNotFoundResult();

                model.email = recuperar.Email;
                model.sid = recuperar.SecredId.ToString();


            }


            return View(model);
        }

        public ActionResult ModificarPassword()
        {
            return View();
        }

   



    }
}