using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model.Admin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess.Admin.Configuracion
{
    public class CargoDataAccess
    {
        public PageResultSP<CargoResponse> getAllCargos(PageResultParam param)
        {
            PageResultSP<CargoResponse> result = new PageResultSP<CargoResponse>();
            result.data = new List<CargoResponse>();

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
                    result.data = context.Database.SqlQuery<CargoResponse>("exec Proc_Sel_Cargos @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<CargoResponse>();

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

        public BaseResponse<string> insertNewCargo(CargoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Cargo cargo  = new Tb_MD_Cargo();
                        cargo.Nombre = model.nombre;
                        cargo.iEstadoRegistro = model.estado;
                        context.Tb_MD_Cargo.Add(cargo);

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

        public BaseResponse<string> EditarCargo(CargoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Cargo cargo = context.Tb_MD_Cargo.Find(model.codigo);
                        if (cargo == null) {
                            throw new Exception("Entidad Nula, Cargo no encontrado");
                        }
                        //Tb_MD_Cargo cargo = new Tb_MD_Cargo();
                        cargo.Nombre = model.nombre;
                        cargo.iEstadoRegistro = model.estado;
                        //context.Tb_MD_Cargo.Add(cargo);

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

        public BaseResponse<string> EliminarCargo(CargoRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Cargo cargo = context.Tb_MD_Cargo.Find(model.codigo);
                        if (cargo == null)
                        {
                            throw new Exception("Entidad Nula, Cargo no encontrado");
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
