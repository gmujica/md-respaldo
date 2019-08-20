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
    public class UsuarioDataAccess
    {
        public PageResultSP<UsuarioResponse> getAllUsuario(PageResultParam param)
        {
            PageResultSP<UsuarioResponse> result = new PageResultSP<UsuarioResponse>();
            result.data = new List<UsuarioResponse>();

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
                    result.data = context.Database.SqlQuery<UsuarioResponse>("exec Proc_Sel_usuarios @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<UsuarioResponse>();

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

        public async Task<BaseResponse<string>> insertUsuario(UsuarioRequest model, string usuarioDoc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Mae_Usuarios usuariovalida = context.Tb_MD_Mae_Usuarios.Where(x => x.vEmailUsuario == model.email && x.vRucEmpresa == model.rucEmpresa).FirstOrDefault();
                        if (usuariovalida != null)
                        {
                            throw new Exception("Ya existe un usuario con el mismo correo en esa empresa");
                        }
                        Tb_MD_Per_Natural persona = null;
                        persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));
                        {
                            if (persona == null)
                            {
                                persona = new Tb_MD_Per_Natural();
                                persona.dFechaCreacion = DateTime.Now;
                                persona.vTipoDocumento = model.tipoDocumento;
                                persona.vNumDocumento = model.nroDocumento;
                                persona.vNombre = model.nombre;
                                persona.vApellido = model.apellidoPat;
                                persona.vApellidoMat = model.apellidoMat;
                                persona.vPreCelular = model.prefijo;
                                persona.vTelefonoMovil = model.celular;
                                persona.vMailContacto = model.email;
                                persona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                persona.vFlgExpuestoPoliticamente = "N";
                                context.Tb_MD_Per_Natural.Add(persona);
                            }
                        }

                        Tb_MD_Mae_Usuarios usuario = new Tb_MD_Mae_Usuarios();
                        usuario.vEmailUsuario = model.email;
                        usuario.vTipoUsuario = model.tipoUsuario;
                        usuario.dFechaCreacion = DateTime.Now;
                        usuario.vUsuarioCreacion = usuarioDoc;
                        usuario.SecretId = Guid.NewGuid();
                        usuario.vRucEmpresa = model.rucEmpresa;
                        usuario.vNroDocumento = model.nroDocumento;
                        usuario.vEstadoRegistro = EstadoRegistroTabla.NoActivo;
                        context.Tb_MD_Mae_Usuarios.Add(usuario);

                        string cliente = model.nombre + " " + model.apellidoPat + " " + model.apellidoMat;

                        bool resul = await CorreoHelper.SendCorreoRegistroAdmExitoso(usuario.vEmailUsuario, usuario.SecretId.ToString(), cliente);

                        if (resul == false)
                        {
                            throw new Exception("No se pudo enviar Correo, Revise su conexion");
                        }

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

        public BaseResponse<string> EditarUsuario(UsuarioRequest model, string usuarioDoc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {

                        Tb_MD_Per_Natural persona = null;
                        persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));
                        {
                            if (persona == null)
                            {
                                persona = new Tb_MD_Per_Natural();
                                persona.dFechaCreacion = DateTime.Now;
                                persona.vTipoDocumento = model.tipoDocumento;
                                persona.vNumDocumento = model.nroDocumento;
                                persona.vNombre = model.nombre;
                                persona.vApellido = model.apellidoPat;
                                persona.vApellidoMat = model.apellidoMat;
                                persona.vPreCelular = model.prefijo;
                                persona.vTelefonoMovil = model.celular;
                                persona.vMailContacto = model.email;
                                persona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                persona.vFlgExpuestoPoliticamente = "N";
                                context.Tb_MD_Per_Natural.Add(persona);
                            }
                        }

                        Tb_MD_Mae_Usuarios usuario = context.Tb_MD_Mae_Usuarios.Find(model.codigo);
                        if (usuario == null)
                        {
                            throw new Exception("Entidad Nula, Usuario no encontrado");
                        }
                        //Tb_MD_Cargo cargo = new Tb_MD_Cargo();

                        usuario.vEmailUsuario = model.email;
                        usuario.vTipoUsuario = model.tipoUsuario;
                        usuario.dFechaModificacion = DateTime.Now;
                        usuario.vUsuarioModificacion = usuarioDoc;
                        usuario.vRucEmpresa = model.rucEmpresa;
                        usuario.vNroDocumento = model.nroDocumento;
                        usuario.vEstadoRegistro = EstadoRegistroTabla.NoActivo;
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

        public BaseResponse<string> EliminarUsuario(UsuarioRequest model, string usuarioDoc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        Tb_MD_Mae_Usuarios usuario = context.Tb_MD_Mae_Usuarios.Find(model.codigo);
                        if (usuario == null)
                        {
                            throw new Exception("Entidad Nula, Cargo no encontrado");
                        }


                        usuario.dFechaModificacion = DateTime.Now;
                        usuario.vUsuarioModificacion = usuarioDoc;
                        usuario.vEstadoRegistro = model.estado;
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

        public BaseResponse<string> insertPerfilUsuario(List<UsuarioPerfilRequest> model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {

                    try
                    {
                        //Tb_MD_Mae_Usuarios usuariovalida = context.Tb_MD_Mae_Usuarios.Where(x => x.vEmailUsuario == model.email && x.vRucEmpresa == model.rucEmpresa).FirstOrDefault();
                        //if (usuariovalida != null)
                        //{
                        //    throw new Exception("Ya existe un usuario con el mismo correo en esa empresa");
                        //}
                        if (model.Count == 0)
                        {
                            throw new Exception("Debe Seleccionar un Perfil");
                        }

                        List<Tb_MD_PerfilUsuario> userPerfil = null;
                        int codigoUsuario = Convert.ToInt16(model.FirstOrDefault().codigoUsuario);
                        userPerfil = context.Tb_MD_PerfilUsuario.Where(x => x.IdUsuario == codigoUsuario).ToList();

                        if (userPerfil.Count > 0)
                        {
                            userPerfil.ForEach(x =>
                            {
                                x.iEstadoRegistro = EstadoRegistroTabla.Eliminado;
                            });
                        }

                        model.ForEach(x =>
                        {
                            Tb_MD_PerfilUsuario uservalida;
                            uservalida = context.Tb_MD_PerfilUsuario.Where(y => y.IdUsuario == x.codigoUsuario && y.IdPerfil == x.codigoPerfil).FirstOrDefault();

                            if (uservalida != null)
                            {
                                uservalida.iEstadoRegistro = EstadoRegistroTabla.Activo;
                            }

                            if (uservalida == null)
                            {
                                Tb_MD_PerfilUsuario usuarioPerfil = new Tb_MD_PerfilUsuario();
                                usuarioPerfil.IdUsuario = x.codigoUsuario;
                                usuarioPerfil.IdPerfil = x.codigoPerfil;
                                usuarioPerfil.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                context.Tb_MD_PerfilUsuario.Add(usuarioPerfil);
                            }


                        });


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
