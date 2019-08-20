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
    public class UbigeoDataAccess
    {

        public PageResultSP<UbigeoResponse> getAllUbigeo(PageResultParam param)
        {
            PageResultSP<UbigeoResponse> result = new PageResultSP<UbigeoResponse>();
            result.data = new List<UbigeoResponse>();

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
                    result.data = context.Database.SqlQuery<UbigeoResponse>("exec Proc_Sel_Ubigeo @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<UbigeoResponse>();

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

        public BaseResponse<string> insertNewUbigeo(UbigeoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Ubigeo ubigeo = new Tb_MD_Ubigeo();
                        ubigeo.CodPais = model.codigoPais;
                        ubigeo.CodDepartamento = model.codigoDepartamento;
                        ubigeo.CodProvincia = model.codigoProvincia;
                        ubigeo.CodDistrito = model.codigoDistrito;
                        ubigeo.iEstadoRegistro = model.estado;
                        context.Tb_MD_Ubigeo.Add(ubigeo);

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

        public BaseResponse<string> EditarUbigeo(UbigeoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Ubigeo ubigeo = context.Tb_MD_Ubigeo.Find(model.codigo);
                        if (ubigeo == null)
                        {
                            throw new Exception("Entidad Nula, Registro de Ubigeo no encontrado");
                        }

                        ubigeo.CodPais = model.codigoPais;
                        ubigeo.CodDepartamento = model.codigoDepartamento;
                        ubigeo.CodProvincia = model.codigoProvincia;
                        ubigeo.CodDistrito = model.codigoDistrito;
                        ubigeo.iEstadoRegistro = model.estado;
                        

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

        public BaseResponse<string> EliminarUbigeo(UbigeoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Ubigeo ubigeo = context.Tb_MD_Ubigeo.Find(model.codigo);
                        if (ubigeo == null)
                        {
                            throw new Exception("Entidad Nula, Registro de Ubigeo no encontrado");
                        }
                        ubigeo.iEstadoRegistro = EstadoRegistroTabla.Eliminado;

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

        public BaseResponse<List<ComboListItem>> getProvincia()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Provincia.Where(x => x.iEstadoRegistro==EstadoRegistroTabla.Activo).Select(x => new ComboListItem
                    {
                        value = x.IdProvincia,
                        text = x.Nombre
                    }).ToList();
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.ex = ex;
                result.success = false;
            }


            return result;
        }

        public BaseResponse<List<ComboListItem>> getDistrito()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Distrito.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new ComboListItem
                    {
                        value = x.IdDistrito,
                        text = x.Nombre
                    }).ToList();
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.ex = ex;
                result.success = false;
            }


            return result;
        }

    }
}
