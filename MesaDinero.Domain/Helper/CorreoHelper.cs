using System.Text;
using System.Web;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Mail;
using System;
using System.Threading.Tasks;

namespace MesaDinero.Domain.Helper
{
    public static class CorreoHelper
    {
        private static string SMTP = ConfigurationManager.AppSettings["SMTP"];
        private static string FROM_MAIL = ConfigurationManager.AppSettings["FROM_MAIL"];
        private static string FROM_PASS = ConfigurationManager.AppSettings["FROM_PASS"];
        private static string FROM_NOMBRE = ConfigurationManager.AppSettings["FROM_NAME"];
        private static string PORT = ConfigurationManager.AppSettings["PORT"];
        private static string HOST_WEB = ConfigurationManager.AppSettings["HostWeb"];
        private static string HOST_ADM = ConfigurationManager.AppSettings["HostAdmin"];
        public static async Task<bool> SendCorreoRegistroExitozo(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Verificación de Datos Exitosa";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\CreacionPassword.html";

             

                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendCorreoRegistroUsuario(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Verificación de Datos Exitosa";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\CreacionPasswordAutorizado.html";



                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendCorreoConfirmacionUsuario(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Verificación de Datos Exitosa";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\ConfirmacionCuentas.html";



                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        


        public static async Task<bool> SendCorreoRegistroCliente(string email, string secret, string nombre, string estado)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Verificación de Datos " + estado;

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\RegistroObRe.html";

                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{estado}}", estado);
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendCorreoRegistroCuentaCliente(string email, string nombre, string estado)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Verificación de Datos " + estado;

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\RegistroObReCuentas.html";

                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{estado}}", estado);
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendCorreoRegistroModificarCliente(string email, string nombre, string estado)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Verificación de Datos " + estado;

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\RegistroObReModificacion.html";

                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{estado}}", estado);
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendCorreoRestaurarProcesoRegistro(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Restaurar Proceso de Registro";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\RestaurarRegistro.html";



                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SedCorreoRecuperarContrasenha(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Recuperar tu Contraseña";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\RecuperarContrasena.html";



                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_WEB);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SendCorreoPagoCliente(string email, string nombre, string monto, string moneda,string secrectId)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Confirmación de Pago";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\ConfirmacionPago.html";
                //CreacionPassword
                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{monto}}", monto);
                body = body.Replace("{{moneda}}", moneda);
                body = body.Replace("{{host}}", HOST_WEB);
                body = body.Replace("{{secredId}}", secrectId);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /*Correo Generar Pwd para administrativos*/
        public static async Task<bool> SendCorreoRegistroAdmExitoso(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Crear Password";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\CreacionPasswordAdmin.html";



                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_ADM);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> SedCorreoRecuperarContrasenhaAdmin(string email, string secret, string nombre)
        {
            try
            {
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress(FROM_MAIL, FROM_NOMBRE, Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "LMD - Recuperar tu Contraseña";

                string body = "";
                string ruta_plantilla = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Plantillas\\Html\\RecuperarContrasenaAdmin.html";



                using (StreamReader reader = new StreamReader(ruta_plantilla))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{{nombre}}", nombre);
                body = body.Replace("{{secret}}", secret);
                body = body.Replace("{{host}}", HOST_ADM);


                mail.IsBodyHtml = true;
                mail.Body = body;
                // Se Adjunta el correo del receptor
                mail.To.Add(email);
                SmtpServer.Port = Convert.ToInt32(PORT); //Puerto que utiliza Gmail para sus servicios
                SmtpServer.Credentials = new System.Net.NetworkCredential(FROM_MAIL, FROM_PASS);
                SmtpServer.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
               delegate(object s
                   , X509Certificate certificate
                   , X509Chain chai
                   , SslPolicyErrors sslPolicyErrors)
               { return true; };


                SmtpServer.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




    }
}
