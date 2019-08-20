using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Helper;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.Model.operaciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess.Admin
{
    public partial class OperadorDataAccess
    {



        public PageResultSP<ClienteRegsitradosOperador> getAllClientesRegistradosForOperado(PageResultParam param)
        {
            PageResultSP<ClienteRegsitradosOperador> result = new PageResultSP<ClienteRegsitradosOperador>();
            try
            {
                result.data = new List<ClienteRegsitradosOperador>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                string estado = param.textFilter;
                if (param.textFilter == null)
                {
                    estado = "";
                }
                var estadoParam = new SqlParameter { ParameterName = "Estado", Value = estado };

             
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<ClienteRegsitradosOperador>("exec Proc_Sel_ClientesRegistradosOperador @PageNumber,@ItemsPerPage,@Estado", pageParam, itemsParam, estadoParam).ToList<ClienteRegsitradosOperador>();

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
                #endregion


                result.success = true;

            }
            catch (Exception ex)
            {
                // copiar
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;

            }

            return result;
        }

        public BaseResponse<PersonaNatutalAllResponse> getDatosPrePersonaNaturalAll(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaNatutalAllResponse> result = new BaseResponse<PersonaNatutalAllResponse>();


            try
            {

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.idPreCliente == model.cid);

                    if (cliente == null)
                        throw new Exception("Ocurrio un error en los datos");


                    result.data = new PersonaNatutalAllResponse();
                    Tb_MD_Pre_Per_Natural persona = null;
                    persona = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento) && x.Id_Pre_Cliente == cliente.idPreCliente);

                    if (persona != null)
                    {
                        result.data.tipoDocumento = persona.vTipoDocumento;
                        result.data.nroDocumento = persona.vNumDocumento;
                        result.data.nombres = persona.vNombre;
                        result.data.apePaterno = persona.vApellido;
                        result.data.apeMaterno = persona.vApellidoMat;

                        if (persona.vFechaNacimiento.HasValue)
                        {
                            result.data.fechaNacimiento = persona.vFechaNacimiento.Value.ToString("dd/MM/yyyy");
                        }

                        result.data.email = persona.vMailContacto;
                        result.data.celular = string.Format("{0}{1}", persona.vPreCelular, persona.vTelefonoMovil);
                        result.data.pais = persona.Tb_MD_Pais.Nombre;
                        result.data.departamento = persona.Tb_MD_Departamento.Nombre;
                        result.data.provincia = persona.Tb_MD_Provincia.Nombre;
                        result.data.distrito = persona.Tb_MD_Distrito.Nombre;
                        result.data.direccion = persona.vDireccion;
                        result.data.origenFondos = persona.Tb_MD_OrigenFondo.Descripcion;
                        result.data.sictuacionLaboral = persona.vFlgSituacionLaboral == "D" ? "Dependiente" : "Independiente";
                        result.data.expuesto = persona.vFlgExpuestoPoliticamente == "S" ? "Sí" : "No";
                        result.data.validacionOperador = cliente.EstadoValidacion;
                        result.data.validacionFideicomiso = cliente.EstadoValidacion_Fideicomiso;

                        if (cliente.EstadoValidacion_Fideicomiso != "P")
                        {
                            result.data.msmFideicomiso = cliente.ComentarioFideicomiso;
                        }
                        if (cliente.EstadoValidacion != "P")
                        {
                            result.data.msmOperador = cliente.ComentarioOperador;
                        }


                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];

                        result.data.cuentasBancarias = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.iIdCliente == model.cid && (x.vEstadoRegistro == EstadoRegistroTabla.Activo || x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar)).Select(x => new CuentaBancariaClienteResponse
                        {
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                            monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                            moneda = x.Tb_MD_TipoMoneda.vDesMoneda,
                            nroCuenta = x.vNroCuenta,
                            tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre,
                            estado=x.vEstadoRegistro,
                            nombreEstado=x.vEstadoRegistro==1?"Activo":"Verificar"
                        }).ToList();


                        var IdclienteParam = new SqlParameter { ParameterName = "Idcliente", Value = persona.Id_Pre_Cliente };
                        result.data.observacionesCliente = context.Database.SqlQuery<ObservacionesClienteResponse>("exec Proc_Sel_observaciones_cliente @Idcliente", IdclienteParam).ToList<ObservacionesClienteResponse>();

                        foreach (var cb in result.data.cuentasBancarias)
                        {
                            cb.logo = cb.logo;
                        }

                    }
                    else
                    {
                        result.data = null;
                    }

                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ex = ex;

                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }



            return result;
        }

        public BaseResponse<PersonaJuridicaAllResponse> getDatosPersonaJuridicaAllXValidacion(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaAllResponse> result = new BaseResponse<PersonaJuridicaAllResponse>();
            result.data = new PersonaJuridicaAllResponse();
            try
            {
               

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.idPreCliente ==  model.cid);
                    if (cliente == null)
                        throw new Exception(ErroresValidacion.ClienteNoExiste);


                    Tb_MD_Pre_Per_Juridica empresa = null;
                    empresa = context.Tb_MD_Pre_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento) && x.idPreCliente == cliente.idPreCliente);

                    if (empresa != null)
                    {
                        result.data.ruc = empresa.vNumDocumento;
                        result.data.nombre = empresa.vRazonSocial;
                        result.data.actividadEconomica = empresa.Tb_MD_ActividadEconomica.Nombre;
                        result.data.origenFondos = empresa.Tb_MD_OrigenFondo.Descripcion;
                        result.data.pais = empresa.Tb_MD_Pais.Nombre;
                        result.data.departamento = empresa.Tb_MD_Departamento.Nombre;
                        result.data.provincia = empresa.Tb_MD_Provincia.Nombre;
                        result.data.distrito = empresa.Tb_MD_Distrito.Nombre;
                        result.data.direccion = empresa.vDireccion;

                        Tb_MD_Documentos documento = null;
                        documento = context.Tb_MD_Documentos.Where(x => x.vNombre.Equals(empresa.vNumDocumento) && x.iEstadoRegistro == EstadoRegistroTabla.Activo).FirstOrDefault();

                        if (documento != null)
                        {
                            result.data.rucArchivo = documento.vNombre;
                        }
                        else
                        {
                            result.data.rucArchivo = "";
                        }

                        // datos de representante legal

                        result.data.accionistas = (from a in context.Tb_MD_Pre_Accionistas
                                          where a.IdPreCliente == cliente.idPreCliente
                                          select new PersonaNatutalAllResponse2
                                          {
                                              tipoDocumento = a.vTipoDocumento,
                                              nroDocumento = a.IdPersonaNatural,
                                              nombres = a.vNombre,
                                              apePaterno = a.vApellido,
                                              apeMaterno = a.vApellidoMat,
                                              celular = a.vPreCelular + a.vTelefonoMovil,
                                              email = a.vMailContacto
                                          }).ToList();


                        // Representante LEgal
                        result.data.repreLegal = new PersonaNatutalAllResponse();
                        var repreLegal = empresa.Tb_MD_Pre_Per_Natural;

                        result.data.repreLegal.tipoDocumento = repreLegal.vTipoDocumento;
                        result.data.repreLegal.nroDocumento = repreLegal.vNumDocumento;
                        result.data.repreLegal.nombres = repreLegal.vNombre;
                        result.data.repreLegal.apePaterno = repreLegal.vApellido;
                        result.data.repreLegal.apeMaterno = repreLegal.vApellidoMat;
                        result.data.repreLegal.celular = string.Format("{0}{1}", repreLegal.vPreCelular, repreLegal.vTelefonoMovil);
                        result.data.repreLegal.email = repreLegal.vMailContacto;

                        result.data.estadoValidacion = cliente.EstadoValidacion;
                        result.data.estadoValidacionFideicomiso = cliente.EstadoValidacion_Fideicomiso;
                        

                        // datos de los accionistas

                        var IdclienteParam = new SqlParameter { ParameterName = "Idcliente", Value = cliente.idPreCliente };
                        result.data.observacionesCliente = context.Database.SqlQuery<ObservacionesClienteResponse>("exec Proc_Sel_observaciones_cliente @Idcliente", IdclienteParam).ToList<ObservacionesClienteResponse>();


                        // Perzona Autorizada 

                        result.data.personaAutorizada = context.Tb_MD_Pre_Empresa_PersonaAutorizada
                            .Where(x => x.IdPreCliente.Equals(cliente.idPreCliente))
                            .Select(x => new PersonaNatutalAllResponse2
                            {
                                nroDocumento = x.IdPersonaAutorizada,
                                nombres = x.vNombre,
                                apePaterno = x.vApellido,
                                celular = x.vPreCelular + x.vTelefonoMovil,
                                email = x.vMailContacto
                            }).ToList();

                        string host = System.Configuration.ConfigurationManager.AppSettings["HostAdmin"];
                        result.data.cuentasBancarias = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.iIdCliente == model.cid && (x.vEstadoRegistro == EstadoRegistroTabla.Activo || x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar)).Select(x => new CuentaBancariaClienteResponse
                        {
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                            monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                            nroCuenta = x.vNroCuenta,
                            tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre,
                            nombreEstado = x.vEstadoRegistro == 1 ? "Activo" : "Verificar"
                        }).ToList();

                    }
                    else
                    {
                        result.data = null;
                    }

                }


                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ex = ex;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public BaseResponse<PersonaJuridicaAllResponse> getDatosPersonaJuridicaAll(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaAllResponse> result = new BaseResponse<PersonaJuridicaAllResponse>();
            result.data = new PersonaJuridicaAllResponse();
            try
            {


                using (MesaDineroContext context = new MesaDineroContext())
                {


                    Tb_MD_Per_Juridica empresa = null;
                    empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));

                    if (empresa != null)
                    {
                        result.data.ruc = empresa.vNumDocumento;
                        result.data.nombre = empresa.vRazonSocial;
                        result.data.actividadEconomica = empresa.Tb_MD_ActividadEconomica.Nombre;
                        result.data.origenFondos = empresa.Tb_MD_OrigenFondo.Descripcion;
                        result.data.pais = empresa.Tb_MD_Pais.Nombre;
                        result.data.departamento = empresa.Tb_MD_Departamento.Nombre;
                        result.data.provincia = empresa.Tb_MD_Provincia.Nombre;
                        result.data.distrito = empresa.Tb_MD_Distrito.Nombre;
                        result.data.direccion = empresa.vDireccion;

                        // datos de representante legal

                        List<Tb_MD_Cuentas_Email> emailsForEmpresa = new List<Tb_MD_Cuentas_Email>();
                        emailsForEmpresa = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica.Equals(empresa.vNumDocumento)).ToList();
                        IList<PersonaNatutalAllResponse2> lstAccionistas = new List<PersonaNatutalAllResponse2>();

                        lstAccionistas = (from a in context.Tb_MD_Accionistas
                                          join pn in context.Tb_MD_Per_Natural on a.IdPersonaNatural equals pn.vNumDocumento
                                          where a.IdEmpresa.Equals(empresa.vNumDocumento)
                                          select new PersonaNatutalAllResponse2
                                          {
                                              tipoDocumento = pn.vTipoDocumento,
                                              nroDocumento = pn.vNumDocumento,
                                              nombres = pn.vNombre,
                                              apePaterno = pn.vApellido,
                                              apeMaterno = pn.vApellidoMat,
                                              celular = pn.vPreCelular + pn.vTelefonoMovil
                                          }).ToList();


                        // Representante LEgal
                        result.data.repreLegal = new PersonaNatutalAllResponse();
                        var repreLegal = empresa.Tb_MD_Per_Natural;

                        result.data.repreLegal.tipoDocumento = repreLegal.vTipoDocumento;
                        result.data.repreLegal.nroDocumento = repreLegal.vNumDocumento;
                        result.data.repreLegal.nombres = repreLegal.vNombre;
                        result.data.repreLegal.apePaterno = repreLegal.vApellido;
                        result.data.repreLegal.apeMaterno = repreLegal.vApellidoMat;
                        result.data.repreLegal.celular = string.Format("{0}{1}", repreLegal.vPreCelular, repreLegal.vTelefonoMovil);
                        result.data.repreLegal.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.RepresentanteLegal) && x.vNumDocumento.Equals(repreLegal.vNumDocumento)).vMailContacto ?? "";

                        // datos de los accionistas
                        result.data.accionistas = new List<PersonaNatutalAllResponse2>();

                        foreach (var acc in lstAccionistas)
                        {
                            acc.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.Accionista) && x.vNumDocumento.Equals(acc.nroDocumento)).vMailContacto ?? "";

                            result.data.accionistas.Add(acc);
                        }

                        // Perzona Autorizada 

                        IList<PersonaNatutalAllResponse2> lstAutorizados = new List<PersonaNatutalAllResponse2>();

                        result.data.personaAutorizada = context.Tb_MD_Empresa_PersonaAutorizada
                            .Where(x => x.IdEmpresa.Equals(empresa.vNumDocumento))
                            .Select(x => new PersonaNatutalAllResponse2
                            {
                                nroDocumento = x.Tb_MD_Per_Natural.vNumDocumento,
                                nombres = x.Tb_MD_Per_Natural.vNombre,
                                apePaterno = x.Tb_MD_Per_Natural.vApellido,
                                celular = x.Tb_MD_Per_Natural.vPreCelular + x.Tb_MD_Per_Natural.vTelefonoMovil
                            }).ToList();

                        foreach (var pa in result.data.personaAutorizada)
                        {
                            pa.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.Autorizado) && x.vNumDocumento.Equals(pa.nroDocumento)).vMailContacto ?? "";
                        }

                        //result.data.personaAutorizada = new PersonaNatutalAllResponse();
                        //var perAuto = empresa.Tb_MD_Per_Natural;

                        //result.data.personaAutorizada.tipoDocumento = perAuto.vTipoDocumento;
                        //result.data.personaAutorizada.nroDocumento = repreLegal.vNumDocumento;
                        //result.data.personaAutorizada.nombres = perAuto.vNombre;
                        //result.data.personaAutorizada.apePaterno = perAuto.vApellido;
                        //result.data.personaAutorizada.apeMaterno = perAuto.vApellidoMat;
                        //result.data.personaAutorizada.celular = string.Format("{0}{1}", perAuto.vPreCelular, perAuto.vTelefonoMovil);
                        //result.data.personaAutorizada.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.Autorizado) && x.vNumDocumento.Equals(perAuto.vNumDocumento)).vMailContacto ?? "";


                        // Cuentas bancarias

                        string host = System.Configuration.ConfigurationManager.AppSettings["HostAdmin"];

                        result.data.cuentasBancarias = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == model.cid && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new CuentaBancariaClienteResponse
                        {
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                            monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                            nroCuenta = x.vNroCuenta,
                            tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre
                        }).ToList();

                    }
                    else
                    {
                        result.data = null;
                    }

                }


                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ex = ex;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public BaseResponse<string> ValidarCuentaClienteForOperador(string estado, int sid, string observacion, int currentUser)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {

                        Tb_MD_Pre_Clientes cliente = null;

                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.idPreCliente == sid);

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);

                        cliente.EstadoValidacion = estado;
                        cliente.UsuarioValidacion_Operador = currentUser;
                        cliente.FechaValidacion_Operador = DateTime.Now;
                        // cliente.SecretId = Guid.NewGuid();
                        cliente.ComentarioOperador = observacion;
                       

                        if (cliente.vTipoCliente == TipoCliente.PersonaNatural)
                            cliente.iEstadoNavegacion = 3;
                        else
                            cliente.iEstadoNavegacion = 4;




                        if (estado.Equals("O"))
                        {

                            if (cliente.vTipoValidacion.Equals("CB"))
                            {

                                Tb_MD_Clientes clienteOrginal = null;
                                clienteOrginal = context.Tb_MD_Clientes.Where(X => X.idPreCliente == cliente.idPreCliente).FirstOrDefault();

                            
                                    Tb_MD_Notificacion limpNot = new Tb_MD_Notificacion();

                                    limpNot = context.Tb_MD_Notificacion.Where(x => x.IdCliente == clienteOrginal.iIdCliente && x.Tipo == 1).FirstOrDefault();
                                    if (limpNot != null)
                                    {
                                        limpNot.iEstadoRegistro = EstadoRegistroTabla.NoActivo;
                                    }

                                    Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                                    notificacion.IdUsuario = "";
                                    notificacion.IdCliente = clienteOrginal.iIdCliente;
                                    notificacion.Titulo = "Cuentas Bancarias Observadas";
                                    notificacion.Mensaje = "Cuentas bancarias han sido observadas, por favor verifique sus cuentas";
                                    notificacion.Tipo = 1;
                                    notificacion.vNumeroSubasta = "";
                                    notificacion.vEstadoSubasta = "";
                                    notificacion.Url = "";
                                    notificacion.Fecha = DateTime.Now.AddDays(1);
                                    notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Notificacion.Add(notificacion);
                                    context.SaveChanges();
                            

                                var clientesBancos = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == clienteOrginal.iIdCliente && x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar).ToList();

                                var preClienteBancos = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.iIdCliente == cliente.idPreCliente && x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar).ToList();

                                clientesBancos.ForEach(x =>
                                {
                                    x.vEstadoRegistro = EstadoRegistroTabla.Observado;
                                });

                                preClienteBancos.ForEach(x =>
                                {
                                    x.vEstadoRegistro = EstadoRegistroTabla.Observado;
                                });


                                CorreoHelper.SendCorreoRegistroCuentaCliente(cliente.vEmail, cliente.NombreCliente, "Observado");
                            
                            }else if(cliente.vTipoValidacion.Equals("C")){
                                CorreoHelper.SendCorreoRegistroModificarCliente(cliente.vEmail, cliente.NombreCliente, "Observado");
                            }
                            else {
                                if (cliente.vTipoRegistro == TipoRegistroPreCliente.NuevoRegistro)
                                    CorreoHelper.SendCorreoRegistroCliente(cliente.vEmail, cliente.SecretId.ToString(), cliente.NombreCliente, "Observado");

                            }

                            
                            cliente.Seguimiento = SeguimientoRegistro.PreProcesoValidacion;
                        }
                        else if (estado.Equals("R"))
                        {
                             if (cliente.vTipoValidacion.Equals("CB"))
                            {
                                CorreoHelper.SendCorreoRegistroCuentaCliente(cliente.vEmail, cliente.NombreCliente, "Rechazado");

                            }
                             else if (cliente.vTipoValidacion.Equals("C"))
                             {
                                 CorreoHelper.SendCorreoRegistroModificarCliente(cliente.vEmail, cliente.NombreCliente, "Rechazado");
                             }
                             else {
                                 if (cliente.vTipoRegistro == TipoRegistroPreCliente.NuevoRegistro)
                                     CorreoHelper.SendCorreoRegistroCliente(cliente.vEmail, cliente.SecretId.ToString(), cliente.NombreCliente, "Rechazado");

                                 cliente.Seguimiento = SeguimientoRegistro.PreProcesoValidacion;
                             }
                          
                        }
                        else
                        {
                            cliente.EstadoValidacion_Fideicomiso = "P";
                            cliente.UsuarioValidacion_Fideicomiso = null;
                            cliente.FechaValidacion_Fideicomiso = null;
                        }

                        Tb_MD_Observacion_Cliente _observacion = new Tb_MD_Observacion_Cliente();
                        _observacion.Estado = estado;
                        _observacion.FechaCreacion = DateTime.Now;
                        _observacion.IdCliente = cliente.idPreCliente;
                        _observacion.Mensaje = observacion;
                        _observacion.RolObservador = "O";
                        _observacion.TipoRegistroObservacion = cliente.vTipoRegistro;
                        _observacion.UsuarioCreacion = currentUser;
                        context.Tb_MD_Observacion_Cliente.Add(_observacion);


                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> ValidarCuentaClienteForFideicomiso(string estado, int sid, string observacion, int currentUser)
        {

            DateTime ahora = DateTime.Now;

            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {

                        Tb_MD_Pre_Clientes cliente = null;

                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.idPreCliente == sid);

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);

                        cliente.EstadoValidacion_Fideicomiso = estado;
                        cliente.UsuarioValidacion_Fideicomiso = currentUser;
                        cliente.FechaValidacion_Fideicomiso = ahora;
                        cliente.ComentarioFideicomiso = observacion;


                        if (estado.Equals("A"))
                        {

                            cliente.dFechaValidacionPaso4 = DateTime.Now;
                            cliente.Seguimiento = SeguimientoRegistro.CrearPassword;

                            if (cliente.vTipoValidacion != null)
                            {
                                if (cliente.vTipoValidacion.Equals("C") || cliente.vTipoValidacion.Equals("CB"))
                                {
                                    cliente.Seguimiento = SeguimientoRegistro.PostCrearPasswords;
                                    CorreoHelper.SendCorreoConfirmacionUsuario(cliente.vEmail, cliente.SecretId.ToString(), cliente.NombreCliente);

                                 

                                }
                                else
                                {
                                  
                                }


                                if (cliente.vTipoValidacion.Equals("C") || cliente.vTipoValidacion.Equals("CB"))
                                {
                                    Tb_MD_Clientes clienteOrginal = null;
                                    clienteOrginal = context.Tb_MD_Clientes.Where(X => X.idPreCliente == cliente.idPreCliente).FirstOrDefault();

                                       if (cliente.vTipoValidacion.Equals("CB")) {

                                        List<Tb_MD_Notificacion> limpNot = new List<Tb_MD_Notificacion>();

                                        limpNot = context.Tb_MD_Notificacion.Where(x => x.IdCliente == clienteOrginal.iIdCliente && x.Tipo == 1).ToList();
                                            if (limpNot .Count()>0)
                                            {
                                                limpNot.ForEach(x =>
                                                {
                                                    x.iEstadoRegistro = EstadoRegistroTabla.NoActivo;
                                                });
                                                
                                            }

                                            Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                                            notificacion.IdUsuario = "";
                                            notificacion.IdCliente = clienteOrginal.iIdCliente;
                                            notificacion.Titulo = "Verificacion de Cuentas Satisfactoria";
                                            notificacion.Mensaje = "Su solicitud ha sido atendida, sus cuentas bancarias estan activas";
                                            notificacion.Tipo = 1;
                                            notificacion.vNumeroSubasta = "";
                                            notificacion.vEstadoSubasta = "";
                                            notificacion.Url = "";
                                            notificacion.Fecha = DateTime.Now.AddDays(1);
                                            notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                            context.Tb_MD_Notificacion.Add(notificacion);
                                            context.SaveChanges();
                                        }
                                    //Tb_MD_ClienteUsuario clienteUsuario = null;
                                    //clienteUsuario=
                                    clienteOrginal.Tb_MD_ClienteUsuario.Where(y => y.EstadoREgistro == EstadoRegistroTabla.PorVerificar).ToList().ForEach(x =>
                                    {
                                        Tb_MD_Empresa_PersonaAutorizada autorizada = null;
                                        autorizada = context.Tb_MD_Empresa_PersonaAutorizada.Where(z => z.SecredId == x.SecredId).FirstOrDefault();
                                        string nombreCompleto = autorizada.Tb_MD_Per_Natural.vNombre + " " + autorizada.Tb_MD_Per_Natural.vApellido + autorizada.Tb_MD_Per_Natural.vApellidoMat;
                                        x.EstadoREgistro = EstadoRegistroTabla.Activo;
                                        CorreoHelper.SendCorreoRegistroUsuario(x.Email, x.SecredId.ToString(), nombreCompleto);

                                    });

                                    //  Tb_MD_Clientes clienteOrginal = null;
                                    //clienteOrginal = context.Tb_MD_Clientes.Where(X => X.idPreCliente == cliente.idPreCliente).FirstOrDefault();

                                    //Tb_MD_ClientesDatosBancos clientesBancos = null;
                                    var clientesBancos = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == clienteOrginal.iIdCliente && x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar).ToList();

                                    var preClienteBancos = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.iIdCliente == cliente.idPreCliente && x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar).ToList();

                                    clientesBancos.ForEach(x =>
                                    {
                                        x.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    });

                                    preClienteBancos.ForEach(x =>
                                    {
                                        x.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    });
                                }
                            
                            }
                            if (cliente.vTipoRegistro == TipoRegistroPreCliente.NuevoRegistro)
                            {
                                CorreoHelper.SendCorreoRegistroExitozo(cliente.vEmail, cliente.SecretId.ToString(), cliente.NombreCliente);
                             
                            }
                                
                      

                            
                        }

                        Tb_MD_Observacion_Cliente _observacion = new Tb_MD_Observacion_Cliente();
                        _observacion.Estado = estado;
                        _observacion.FechaCreacion = ahora;
                        _observacion.IdCliente = cliente.idPreCliente;
                        _observacion.Mensaje = observacion;
                        _observacion.RolObservador = "F";
                        _observacion.TipoRegistroObservacion = cliente.vTipoRegistro;
                        _observacion.UsuarioCreacion = currentUser;
                        context.Tb_MD_Observacion_Cliente.Add(_observacion);



                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        //OPERACIONES CONFIRMADAS DE LA TESORERIA

        public PageResultSP<EstatusOperacionesConfirmadas> traerOperacionesConfirmadasRegistradosT(PageResultParam param)
        {
            PageResultSP<EstatusOperacionesConfirmadas> valorRegistrados = new PageResultSP<EstatusOperacionesConfirmadas>();

            try
            {
                valorRegistrados.data = new List<EstatusOperacionesConfirmadas>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<EstatusOperacionesConfirmadas>("exec Proc_Sel_OperacionesConfirmadas @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<EstatusOperacionesConfirmadas>();

                    foreach (var cliente in valorRegistrados.data)
                    {
                        //cliente.fechaShort = cliente.fecha.ToString("dd/MM/yyyy");
                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];
                        cliente.logoOrigen = host + cliente.logoOrigen;
                        cliente.logoDestino = host + cliente.logoDestino;

                        if (cliente.fecha != null)
                        {
                            cliente.fechaShort = cliente.fecha.Value.ToString("dd/MM/yyyy");

                        }

                        cliente.horaFin = "";

                        if (cliente.estadoSubastaCodigo.Trim() == EstadoSubasta.PendientePago)
                        {
                            if (cliente.fechaFinPago != null)
                            {
                                DateTime fechaFin = DateTime.Parse(cliente.fechaFinPago.ToString()); //obtenemos este valor de una bbdd
                                var segundos = Math.Round((fechaFin - DateTime.Now).TotalSeconds, 0);
                                if (segundos > 0)
                                {
                                    cliente.horaFin = segundos.ToString();
                                }
                                else
                                {
                                    cliente.horaFin = "0";
                                    using (var transaccion = context.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            var subasta = context.Tb_MD_Subasta.Find(cliente.idTransaccion);

                                            subasta.vEstadoSubasta = EstadoSubasta.OperacionIncumplida;

                                            context.SaveChanges();
                                            transaccion.Commit();

                                            //result.success = true;

                                        }
                                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                        {
                                            #region Error EntityFramework
                                            var errorMessages = ex.EntityValidationErrors
                                                    .SelectMany(x => x.ValidationErrors)
                                                    .Select(x => x.ErrorMessage);

                                            var fullErrorMessage = string.Join("; ", errorMessages);

                                            //result.success = false;
                                            //result.error = fullErrorMessage;
                                            transaccion.Rollback();
                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            //result.success = false;

                                            transaccion.Rollback();
                                            //result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                        }

                                    }






                                }
                            }
                        }



                    }




                    if (valorRegistrados.data.Count > 0)
                    {
                        total = Convert.ToInt32(valorRegistrados.data[0].total);
                    }
                }

                #region Copiar Al Cual
                var pag = Utilities.ResultadoPagination(page, param.itemPerPage, total);

                valorRegistrados.itemperpage = pag.itemperpage;
                valorRegistrados.limit = pag.limit;
                valorRegistrados.numbersPages = pag.numbersPages;
                valorRegistrados.offset = pag.offset;
                valorRegistrados.page = pag.page;
                valorRegistrados.PageCount = pag.pageCount;
                valorRegistrados.total = pag.total;
                #endregion


                valorRegistrados.success = true;

            }
            catch (Exception ex)
            {
                // copiar
                valorRegistrados.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                valorRegistrados.success = false;

            }

            return valorRegistrados;
        }
        public PageResultSP<ClienteRegsitradosResponse> getListadoClientesRegistrados(PageResultParam param)
        {
            PageResultSP<ClienteRegsitradosResponse> result = new PageResultSP<ClienteRegsitradosResponse>();
            try
            {
                result.data = new List<ClienteRegsitradosResponse>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                string nroDocumento = param.textFilter;
                if (param.textFilter == null)
                {
                    nroDocumento = "";
                }
                var nroDocumentoParam = new SqlParameter { ParameterName = "nroDocumento", Value = nroDocumento };


                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<ClienteRegsitradosResponse>("exec Proc_Sel_ClientesRegistrados @PageNumber,@ItemsPerPage,@nroDocumento", pageParam, itemsParam, nroDocumentoParam).ToList<ClienteRegsitradosResponse>();

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
                #endregion


                result.success = true;

            }
            catch (Exception ex)
            {
                // copiar
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;

            }

            return result;
        }
        public BaseResponse<ClientePersonaNatutalAllResponse> getDatosClienteRegistradosAll(GetDatosPersonaNatural model)
        {
            BaseResponse<ClientePersonaNatutalAllResponse> result = new BaseResponse<ClientePersonaNatutalAllResponse>();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {

                    result.data = new ClientePersonaNatutalAllResponse();
                    Tb_MD_Per_Natural persona = null;
                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));

                    if (persona != null)
                    {
                        result.data.tipoDocumento = persona.vTipoDocumento;
                        result.data.nroDocumento = persona.vNumDocumento;
                        result.data.nombres = persona.vNombre;
                        result.data.apePaterno = persona.vApellido;
                        result.data.apeMaterno = persona.vApellidoMat;

                        if (persona.vFechaNacimiento.HasValue)
                        {
                            result.data.fechaNacimiento = persona.vFechaNacimiento.Value.ToString("dd/MM/yyyy");
                        }

                        result.data.email = persona.vMailContacto;
                        result.data.celular = string.Format("{0}{1}", persona.vPreCelular, persona.vTelefonoMovil);
                        result.data.pais = persona.Tb_MD_Pais.Nombre;
                        result.data.departamento = persona.Tb_MD_Departamento.Nombre;
                        result.data.provincia = persona.Tb_MD_Provincia.Nombre;
                        result.data.distrito = persona.Tb_MD_Distrito.Nombre;
                        result.data.direccion = persona.vDireccion;
                        result.data.origenFondos = persona.Tb_MD_OrigenFondo.Descripcion;
                        result.data.sictuacionLaboral = persona.vFlgSituacionLaboral == "D" ? "Dependiente" : "Independiente";
                        result.data.expuesto = persona.vFlgExpuestoPoliticamente == "S" ? "Sí" : "No";

                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];

                        result.data.cuentasBancarias = context.Tb_MD_ClientesDatosBancos.Where(x => x.vNroDocumento == model.nroDocumento && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new CuentaBancariaClienteResponse
                        {
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                            monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                            moneda = x.Tb_MD_TipoMoneda.vDesMoneda,
                            nroCuenta = x.vNroCuenta,
                            tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre
                        }).ToList();


                        var pageParam = new SqlParameter { ParameterName = "vNumDocumento", Value = model.nroDocumento };
                        List<ClienteOperacionesResponse> operacionCliente = null;


                        operacionCliente = context.Database.SqlQuery<ClienteOperacionesResponse>("exec Proc_Sel_OperacionesClientes @vNumDocumento", pageParam).ToList<ClienteOperacionesResponse>();
                        result.data.clienteOperaciones = operacionCliente;
                    }
                    else
                    {
                        result.data = null;
                    }

                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.ex = ex;

                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }



            return result;
        }
        public BaseResponse<PersonaJuridicaAllResponse> getDatosClienteJuridicaAll(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaAllResponse> result = new BaseResponse<PersonaJuridicaAllResponse>();
            result.data = new PersonaJuridicaAllResponse();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Per_Juridica empresa = null;
                    empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));

                    if (empresa != null)
                    {
                        result.data.ruc = empresa.vNumDocumento;
                        result.data.nombre = empresa.vRazonSocial;
                        result.data.actividadEconomica = empresa.Tb_MD_ActividadEconomica.Nombre;
                        result.data.origenFondos = empresa.Tb_MD_OrigenFondo.Descripcion;
                        if (empresa.Tb_MD_Pais != null)
                        {
                            result.data.pais = empresa.Tb_MD_Pais.Nombre;
                        }
                        if (empresa.Tb_MD_Departamento != null)
                        {
                            result.data.departamento = empresa.Tb_MD_Departamento.Nombre;
                        }
                        if (empresa.Tb_MD_Provincia != null)
                        {
                            result.data.provincia = empresa.Tb_MD_Provincia.Nombre;
                        }

                        if (empresa.Tb_MD_Distrito != null)
                        {
                            result.data.distrito = empresa.Tb_MD_Distrito.Nombre;
                        }
                        if (empresa.vDireccion != null)
                        {
                            result.data.direccion = empresa.vDireccion;
                        }

                        // datos de representante legal

                        List<Tb_MD_Cuentas_Email> emailsForEmpresa = new List<Tb_MD_Cuentas_Email>();
                        emailsForEmpresa = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica.Equals(empresa.vNumDocumento)).ToList();
                        IList<PersonaNatutalAllResponse2> lstAccionistas = new List<PersonaNatutalAllResponse2>();

                        lstAccionistas = (from a in context.Tb_MD_Accionistas
                                          join pn in context.Tb_MD_Per_Natural on a.IdPersonaNatural equals pn.vNumDocumento
                                          where a.IdEmpresa.Equals(empresa.vNumDocumento)
                                          select new PersonaNatutalAllResponse2
                                          {
                                              tipoDocumento = pn.vTipoDocumento,
                                              nroDocumento = pn.vNumDocumento,
                                              nombres = pn.vNombre,
                                              apePaterno = pn.vApellido,
                                              apeMaterno = pn.vApellidoMat,
                                              celular = pn.vPreCelular + pn.vTelefonoMovil
                                          }).ToList();


                        // Representante LEgal
                        result.data.repreLegal = new PersonaNatutalAllResponse();
                        var repreLegal = empresa.Tb_MD_Per_Natural;

                        result.data.repreLegal.tipoDocumento = repreLegal.vTipoDocumento;
                        result.data.repreLegal.nroDocumento = repreLegal.vNumDocumento;
                        result.data.repreLegal.nombres = repreLegal.vNombre;
                        result.data.repreLegal.apePaterno = repreLegal.vApellido;
                        result.data.repreLegal.apeMaterno = repreLegal.vApellidoMat;
                        result.data.repreLegal.celular = string.Format("{0}{1}", repreLegal.vPreCelular, repreLegal.vTelefonoMovil);
                        var emailRepresentante = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.RepresentanteLegal) && x.vNumDocumento.Equals(repreLegal.vNumDocumento));
                        if (emailRepresentante != null)
                        {
                            result.data.repreLegal.email = emailRepresentante.vMailContacto;
                        }
                        else {
                            result.data.repreLegal.email = "";
                        }
                        

                        //.vMailContacto ?? "";
                        // datos de los accionistas
                        result.data.accionistas = new List<PersonaNatutalAllResponse2>();

                        foreach (var acc in lstAccionistas)
                        {
                            acc.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.Accionista) && x.vNumDocumento.Equals(acc.nroDocumento)).vMailContacto ?? "";

                            result.data.accionistas.Add(acc);
                        }

                        // Perzona Autorizada 

                        IList<PersonaNatutalAllResponse2> lstAutorizados = new List<PersonaNatutalAllResponse2>();

                        result.data.personaAutorizada = context.Tb_MD_Empresa_PersonaAutorizada
                            .Where(x => x.IdEmpresa.Equals(empresa.vNumDocumento))
                            .Select(x => new PersonaNatutalAllResponse2
                            {
                                nroDocumento = x.Tb_MD_Per_Natural.vNumDocumento,
                                nombres = x.Tb_MD_Per_Natural.vNombre,
                                apePaterno = x.Tb_MD_Per_Natural.vApellido,
                                celular = x.Tb_MD_Per_Natural.vPreCelular + x.Tb_MD_Per_Natural.vTelefonoMovil
                            }).ToList();

                        foreach (var pa in result.data.personaAutorizada)
                        {
                            pa.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.Autorizado) && x.vNumDocumento.Equals(pa.nroDocumento)).vMailContacto ?? "";
                        }



                        // Cuentas bancarias

                        string host = System.Configuration.ConfigurationManager.AppSettings["HostAdmin"];

                        if (model.cid == Convert.ToInt16("1"))
                        {// Si es Partner context.Tb_MD_TipoCuentaBancaria.Find(x.vTipoCuenta).Nombre.ToString()
                            result.data.cuentasBancarias = context.Tb_MD_Cuentas_Bancarias.Where(x => x.vNumDocumento == model.nroDocumento && x.iEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new CuentaBancariaClienteResponse
                            {
                                
                                logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                                monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                                nroCuenta = x.vNumCuenta,
                                tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre
                            }).ToList();
                        }
                        else {
                            result.data.cuentasBancarias = context.Tb_MD_ClientesDatosBancos.Where(x => x.vNroDocumento == model.nroDocumento && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new CuentaBancariaClienteResponse
                            {
                                logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                                monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                                nroCuenta = x.vNroCuenta,
                                tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre
                            }).ToList();
                        
                        }

                       


                        var pageParam = new SqlParameter { ParameterName = "vNumDocumento", Value = model.nroDocumento };
                        List<ClienteOperacionesResponse> operacionCliente = null;


                        operacionCliente = context.Database.SqlQuery<ClienteOperacionesResponse>("exec Proc_Sel_OperacionesClientes @vNumDocumento", pageParam).ToList<ClienteOperacionesResponse>();
                        result.data.clienteOperaciones = operacionCliente;

                    }

                    else
                    {
                        result.data = null;
                    }

                }


                result.success = true;
            } 
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
               #region Error EntityFramework
                var errorMessages = ex.EntityValidationErrors
                                                    .SelectMany(x => x.ValidationErrors)
                                                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                //result.success = false;
                //result.error = fullErrorMessage;
                                        
                                            #endregion
             }
            catch (Exception ex)
            {
                result.success = false;
                result.ex = ex;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public PageResultSP<PartnerListadoResponse> getListadoPartnerAll(PageResultParam param)
        {
            PageResultSP<PartnerListadoResponse> result = new PageResultSP<PartnerListadoResponse>();
            try
            {
                result.data = new List<PartnerListadoResponse>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<PartnerListadoResponse>("exec Proc_Sel_Lista_Partner @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<PartnerListadoResponse>();

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
                #endregion


                result.success = true;

            }
            catch (Exception ex)
            {
                // copiar
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;

            }

            return result;
        }


        public PageResultSP<PartnerLiquidacionResponse> getListadoLiquidacionPartner(PageResultParam param, string numPartner)
        {
            PageResultSP<PartnerLiquidacionResponse> result = new PageResultSP<PartnerLiquidacionResponse>();
            try
            {
                result.data = new List<PartnerLiquidacionResponse>();
                string idpartner = "";
                string estado = "";
                if (numPartner == "")
                {
                    if (param.textFilter != null)
                    {
                        idpartner = param.textFilter;
                    }

                }
                else
                {
                    idpartner = numPartner;
                }

                if (param.idFilter == 9)
                {
                    estado = "";
                }
                else if (param.idFilter == 10)
                {
                    estado = "G";
                }
                else
                {
                    estado = param.idFilter.ToString();
                }
                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                var partnerParam = new SqlParameter { ParameterName = "numPartner", Value = idpartner };
                var numliqParam = new SqlParameter { ParameterName = "numLiq", Value = param.searchFilter };
                var estadoParam = new SqlParameter { ParameterName = "estado", Value = estado };


                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<PartnerLiquidacionResponse>("exec Proc_Sel_LiquidacionPartner @PageNumber,@ItemsPerPage,@numPartner,@numLiq,@estado", pageParam, itemsParam, partnerParam, numliqParam, estadoParam).ToList<PartnerLiquidacionResponse>();

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
                #endregion
                result.success = true;

            }
            catch (Exception ex)
            {
                // copiar
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;

            }

            return result;
        }


        public BaseResponse<string> GenerarLiquidacion(List<ListaCodigosLiquidacionRequest> listaSubasta,int codigousuario)
        {
            DateTime ahora = DateTime.Now;
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (listaSubasta.Count() <= 0) { 
                             throw new Exception("Debe seleccionar al menos una operacion");
                        }
                        //listaSubasta = new List<ListaCodigosLiquidacionRequest>();
                        Tb_MD_Constantes constante = null;
                        int nuevoCodigo=0;
                        string anio = DateTime.Now.Date.Year.ToString();
                        anio = anio.Substring(2,2);

                        string codigoLiquidacion = ConfigurationManager.AppSettings["CODLIQUIDACION"].ToString();

                        constante = context.Tb_MD_Constantes.Where(x => x.IdConstante == codigoLiquidacion).FirstOrDefault();
                        nuevoCodigo =Convert.ToInt16( constante.Valor)+1;
                        constante.Valor = nuevoCodigo.ToString();


                        string formatoCodigo = "LIQ-19-" + String.Format("{0:000000}", nuevoCodigo);

    
                        listaSubasta.ForEach(m=>{
                            Tb_MD_Subasta_Pago subastaPago = null;
                             subastaPago = context.Tb_MD_Subasta_Pago.Where(x => x.nNumeroSubasta == m.nroSubasta).FirstOrDefault();
                             if (subastaPago != null) {
                                 subastaPago.vNumeroLiquidacion = formatoCodigo;
                                 subastaPago.dFechaRegLiq = DateTime.Now;
                                 subastaPago.iUsuarioLiqLmd = codigousuario;
                                 subastaPago.vEstadoLiq = "1";
                             }
                        });

                        
                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }


        public BaseResponse<string> AprobarLiquidacionPartner(List<ListaCodigosLiquidacionRequest> listaSubasta, int codigousuario)
        {
            DateTime ahora = DateTime.Now;
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (listaSubasta.Count() <= 0)
                        {
                            throw new Exception("Debe seleccionar al menos una operacion");
                        }
                      
                        listaSubasta.ForEach(m =>
                        {
                            Tb_MD_Subasta_Pago subastaPago = null;
                            subastaPago = context.Tb_MD_Subasta_Pago.Where(x => x.nNumeroSubasta == m.nroSubasta).FirstOrDefault();
                            if (subastaPago != null)
                            {
                                //subastaPago.vNumeroLiquidacion = formatoCodigo;

                                subastaPago.vValidaPart = m.checkear;
                                subastaPago.iUsuarioPart = codigousuario;
                                subastaPago.dFechaValPart = DateTime.Now;
                                subastaPago.vEstadoLiq = m.estado.ToString();
                              
                            }
                        });


                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }


        public BaseResponse<string> RechazaLiquidacionPartner(List<ListaCodigosLiquidacionRequest> listaSubasta, int codigousuario)
        {
            DateTime ahora = DateTime.Now;
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (listaSubasta.Count() <= 0)
                        {
                            throw new Exception("Debe seleccionar al menos una operacion");
                        }
                      
                        listaSubasta.ForEach(m =>
                        {
                            Tb_MD_Subasta_Pago subastaPago = null;
                            subastaPago = context.Tb_MD_Subasta_Pago.Where(x => x.nNumeroSubasta == m.nroSubasta).FirstOrDefault();
                            if (subastaPago != null)
                            {
                                //subastaPago.vNumeroLiquidacion = formatoCodigo;

                                subastaPago.vValidaPart = null;
                                subastaPago.iUsuarioPart = null;
                                subastaPago.dFechaValPart = null;
                                subastaPago.dFechaRegLiq = null;
                                subastaPago.vEstadoLiq =null;
                                subastaPago.vNumeroLiquidacion = null;
                                subastaPago.iUsuarioLiqLmd = null;
                              
                            }
                        });


                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        
        public BaseResponse<string> AprobarLiquidacionFideicomiso(List<ListaCodigosLiquidacionRequest> listaSubasta, int codigousuario)
        {
            DateTime ahora = DateTime.Now;
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (listaSubasta.Count() <= 0)
                        {
                            throw new Exception("Debe seleccionar al menos una operacion");
                        }

                        listaSubasta.ForEach(m =>
                        {
                            Tb_MD_Subasta_Pago subastaPago = null;
                            subastaPago = context.Tb_MD_Subasta_Pago.Where(x => x.nNumeroSubasta == m.nroSubasta).FirstOrDefault();
                            if (subastaPago != null)
                            {
                                //subastaPago.vNumeroLiquidacion = formatoCodigo;

                                //subastaPago.vValidaPart = m.checkear;
                                //subastaPago.iUsuarioPart = codigousuario;
                                //subastaPago.dFechaValPart = DateTime.Now;
                                //subastaPago.vEstadoLiq = m.estado.ToString();

                                subastaPago.iUsuarioFid = codigousuario;
                                subastaPago.dFechaPagFid = DateTime.Now;
                                subastaPago.vEstadoLiq = m.estado.ToString();

                            }
                        });


                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }



        public BaseResponse<string> PagarLiquidacionPartner(List<ListaCodigosLiquidacionRequest> listaSubasta, int codigousuario)
        {
            DateTime ahora = DateTime.Now;
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (listaSubasta.Count() <= 0)
                        {
                            throw new Exception("Debe seleccionar al menos una operacion");
                        }

                        listaSubasta.ForEach(m =>
                        {
                            Tb_MD_Subasta_Pago subastaPago = null;
                            subastaPago = context.Tb_MD_Subasta_Pago.Where(x => x.nNumeroSubasta == m.nroSubasta).FirstOrDefault();
                            if (subastaPago != null)
                            {
                                //subastaPago.vNumeroLiquidacion = formatoCodigo;
                                subastaPago.vNroVoucherFid = m.numVoucher;
                                subastaPago.iUsuarioPartnerTes = codigousuario;
                                subastaPago.dFechaPagPartnerTes = DateTime.Now;
                                subastaPago.vEstadoLiq = m.estado.ToString();

                            }
                        });


                        result.success = true;

                        context.SaveChanges();
                        transaccion.Commit();

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
                        result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }



    }
}
