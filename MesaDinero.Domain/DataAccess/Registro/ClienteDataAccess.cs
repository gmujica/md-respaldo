using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Helper;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess
{
    public partial class ClienteDataAccess
    {
        public BaseResponse<PersonaNatutalRequest> getDatosBasicosCurrentUser(int clienteId)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            result.data = new PersonaNatutalRequest();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Clientes cliente = null;
                    cliente = context.Tb_MD_Clientes.FirstOrDefault(x => x.iIdCliente == clienteId);
                    if (cliente == null)
                        throw new Exception("El usuario no esta asociado a ningun cliente");

                    if(!cliente.codigoModificacionDatos .HasValue)
                    {
                        Tb_MD_Per_Natural persona = null;
                        persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(cliente.vNroDocumento));

                        if (persona != null)
                        {

                            #region Datos Verificados
                            result.data.nroDocumento = persona.vNumDocumento;
                            result.data.tipoDocumento = persona.vTipoDocumento;
                            result.data.nombres = persona.vNombre;
                            result.data.apePaterno = persona.vApellido;
                            result.data.apeMaterno = persona.vApellidoMat;

                            if (persona.vFechaNacimiento.HasValue)
                            {
                                result.data.fnDia = persona.vFechaNacimiento.Value.Day;
                                result.data.fnMes = persona.vFechaNacimiento.Value.Month;
                                result.data.fnAnio = persona.vFechaNacimiento.Value.Year;
                            }

                            result.data.email = persona.vMailContacto;
                            result.data.preCelular = persona.vPreCelular;
                            result.data.celular = persona.vTelefonoMovil;

                            result.data.pais = persona.vIdPaisOrigen;
                            result.data.departamento = persona.iCodDepartamento ?? 0;
                            result.data.provincia = persona.iCodProvincia ?? 0;
                            result.data.distrito = persona.iCodDistrito ?? 0;

                            result.data.direccion = persona.vDireccion;
                            result.data.origenFondos = persona.iOrigenFondos ?? 0;
                            result.data.sictuacionLaboral = !string.IsNullOrEmpty(persona.vFlgSituacionLaboral) ? persona.vFlgSituacionLaboral : "I";
                            result.data.expuesto = !string.IsNullOrEmpty(persona.vFlgExpuestoPoliticamente) ? persona.vFlgExpuestoPoliticamente : "N";

                            if (persona.vFlgExpuestoPoliticamente == "S")
                            {
                                Tb_MD_Expuesto_Politicamente expuesto = null;
                                expuesto = context.Tb_MD_Expuesto_Politicamente.FirstOrDefault(x => x.vNumDocumento.Equals(persona.vNumDocumento) && x.dFechaFinActividad == null);

                                if (expuesto != null)
                                {
                                    result.data.entidadNombreExpuesto = expuesto.vNombreEntidad;
                                    result.data.cargoExpuesto = expuesto.vCargo;
                                }

                            }
                            #endregion

                        }
                        else
                        { result.data = null; }
                    }
                    else
                    {
                        Tb_MD_Pre_Per_Natural persona = null;
                        persona = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.Id_Pre_Cliente == cliente.codigoModificacionDatos);

                        if(persona != null)
                        {
                            result.data.nroDocumento = persona.vNumDocumento;
                            result.data.tipoDocumento = persona.vTipoDocumento;
                            result.data.nombres = persona.vNombre;
                            result.data.apePaterno = persona.vApellido;
                            result.data.apeMaterno = persona.vApellidoMat;

                            if (persona.vFechaNacimiento.HasValue)
                            {
                                result.data.fnDia = persona.vFechaNacimiento.Value.Day;
                                result.data.fnMes = persona.vFechaNacimiento.Value.Month;
                                result.data.fnAnio = persona.vFechaNacimiento.Value.Year;
                            }

                            result.data.email = persona.vMailContacto;
                            result.data.preCelular = persona.vPreCelular;
                            result.data.celular = persona.vTelefonoMovil;

                            result.data.pais = persona.vIdPaisOrigen;
                            result.data.departamento = persona.iCodDepartamento ?? 0;
                            result.data.provincia = persona.iCodProvincia ?? 0;
                            result.data.distrito = persona.iCodDistrito ?? 0;

                            result.data.direccion = persona.vDireccion;
                            result.data.origenFondos = persona.iOrigenFondos ?? 0;
                            result.data.sictuacionLaboral = !string.IsNullOrEmpty(persona.vFlgSituacionLaboral) ? persona.vFlgSituacionLaboral : "I";
                            result.data.expuesto = !string.IsNullOrEmpty(persona.vFlgExpuestoPoliticamente) ? persona.vFlgExpuestoPoliticamente : "N";



                        }
                        else
                        { result.data = null; }
                    }
                   

                    
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                // result.ex = ex;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result; 
        }

        public BaseResponse<string> updateDatosBasicosCurrentUser(PersonaNatutalRequest model, int clienteId)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context= new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        DateTime ahora = DateTime.Now;

                        Tb_MD_Clientes cliente = null;
                        cliente = context.Tb_MD_Clientes.FirstOrDefault(x => x.iIdCliente == clienteId);

                        Tb_MD_Pre_Clientes preCliente = null;

                        if (cliente.codigoModificacionDatos == null)
                            preCliente = new Tb_MD_Pre_Clientes();
                        else
                            preCliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.idPreCliente == cliente.codigoModificacionDatos);

                        preCliente.vNombre = cliente.vNombre;
                        preCliente.vApellido = cliente.vApellido;
                        preCliente.iCodDepartamento = model.departamento;
                        preCliente.iCodDistrito = model.distrito;
                        preCliente.NombreCliente = cliente.NombreCliente;
                        preCliente.vTipoCliente = (int)cliente.vTipoCliente;
                        preCliente.vTipoDocumento = cliente.vTipoDocumento;
                        preCliente.iEstadoNavegacion = -1;
                        preCliente.envioValidacion = true;
                        preCliente.EstadoValidacion = "P";
                        preCliente.EstadoValidacion_Fideicomiso = "P";
                        preCliente.Seguimiento = SeguimientoRegistro.RegistroProcesoValidacion;

                        preCliente.dFechaEnvioValidacion = ahora;

                        if(cliente.codigoModificacionDatos == null)
                        {
                            preCliente.vNroDocumento = cliente.vNroDocumento;
                            preCliente.SecretId = Guid.NewGuid();
                            preCliente.vEstadoRegistro = -1;
                            preCliente.envioMSM = true;
                            preCliente.dFechaCreacion = ahora;
                            preCliente.vTipoRegistro = TipoRegistroPreCliente.ModificacionDatosBasicos;

                            context.Tb_MD_Pre_Clientes.Add(preCliente);
                            context.SaveChanges();
                        }

                        Tb_MD_Per_Natural persona = null;
                        persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(cliente.vNroDocumento));

                        Tb_MD_Pre_Per_Natural prePersona = null;
                        prePersona = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.Id_Pre_Cliente == preCliente.idPreCliente);

                        bool nuevaPrePersona = false;

                        if (prePersona == null)
                        {
                            prePersona = new Tb_MD_Pre_Per_Natural();
                            nuevaPrePersona = true;
                            prePersona.dFechaCreacion = ahora;
                            prePersona.Id_Pre_Cliente = preCliente.idPreCliente;
                            prePersona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                        }

                        prePersona.vNumDocumento = persona.vNumDocumento;
                        prePersona.vTipoDocumento = persona.vTipoDocumento;
                        prePersona.vNombre = persona.vNombre;
                        prePersona.vApellido = persona.vApellido;
                        prePersona.vApellidoMat = persona.vApellidoMat;
                        prePersona.vMailContacto = persona.vMailContacto;
                        prePersona.vPreCelular = persona.vPreCelular;
                        prePersona.vTelefonoMovil = persona.vTelefonoMovil;

                        prePersona.vFechaNacimiento = new DateTime(model.fnAnio, model.fnMes, model.fnDia);

                        #region Direccion
                        prePersona.vIdPaisOrigen = model.pais;
                        prePersona.iCodDepartamento = model.departamento;
                        prePersona.iCodProvincia = model.provincia;
                        prePersona.iCodDistrito = model.distrito;
                        prePersona.vDireccion = model.direccion;
                        #endregion

                        prePersona.iOrigenFondos = model.origenFondos;
                        prePersona.vFlgSituacionLaboral = model.sictuacionLaboral;
                        prePersona.vFlgExpuestoPoliticamente = model.expuesto;
                        if(model.expuesto == "N")
                        {
                            prePersona.CargoExpuesto = "";
                            prePersona.NombreEntidadExpuesto = "";
                        }
                        else
                        {
                            prePersona.CargoExpuesto = model.cargoExpuesto;
                            prePersona.NombreEntidadExpuesto = model.entidadNombreExpuesto;
                        }


                        if(nuevaPrePersona)
                        {
                            context.Tb_MD_Pre_Per_Natural.Add(prePersona);
                        }

                        cliente.codigoModificacionDatos = preCliente.idPreCliente;

                        context.SaveChanges();
                        transaction.Commit();

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
                        transaction.Rollback();
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        //result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

           

            return result;
        }

        public BaseResponse<List<CuentaBancariaClienteResponse>> getDatosBancariosCurrentClient(int clienteId)
        {
            BaseResponse<List<CuentaBancariaClienteResponse>> result = new BaseResponse<List<CuentaBancariaClienteResponse>>();
            result.data = new List<CuentaBancariaClienteResponse>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];
                        result.data = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == clienteId && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new CuentaBancariaClienteResponse
                        {
                            codigo = x.iIdDatosBank,
                            banco = x.vBanco,
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad,
                            moneda = x.vMoneda,
                            monedatext = x.Tb_MD_TipoMoneda.vDesMoneda,
                            nroCuenta = x.vNroCuenta,
                            tipoCuenta = x.iTipoCuenta,
                            tipoCuentaText = x.Tb_MD_TipoCuentaBancaria.Nombre,
                            nroCCI = x.vCCI,
                            estado = x.vEstadoRegistro

                        }).ToList();

                        result.success = true;
                    }
                    catch (Exception ex)
                    {
                       
                        result.success = false;
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        result.other = ex.StackTrace;
                    }
                }
            }


            return result;
        }

        public BaseResponse<string> updateCuentasBancarias(List<CuentaBancariaClienteResponse> model, int clienteId)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.idPreCliente == clienteId);
                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);

                        List<CuentaBancariaClienteResponse> inserts = new List<CuentaBancariaClienteResponse>();
                        inserts = model.Where(x => x.codigo == 0 && x.estado == 1).ToList();

                        foreach (var i in inserts)
                        {
                            Tb_MD_ClientesDatosBancos cuenta = new Tb_MD_ClientesDatosBancos();
                            cuenta.vBanco = i.banco;
                            cuenta.vMoneda = i.moneda;
                            cuenta.vNroCuenta = i.nroCuenta;
                            cuenta.vCCI = i.nroCCI;
                            cuenta.iTipoCuenta = i.tipoCuenta;
                            cuenta.vEstadoRegistro = EstadoRegistroTabla.Activo;
                            cuenta.dFechaCreacion = DateTime.Now;
                            cuenta.vSecredId = Guid.NewGuid();
                            cuenta.iIdCliente = clienteId;
                            cuenta.vTipoPersona = cliente.vTipoCliente;
                            cuenta.vNroDocumento = cliente.vNroDocumento;

                            context.Tb_MD_ClientesDatosBancos.Add(cuenta);
                        }

                        List<Tb_MD_ClientesDatosBancos> cuentas = new List<Tb_MD_ClientesDatosBancos>();
                        cuentas = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == clienteId && x.vEstadoRegistro == EstadoRegistroTabla.Activo).ToList();

                        foreach (var c in cuentas)
                        {
                            CuentaBancariaClienteResponse u = null; u = model.Where(x => x.codigo == c.iIdDatosBank).FirstOrDefault();
                            if(u != null)
                            c.vEstadoRegistro = u.estado;
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
                        //result.ex = ex;
                        transaccion.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> sendCorreoRecuperarPassword(string email){

            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid SecredId = Guid.NewGuid();
                        DateTime ahora = DateTime.Now;
                        Tb_MD_ClienteUsuario usuario = null;
                        usuario = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.EstadoREgistro == EstadoRegistroTabla.Activo && x.Email.Equals(email));

                        if (usuario == null)
                            throw new Exception("No se encontro un cliente de LMD con este Email");

                        Tb_MD_RecuperarPassword entity = new Tb_MD_RecuperarPassword
                        {
                            Email = usuario.Email,
                            FechaCreacion = ahora,
                            FechaExpiracion = ahora.AddDays(1),
                            IdUsuario = usuario.IdUsuario,
                            SecredId = SecredId,
                            TipoUsuario = "CL"
                        };

                        context.Tb_MD_RecuperarPassword.Add(entity);
                        context.SaveChanges();
                        transaction.Commit();

                        result.success = true;
                        CorreoHelper.SedCorreoRecuperarContrasenha(usuario.Email, SecredId.ToString(), usuario.NombreCliente);

                    }
                    catch(Exception ex)
                    {
                        result.success = false;
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> CambioPasswordXCorreoExterno(string secredId, string password)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            
             using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(secredId);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_RecuperarPassword recuperar = null;
                        recuperar = context.Tb_MD_RecuperarPassword.FirstOrDefault(x => x.SecredId == sid);

                        if (recuperar == null)
                            throw new Exception("La operacion que intenta realizar no cuenta con autorización para hacerla.");

                        Tb_MD_ClienteUsuario usuario = null;
                        usuario = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.IdUsuario == recuperar.IdUsuario);

                        if (usuario == null)
                            throw new Exception("No se encontro el usuario.");

                        string clave = Encrypt.EncryptKey(password);
                        usuario.Password = clave;

                        context.SaveChanges();
                        transaction.Commit();


                        result.success = true;
                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        transaction.Rollback();
                    }
                }
             }

            return result;
        }

        public BaseResponse<string> sendCorreoRecuperarPasswordAdmin(string email)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid SecredId = Guid.NewGuid();
                        DateTime ahora = DateTime.Now;
                        Tb_MD_Mae_Usuarios usuario = null;
                        usuario = context.Tb_MD_Mae_Usuarios.FirstOrDefault(x => x.vEstadoRegistro == EstadoRegistroTabla.Activo && x.vEmailUsuario.Equals(email));

                        Tb_MD_Per_Natural persona = null;
                        persona = context.Tb_MD_Per_Natural.FirstOrDefault(y => y.vNumDocumento == usuario.vNroDocumento);


                        if (usuario == null)
                            throw new Exception("No se encontro un cliente de LMD con este Email");

                        Tb_MD_RecuperarPassword entity = new Tb_MD_RecuperarPassword
                        {
                            Email = usuario.vEmailUsuario,
                            FechaCreacion = ahora,
                            FechaExpiracion = ahora.AddDays(1),
                            IdUsuario = usuario.iIdUsuario,
                            SecredId = SecredId,
                            TipoUsuario = "CL"
                        };

                        context.Tb_MD_RecuperarPassword.Add(entity);
                        context.SaveChanges();
                        transaction.Commit();

                        result.success = true;
                        CorreoHelper.SedCorreoRecuperarContrasenhaAdmin(usuario.vEmailUsuario, SecredId.ToString(), persona.vNombre+" "+persona.vApellido+" "+persona.vApellidoMat);

                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }


        public BaseResponse<string> CambioPasswordXCorreoAdmin(string secredId, string password)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(secredId);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_RecuperarPassword recuperar = null;
                        recuperar = context.Tb_MD_RecuperarPassword.FirstOrDefault(x => x.SecredId == sid);

                        if (recuperar == null)
                            throw new Exception("La operacion que intenta realizar no cuenta con autorización para hacerla.");

                        Tb_MD_Mae_Usuarios usuario = null;
                        usuario = context.Tb_MD_Mae_Usuarios.FirstOrDefault(x => x.iIdUsuario == recuperar.IdUsuario);

                        if (usuario == null)
                            throw new Exception("No se encontro el usuario.");

                        string clave = Encrypt.EncryptKey(password);
                        usuario.vPassword = clave;

                        context.SaveChanges();
                        transaction.Commit();


                        result.success = true;
                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }



    }
}
