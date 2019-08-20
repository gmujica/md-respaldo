using MesaDinero.Data.PersistenceModel;
using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model.Admin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess.Admin
{
    public class TipoDocumentoDataAccess
    {
        //public BaseResponse<List<TipoDocumentoResponse>> listarTipoDocumentos()
        //{
        //    BaseResponse<List<TipoDocumentoResponse>> resultado = new BaseResponse<List<TipoDocumentoResponse>>();
        //    resultado.data = new List<TipoDocumentoResponse>();

        //    try
        //    {
        //        using (MesaDineroContext context = new MesaDineroContext())
        //        {
        //            resultado.data = context.Tb_MD_TipoDocumento.Where(x => x.EstadoRegistro == EstadoRegistroTabla.Activo).OrderBy(x => x.Nombre).Select(x => new TipoDocumentoResponse { 
        //             codigo = x.IdTipoDocumento,
        //             nombre = x.Nombre,
        //             tipo = x.Tipo
        //            }).ToList();
        //        }


        //        resultado.success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        resultado.success = false;
        //    }


        //    return resultado;

        //}


        public PageResultSP<TipoDocumentoResponse> getTipoDocumento(PageResultParam param)
        {
            PageResultSP<TipoDocumentoResponse> result = new PageResultSP<TipoDocumentoResponse>();
            result.data = new List<TipoDocumentoResponse>();

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
                    result.data = context.Database.SqlQuery<TipoDocumentoResponse>("exec Proc_Sel_TipoDocumento @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<TipoDocumentoResponse>();

                    if (result.data.Count > 0)
                    {
                        total = Convert.ToInt32(result.data[0].total);
                    }
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

        public BaseResponse<string> insertNewTipoDocumento(TipoDocumentoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_TipoDocumento tipo = context.Tb_MD_TipoDocumento.Find(model.codigo);
                        if (tipo != null)
                        {
                            throw new Exception("Codigo ya existente,Ingrese otro codigo");
                        }

                        Tb_MD_TipoDocumento tipodoc = new Tb_MD_TipoDocumento();
                        tipodoc.IdTipoDocumento = model.codigo;
                        tipodoc.Nombre = model.nombre;
                        tipodoc.Tipo = model.tipo;
                        tipodoc.EstadoRegistro = model.estado;

                        context.Tb_MD_TipoDocumento.Add(tipodoc);

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






        public BaseResponse<string> EditarTipoDocumento(TipoDocumentoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_TipoDocumento tipo = context.Tb_MD_TipoDocumento.Find(model.codigo);
                        if (tipo == null)
                        {
                            throw new Exception("Entidad Nula, Tipo Documento no encontrado");
                        }

                        tipo.Nombre = model.nombre;
                        tipo.Tipo = model.tipo;
                        tipo.EstadoRegistro = model.estado;

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



        public BaseResponse<string> EliminarTipoDocumento(TipoDocumentoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_TipoDocumento tipodoc = context.Tb_MD_TipoDocumento.Find(model.codigo);
                        if (tipodoc == null)
                        {
                            throw new Exception("Entidad Nula, Tipo Documento no encontrado");
                        }
                        tipodoc.EstadoRegistro = EstadoRegistroTabla.Eliminado;

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
