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
using System.Text.RegularExpressions;
namespace MesaDinero.Web.Controllers
{
    public class RegistroController : Controller
    {

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //
        // GET: /Registro/

        private MesaDinero.Data.PersistenceModel.Tb_MD_Pre_Clientes mCliente = null;
        private MesaDinero.Data.PersistenceModel.Tb_MD_ClienteUsuario mClienteUsuario = null;

        public void CargarLogicaRegistro(string secredId)
        {
            MesaDinero.Domain.DataAccess.CommonDataAccess _common = new Domain.DataAccess.CommonDataAccess();
            Guid sid = Guid.NewGuid();
            try
            {
                sid = Guid.Parse(secredId);
            }
            catch (Exception)
            {
                return;
            }
            mCliente = _common.getPreClienteBySecredId(sid);
            

        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Continuar(string id)
        {
            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();
            else if (mCliente.Seguimiento == SeguimientoRegistro.IngresoClaveSMS)
                return RedirectToAction("ConfirmarSMS", "Acceso", new { id = id });
            else if (mCliente.Seguimiento == SeguimientoRegistro.RegistroBatosPrincipales)
                return RedirectToAction("DatosBasicos", new { id = id });
            else if (mCliente.Seguimiento == SeguimientoRegistro.RegistroPersonaAutorizada)
                return RedirectToAction("PersonaAutorizada", new { id = id });
            else if (mCliente.Seguimiento == SeguimientoRegistro.RegistroDatosBancarios)
                return RedirectToAction("DatosBancarios", new { id = id });
            else if (mCliente.Seguimiento == SeguimientoRegistro.RegistroProcesoValidacion || mCliente.Seguimiento == SeguimientoRegistro.PreProcesoValidacion)
                return RedirectToAction("Verificacion", new { id = id });
            else if (mCliente.Seguimiento == SeguimientoRegistro.CrearPassword){
                Session["modo"] = 1;
                return RedirectToAction("Password", new { id = id });
            }
            
            else if (mCliente.Seguimiento == SeguimientoRegistro.PostCrearPasswords){
                Session["modo"] = 1;
                return RedirectToAction("Password", new { id = id });
            }
            
            else
                return new HttpNotFoundResult();

        }

        public ActionResult ContinuarUsuario(string id)
        {
            //MesaDinero.Domain.DataAccess.CommonDataAccess _common = new Domain.DataAccess.CommonDataAccess();
            //Guid sid = Guid.NewGuid();
            //sid = Guid.Parse(id);

            //mCliente = _common.getUsuarioClienteBySecredId(sid);

            ////CargarLogicaRegistro(id);

            if (id == null)
                return new HttpNotFoundResult();


            //if (mCliente.Seguimiento == SeguimientoRegistro.PostCrearPasswords)
            Session["modo"] = 2;
                return RedirectToAction("Password", new { id = id });
            //else
            //    return new HttpNotFoundResult();

        }

        public ActionResult Subasta(string id)
        {
            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

           


            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();

             using (MesaDinero.Data.PersistenceModel.MesaDineroContext context = new Data.PersistenceModel.MesaDineroContext())
            {

                model.tiempoSubasta = context.Tb_MD_Tiempos.First(x => x.vCodTransaccion.Equals("T_Sb")).nTiempoStandar ?? 0;
                model.partners = context.Database.SqlQuery<string>("exec proc_sel_partners_inicio_subasta").ToList<string>();
            }

            model.seguimiento = mCliente.Seguimiento;
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.header.seguimiento = "SM1";
            model.nombres = MesaDineroHelper.getNombreCliente(mCliente);
            model.userName = MesaDineroHelper.getIniciales(mCliente);
            //model.userName = model.userName.ToUpper(); 
            ViewBag.sid = mCliente.SecretId.ToString();
            return View(model);
        
        }

        public ActionResult Operaciones(string id) 
        {
            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();


            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            

            model.estado = mCliente.EstadoValidacion;

            model.seguimiento = mCliente.Seguimiento;
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.header.seguimiento = "";
           

            ViewBag.inicialesCliente = MesaDineroHelper.getIniciales(mCliente);
            ViewBag.nombreCliente = MesaDineroHelper.getNombreCliente(mCliente);

            model.nombres = MesaDineroHelper.getNombreCliente(mCliente);
            model.userName = MesaDineroHelper.getIniciales(mCliente);

            ViewBag.sid = mCliente.SecretId.ToString();


            return View(model);
        }

        public ActionResult MisDatos(string id)
        {

            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            //if (mCliente.Finalizado)
            //    return new HttpNotFoundResult();

            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.nroDocumento = mCliente.vNroDocumento;
            model.nombres = mCliente.vNombre;
            string[] apellidos = mCliente.vApellido.Split(' ');
            model.apePaterno = apellidos[0];
            if (apellidos.Length > 1)
                model.apeMaterno = apellidos[1];
            else
                model.apeMaterno = "";

            if (mCliente.vTipoCliente == TipoCliente.PersonaNatural)
            {
                model.header.seguimiento = "PDP";

                if (mCliente.iEstadoNavegacion == 4)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = false;
                }

            }
            else
            {
                model.header.seguimiento = "EDP";
            }

            ViewBag.inicialesCliente = MesaDineroHelper.getIniciales(mCliente);
            ViewBag.nombreCliente = MesaDineroHelper.getNombreCliente(mCliente);

            model.header.secredId = id;
            model.header.estadoNavegacion = mCliente.iEstadoNavegacion;
            model.header.tipoCliente = mCliente.vTipoCliente;


            model.email = mCliente.vEmail;
            model.celular = mCliente.vCelular;

            model.userName = MesaDineroHelper.getIniciales(mCliente);

            ViewBag.sid = mCliente.SecretId.ToString();

            return View(model);

        }

        public ActionResult MisCuentasBanco(string id)
        {
            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;

            if (model.tipoCliente == TipoCliente.PersonaNatural)
            {
                model.header.seguimiento = "PDB";

                if (mCliente.iEstadoNavegacion == 4)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }

            }
            else
            {
                if (mCliente.iEstadoNavegacion == 5)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }

                model.header.seguimiento = "EDB";
            }

            model.header.secredId = id;
            model.header.estadoNavegacion = mCliente.iEstadoNavegacion;
            model.header.tipoCliente = mCliente.vTipoCliente;

            ViewBag.inicialesCliente = MesaDineroHelper.getIniciales(mCliente);
            ViewBag.nombreCliente = MesaDineroHelper.getNombreCliente(mCliente);

            model.userName = MesaDineroHelper.getNombreCliente(mCliente);


            ViewBag.sid = mCliente.SecretId.ToString();

            return View(model);

        }

        public ActionResult DatosBasicos(string id)
        {            
           CargarLogicaRegistro(id);

            if(mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

            //if (mCliente.Seguimiento != SeguimientoRegistro.RegistroBatosPrincipales)
            //    return new HttpNotFoundResult();

            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.nroDocumento = mCliente.vNroDocumento;
            model.nombres = mCliente.vNombre;
            string[] apellidos = mCliente.vApellido.Split(' ');
            model.apePaterno = apellidos[0];
            if (apellidos.Length > 1)
                model.apeMaterno = apellidos[1];
            else
                model.apeMaterno = "";

            if(mCliente.vTipoCliente == TipoCliente.PersonaNatural)
            {
                model.header.seguimiento = "PDP";

                if (mCliente.iEstadoNavegacion == 4)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }
                
            }
            else
            {
                if (mCliente.iEstadoNavegacion == 5)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }

                model.header.seguimiento = "EDP";
                model.nombres = mCliente.nombreEmpresa;
               
            }

            model.userName = MesaDineroHelper.getIniciales(mCliente);

            model.header.secredId = id;
            model.header.estadoNavegacion = mCliente.iEstadoNavegacion;
            model.header.tipoCliente = mCliente.vTipoCliente;

            model.email = mCliente.vEmail;
            model.celular = mCliente.vCelular;


            return View(model);
        }

        public ActionResult PersonaAutorizada(string id)
        {
            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

            //if (mCliente.Seguimiento != SeguimientoRegistro.RegistroPersonaAutorizada)
            //    return new HttpNotFoundResult();

            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            model.sid = id;
            model.header.seguimiento = "EPA";

            model.nroDocumento = mCliente.nroDocumentoContacto;
            model.nombres = mCliente.vNombre;
            string[] apellidos = mCliente.vApellido.Split(' ');
            model.apePaterno = apellidos[0];
            if (apellidos.Length > 1)
                model.apeMaterno = apellidos[1];
            else
                model.apeMaterno = "";


            if(mCliente.vTipoCliente == TipoCliente.PersonaJuridica)
            {
                if (mCliente.iEstadoNavegacion == 5)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }
            }


            model.email = mCliente.vEmail;
            model.celular = mCliente.vCelular;

            model.header.secredId = id;
            model.header.estadoNavegacion = mCliente.iEstadoNavegacion;
            model.header.tipoCliente = mCliente.vTipoCliente;

            model.userName = MesaDineroHelper.getIniciales(mCliente);

            return View(model);
        }

        public ActionResult DatosBancarios(string id)
        {

            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;

            if (model.tipoCliente == TipoCliente.PersonaNatural)
            {
                if (mCliente.iEstadoNavegacion <= 1)
                    return new HttpNotFoundResult();

                model.header.seguimiento = "PDB";

                if(mCliente.iEstadoNavegacion == 4)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }
            }
            else 
            {
                if (mCliente.iEstadoNavegacion <= 2)
                    return new HttpNotFoundResult();

                model.header.seguimiento = "EDB";

                if (mCliente.iEstadoNavegacion == 5)
                {
                    model.editar = false;
                }
                else
                {
                    model.editar = true;
                }
            }

            model.userName = MesaDineroHelper.getIniciales(mCliente); ;


            model.header.secredId = id;
            model.header.estadoNavegacion = mCliente.iEstadoNavegacion;
            model.header.tipoCliente = mCliente.vTipoCliente;

            return View(model);
        }

        public ActionResult Verificacion(string id)
        {
            CargarLogicaRegistro(id);

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

            if (mCliente.iEstadoNavegacion <=2)
                return new HttpNotFoundResult();


            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.estado = mCliente.EstadoValidacion;
            model.comentario = mCliente.ComentarioOperador;

            if (model.tipoCliente == TipoCliente.PersonaNatural)
            {
                model.header.seguimiento = "PV";
              
            }
            else
            {
                model.header.seguimiento = "EV";
            }

            model.seguimiento = mCliente.Seguimiento;

            model.header.secredId = id;
            model.header.estadoNavegacion = mCliente.iEstadoNavegacion;
            model.header.tipoCliente = mCliente.vTipoCliente;

           model.userName =  MesaDineroHelper.getIniciales(mCliente);
           
            model.fdatosBasicos = (DateTime)mCliente.dFechaValidacionPaso2;
            model.fdatosBancarios = (DateTime)mCliente.dFechaValidacionPaso3;
            model.nombres = mCliente.NombreCliente;

            return View(model);
        } 
         //public ActionResult Password(string id, int modo=0)
        public ActionResult Password(string id)
        {

            MesaDinero.Domain.DataAccess.CommonDataAccess _common = new Domain.DataAccess.CommonDataAccess();
            int modo = Convert.ToInt16(Session["modo"].ToString());
            if(modo==2){
                Guid sid = Guid.NewGuid();
                sid = Guid.Parse(id);
                mCliente = _common.getUsuarioClienteBySecredId(sid);
            }
           
            if(modo==1){
                CargarLogicaRegistro(id);
            }
            

            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();

            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();

            if (mCliente.Seguimiento == SeguimientoRegistro.PostCrearPasswords)
            {
                Guid sid = Guid.NewGuid();
                sid = Guid.Parse(id);
                mClienteUsuario = _common.getUsuarioAutorizadoBySecredId(sid);
                mCliente.vTipoCliente = mClienteUsuario.TipoCliente;
                mCliente.vEmail = mClienteUsuario.Email;
                model.opcionUsuario = 2;
            }
            else {
                if (mCliente.Seguimiento != SeguimientoRegistro.CrearPassword)
                    return new HttpNotFoundResult();
                model.opcionUsuario = 1;
            }
          
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.email = mCliente.vEmail;


            return View(model);
        }

        [HttpPost]
        public ActionResult Password(RegistroCrearPassWord_Request model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            try
            {
                if(model.password.Length < 6 || model.password.Length > 10)
                    throw new Exception("La contraseña debe tener entre 6 a 10 caracteres");

                bool hashLetter = MesaDineroHelper.hashLetter(model.password), hashNumber = MesaDineroHelper.hashNumber(model.password);
                
                if (!hashLetter || !hashNumber)
                    throw new Exception("La contraseña debe constar de letras y numeros");

                if (model.password != model.rePassword)
                    throw new Exception("Las contraseñas no coinciden. Por favor asegurece de haber escrito bien su contraseña en ambos campos");

                
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

                return RedirectToAction("", "Inicio");

            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                TempData["error"] = result.error;
                return RedirectToAction("Password", new { id = model.sid});
            }
        }



        [HttpPost]
        public ActionResult VerificacionOperador(Datos model)
        {

            try 
	        {	        
		     Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
                _registroDataAccess.validarRegistroOperador(model.sid);
                return RedirectToAction("Password", new { id = model.sid });
	        }
	        catch (Exception ex)
	        {
                return Content(string.Format("Error: {0}",ex.Message));
	        }

            
        }

        public FileResult Descargar(string ruc)
        {
            // Obtener contenido del archivo
            //string text = "El texto para mi archivo.";
            //var stream = new MemoryStream(Encoding.ASCII.GetBytes(text));

            //return File(stream, "text/plain", "Prueba.txt");
            MesaDinero.Domain.DataAccess.RegistroCliente _clienteDataAccess = new MesaDinero.Domain.DataAccess.RegistroCliente();
            MesaDinero.Domain.Model.operaciones.ExportFiles file = new MesaDinero.Domain.Model.operaciones.ExportFiles();
            file = _clienteDataAccess.descargarDocumento(ruc);

            byte[] byteArray = file.FileBytes;
            String NombreArchivo = file.Name.ToString();
            String ExtensionArchivo = file.Extension.ToString();
            //MemoryStream ms = new MemoryStream(byteArray,false);

            return File(byteArray, "text/" + ExtensionArchivo, NombreArchivo  + ExtensionArchivo);
        }

        
        

        [HttpPost]
        public ActionResult CrearPassword(Domain.Model.RegistroCrearPassWord_Request model)
        {
            try
            {
                //Domain.DataAccess.RegistroCliente _registroDataAccess = new Domain.DataAccess.RegistroCliente();
                //BaseResponse<Domain.Model.RegistroPassWpord_Response> result = _registroDataAccess.CrearPasswprd(model);

                //var model1 = result.data;

                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.WindowsAccountName, model1.email),
                //    new Claim(ClaimTypes.Name, model1.email),
                //    new Claim(ClaimTypes.Actor, model1.nroDocumento),
                //    new Claim(ClaimTypes.Role, ""),
                //    new Claim(ClaimTypes.Country,model.sid),
                //    new Claim(ClaimTypes.PostalCode,model1.tipoCliente.ToString()),     
                //    new Claim(ClaimTypes.NameIdentifier, model1.email),
                //    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",model1.email)
                //    //new Claim(ClaimTypes., curUser.UserGroupID.ToString())
                //};
                //var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                //var ctx = Request.GetOwinContext();
                //AuthenticationManager.SignIn(id);


                
                


                return RedirectToAction("", "Inicio");

            }
            catch (Exception ex)
            {
                return Content(string.Format("Error: {0}",ex.Message));
            }
 
        }

        public ActionResult ListaOperacion(string id)
        {
            CargarLogicaRegistro(id);
            if (mCliente == null)
                return new HttpNotFoundResult();

            if (mCliente.Finalizado)
                return new HttpNotFoundResult();


            MesaDinero.Domain.Model.RegistroResponse model = new Domain.Model.RegistroResponse();


            model.estado = mCliente.EstadoValidacion;

            model.seguimiento = mCliente.Seguimiento;
            model.sid = id;
            model.tipoCliente = mCliente.vTipoCliente;
            model.header.seguimiento = "";


            ViewBag.inicialesCliente = MesaDineroHelper.getIniciales(mCliente);
            ViewBag.nombreCliente = MesaDineroHelper.getNombreCliente(mCliente);

            model.nombres = MesaDineroHelper.getNombreCliente(mCliente);
            model.userName = MesaDineroHelper.getIniciales(mCliente);

            ViewBag.sid = mCliente.SecretId.ToString();
            return View(model);
        }



    }

    public class Datos
    {
        public string sid { get; set; }
    }

}