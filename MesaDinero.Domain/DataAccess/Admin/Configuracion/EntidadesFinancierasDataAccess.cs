using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace MesaDinero.Domain.DataAccess.Admin
{
    public class EntidadesFinancierasDataAccess
    {

        public PageResultSP<EntidadFinancieraResponses> getAllEntidadesFinancieras(PageResultParam param)
        {
            PageResultSP<EntidadFinancieraResponses> result = new PageResultSP<EntidadFinancieraResponses>();
            result.data = new List<EntidadFinancieraResponses>();

            try
            {
                int page = param.pageIndex + 1;
                int total = 0;


                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                #endregion


                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<EntidadFinancieraResponses>("exec Proc_Sel_Entidades_Financieras @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<EntidadFinancieraResponses>();

                    if (result.data.Count > 0)
                    {
                        total = Convert.ToInt32(result.data[0].total);
                    }
                }

                string host = System.Configuration.ConfigurationManager.AppSettings["HostAdmin"];
                foreach (var d in result.data)
                {
                    d.logo = host + d.logo + "?t=" + string.Format("{0:hhMMss}",DateTime.Now);
                }




                #region Copiar Al Cual
                var pag = Utilities.ResultadoPagination(page, param.itemPerPage, total);

                result.itemperpage = pag.itemperpage;
                result.limit = pag.limit;
                result.numbersPages = pag.numbersPages;
                result.offset = pag.offset;
                result.page = pag.page;
                result.PageCount = pag.pageCount;
                result.total = pag.total;
                result.success = true;
                #endregion


            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;
            }


            return result;
        }

        public BaseResponse<string> insertNewEntidadFinanciera(EntidadFinancieraRequest model,HttpPostedFile file)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Entidades_Financieras entidad = context.Tb_MD_Entidades_Financieras.Find(model.codigo);
                        if (entidad != null)
                        {
                            throw new Exception("Codigo ya existente,Ingrese otro codigo");
                        }
                        //"../../MesaDinero.Web/Content/images/bancos/"
                        string logo = "";
                        if (file != null) {
                            
                            //string proyectoruta = System.Web.HttpContext.Current.Server.MapPath("~/MesaDinero.Web/Content/images/bancos/");
                            //string rutaCorta = ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                            //string rutaRaiz =  System.Web.HttpContext.Current.Server.MapPath("~" + rutaCorta);

                            string rutaCorta = ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                            string rutaRaiz =ConfigurationManager.AppSettings["RUTA_RAIZ_BANCO"]+ rutaCorta;
                            string rutaRaiz_web = ConfigurationManager.AppSettings["RUTA_RAIZ_WEB_BANCO"] + rutaCorta;
                            

                            string nombreImagen = model.codigo.Trim();
                            string extension = Path.GetExtension(file.FileName);

                            string rutaCompleta = rutaRaiz + nombreImagen + extension;
                            string rutaCompleta_web = rutaRaiz_web + nombreImagen + extension;

                            logo= rutaCorta + nombreImagen + extension;
                            file.SaveAs(rutaCompleta);
                            file.SaveAs(rutaCompleta_web);

                        }
                    
                        Tb_MD_Entidades_Financieras entidadFinanciera = new Tb_MD_Entidades_Financieras();
                        entidadFinanciera.vCodEntidad = model.codigo.Trim();
                        entidadFinanciera.vDesEntidad = model.nombre;
                        entidadFinanciera.vLogoEntidad = logo;
                        entidadFinanciera.dFechaCreacion = DateTime.Now;
                        entidadFinanciera.VTipo = model.tipo;
                        entidadFinanciera.vFormatoCCI = model.formatoCCI;
                        entidadFinanciera.vFormatoCuentaBancaria = model.formatoCB;
                        entidadFinanciera.iEstadoRegistro = model.estado;
                        context.Tb_MD_Entidades_Financieras.Add(entidadFinanciera);
                        context.SaveChanges();
                        transaccion.Commit();

                        result.success = true;

                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        #region Error EntityFramework
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        var fullErrorMessage = string.Join("; ", errorMessages);

                        result.success = false;
                        result.error = fullErrorMessage;
                        transaccion.Rollback();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.success = false;

                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }

                }
            }

            return result;
        }

        public BaseResponse<string> EditarEntidadFinanciera(EntidadFinancieraRequest model, HttpPostedFile file)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Entidades_Financieras entidadFinanciera = context.Tb_MD_Entidades_Financieras.Find(model.codigo.Trim());
                        if (entidadFinanciera == null)
                        {
                            throw new Exception("Entidad Nula, Entidad Financiera no encontrado");
                        }
                        //Tb_MD_Cargo cargo = new Tb_MD_Cargo();
                        //context.Tb_MD_Cargo.Add(cargo);
                        string logo = "";
                        //if (file != null)
                        //{
                        //    string proyectoruta = System.Web.HttpContext.Current.Server.MapPath("~/MesaDinero.Web/Content/images/bancos/");
                        //    string rutaCorta = ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                        //    string rutaRaiz = System.Web.HttpContext.Current.Server.MapPath("~" + rutaCorta);
                          
                        //    string nombreImagen = model.codigo.Trim();
                        //    string extension = Path.GetExtension(file.FileName);
                        //    string rutaCompleta = rutaRaiz + nombreImagen + extension;
                        //    //if (model.logo != "") {
                        //    //    File.Delete(System.Web.HttpContext.Current.Server.MapPath("~" + model.logo));
                        //    //}

                        //    logo = rutaCorta + nombreImagen + extension;
                        //    file.SaveAs(rutaCompleta);
                            

                        //}

                        if (file != null)
                        {

                            //string proyectoruta = System.Web.HttpContext.Current.Server.MapPath("~/MesaDinero.Web/Content/images/bancos/");
                            //string rutaCorta = ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                            //string rutaRaiz =  System.Web.HttpContext.Current.Server.MapPath("~" + rutaCorta);

                            string rutaCorta = ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                            string rutaRaiz = ConfigurationManager.AppSettings["RUTA_RAIZ_BANCO"] + rutaCorta;
                            string rutaRaiz_web = ConfigurationManager.AppSettings["RUTA_RAIZ_WEB_BANCO"] + rutaCorta;


                            string nombreImagen = model.codigo.Trim();
                            string extension = Path.GetExtension(file.FileName);

                            string rutaCompleta = rutaRaiz + nombreImagen + extension;
                            string rutaCompleta_web = rutaRaiz_web + nombreImagen + extension;

                            logo = rutaCorta + nombreImagen + extension;
                            file.SaveAs(rutaCompleta);
                            file.SaveAs(rutaCompleta_web);

                        }

                        entidadFinanciera.vDesEntidad = model.nombre;
                        if (logo != "") {
                            entidadFinanciera.vLogoEntidad = logo;
                        }

                        entidadFinanciera.dFechaModificacion = DateTime.Now;
                        entidadFinanciera.VTipo = model.tipo;
                        entidadFinanciera.vFormatoCCI = model.formatoCCI;
                        entidadFinanciera.vFormatoCuentaBancaria = model.formatoCB;
                        entidadFinanciera.iEstadoRegistro = model.estado;


                        context.SaveChanges();
                        transaccion.Commit();

                        result.success = true;

                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        #region Error EntityFramework
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        var fullErrorMessage = string.Join("; ", errorMessages);

                        result.success = false;
                        result.error = fullErrorMessage;
                        transaccion.Rollback();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.success = false;

                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }

                }
            }

            return result;
        }

        public BaseResponse<string> EliminarEntidadFinanciera(EntidadFinancieraRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Entidades_Financieras cargo = context.Tb_MD_Entidades_Financieras.Find(model.codigo);
                        if (cargo == null)
                        {
                            throw new Exception("Entidad Nula, Entidad Financiera no encontrado");
                        }
                        cargo.iEstadoRegistro = EstadoRegistroTabla.Eliminado;

                        context.SaveChanges();
                        transaccion.Commit();

                        result.success = true;

                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        #region Error EntityFramework
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        var fullErrorMessage = string.Join("; ", errorMessages);

                        result.success = false;
                        result.error = fullErrorMessage;
                        transaccion.Rollback();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.success = false;

                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }

                }
            }

            return result;
        }



    }
}
