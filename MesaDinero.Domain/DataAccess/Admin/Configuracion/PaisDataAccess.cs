using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model.Admin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess.Admin
{
    public class PaisDataAccess
    {
        public PageResultSP<PaisResponse> getAllPais(PageResultParam param)
        {
            PageResultSP<PaisResponse> result = new PageResultSP<PaisResponse>();
            result.data = new List<PaisResponse>();

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
                    result.data = context.Database.SqlQuery<PaisResponse>("exec Proc_Sel_Pais @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<PaisResponse>();

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

        public BaseResponse<string> insertNewPais(PaisRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Pais vpais = context.Tb_MD_Pais.Find(model.codigo);
                        if (vpais != null)
                        {
                            throw new Exception("Codigo ya existente,Ingrese otro codigo");
                        }

                        Tb_MD_Pais pais = new Tb_MD_Pais();
                        pais.IdPais = model.codigo;
                        pais.Nombre = model.nombre;
                        pais.iEstadoRegistro = model.estado;
                        context.Tb_MD_Pais.Add(pais);

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

        public BaseResponse<string> EditarPais(PaisRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Pais pais = context.Tb_MD_Pais.Find(model.codigo);
                        if (pais == null)
                        {
                            throw new Exception("Entidad Nula, Pais no encontrado");
                        }

                        pais.Nombre = model.nombre;
                        pais.iEstadoRegistro = model.estado;

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

        public BaseResponse<string> EliminarPais(PaisRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Pais pais = context.Tb_MD_Pais.Find(model.codigo);
                        if (pais == null)
                        {
                            throw new Exception("Entidad Nula, Cargo no encontrado");
                        }
                        pais.iEstadoRegistro = EstadoRegistroTabla.Eliminado;

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
