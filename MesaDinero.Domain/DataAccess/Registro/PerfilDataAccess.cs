using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Helper;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MesaDinero.Domain.DataAccess.Registro
{
    public partial class PerfilDataAccess
    {

        public PageResultSP<PerfilResponse> getAllPerfil(PageResultParam param)
        {
            PageResultSP<PerfilResponse> result = new PageResultSP<PerfilResponse>();
            result.data = new List<PerfilResponse>();

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
                    result.data = context.Database.SqlQuery<PerfilResponse>("exec Proc_Sel_Perfil @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<PerfilResponse>();

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

        public BaseResponse<string> insertNewPerfil(PerfilRequest model,string usuarioDoc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Perfiles perfil = new Tb_MD_Perfiles();
                        perfil.NombrePerfil = model.nombre;
                        perfil.EstadoRegistro = model.estado;
                        perfil.FechaCreacion = DateTime.Now;
                        perfil.vUsuarioCreacion = usuarioDoc;
                        context.Tb_MD_Perfiles.Add(perfil);

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

        public BaseResponse<string> EditarPerfil(PerfilRequest model, string usuarioDoc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Perfiles perfil = context.Tb_MD_Perfiles.Find(model.codigo);
                        if (perfil == null)
                        {
                            throw new Exception("Entidad Nula, Cargo no encontrado");
                        }
                        //Tb_MD_Cargo cargo = new Tb_MD_Cargo();
                  
                        perfil.NombrePerfil = model.nombre;
                        perfil.EstadoRegistro = model.estado;
                        perfil.FechaModificacion = DateTime.Now;
                        perfil.vUsuarioCreacion = usuarioDoc;
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

        public BaseResponse<string> EliminarPerfil(PerfilRequest model, string usuarioDoc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Perfiles perfil = context.Tb_MD_Perfiles.Find(model.codigo);
                        if (perfil == null)
                        {
                            throw new Exception("Entidad Nula, Cargo no encontrado");
                        }
                        perfil.EstadoRegistro = EstadoRegistroTabla.Eliminado;
                        perfil.FechaModificacion = DateTime.Now;
                        perfil.vUsuarioModificacion = usuarioDoc;
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

        public PageResultSP<PerfilResponse> getAllPerfilUsuario(PageResultParam param)
        {
            PageResultSP<PerfilResponse> result = new PageResultSP<PerfilResponse>();
            result.data = new List<PerfilResponse>();

            try
            {
                int page = param.pageIndex + 1;
                int total = 0;


                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                #endregion
                int userId = 0;
                if (param.textFilter != "") {
                    userId = Convert.ToInt16(param.textFilter);
                }
                 

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<PerfilResponse>("exec Proc_Sel_Perfil @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<PerfilResponse>();

                    if (result.data.Count > 0)
                    {
                        total = Convert.ToInt32(result.data[0].total);

                    }
                    if (userId!=0) {
                        result.data.ForEach(x =>
                        {
                            Tb_MD_PerfilUsuario perfil = context.Tb_MD_PerfilUsuario.Where(y => y.IdUsuario == userId && y.IdPerfil == x.codigo && y.iEstadoRegistro == EstadoRegistroTabla.Activo).FirstOrDefault();
                            if (perfil != null)
                            {
                                x.checkActivo = true;
                            }
                        });
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

    }
}
