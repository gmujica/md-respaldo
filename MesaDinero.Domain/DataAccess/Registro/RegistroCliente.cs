using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Helper;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.Model.operaciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;

namespace MesaDinero.Domain.DataAccess
{
    public partial class RegistroCliente
    {
        public BaseResponse<string> reanudarRegistro(ReanudarRegistroRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            try
            {
                model.email = model.email.ToLower();
                Tb_MD_Pre_Clientes pre_cliente = null;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    pre_cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.iEstadoNavegacion > 0 && !x.Finalizado && x.vEmail.Equals(model.email) && x.vNroDocumento.Equals(model.nroDocumento));

                    if (pre_cliente != null)
                        result.data = pre_cliente.SecretId.ToString();
                    else
                        result.data = "";

                }
                result.success = true;
            }
            catch (Exception ex)
            {
                result.other = ex.StackTrace;
                result.success = false;
                //result.ex = ex;               
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public BaseResponse<string> crearCuenta(RegistroClientesRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();



            Tb_MD_Pre_Clientes preCliente = null;
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        model.email = model.email.ToLower();

                        #region Validacion
                        preCliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.vNroDocumento.Equals(model.nroDocumento) && x.Finalizado);
                        if (preCliente != null)
                        {
                            result.other = "CLR";
                            result.data = model.nroDocumento;
                            throw new Exception(string.Format("{0} ya eres parte de La Mesa de Dinero </br> inicia sesión para comerzar a subastar", model.nroDocumento));
                        }

                        preCliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.vNroDocumento.Equals(model.nroDocumento) && !x.Finalizado && x.vEstadoRegistro > 0);
                        if (preCliente != null)
                        {
                            result.other = "CLRP";
                            result.data = model.nroDocumento;
                            throw new Exception(string.Format("{0} ya tienes un registro en proceso </br> dirigete a la sección inicio de sesion para retomar tu registro.", model.nroDocumento));
                        }

                        Tb_MD_Per_Juridica personaJuridica = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));
                        if (personaJuridica != null)
                        {
                            result.other = "CLR";
                            result.data = model.nroDocumento;
                            throw new Exception(string.Format("{0} ya eres parte de La Mesa de Dinero </br> inicia sesión para comerzar a subastar", model.nroDocumento));
                        }

                        preCliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.vEmail.Equals(model.email) && (x.Finalizado || x.vEstadoRegistro > 0));
                        if (preCliente != null)
                        {
                            result.other = "CLCE";
                            result.other = model.email;
                            throw new Exception(string.Format(" el correo {0} ya se encuentra ocuapdo, por favor ingrese un correo distinto.", model.email));
                        }




                        #endregion


                        result.data = "";

                        preCliente = new Tb_MD_Pre_Clientes();
                        preCliente.vNroDocumento = model.nroDocumento;
                        preCliente.vNombre = model.nombre;
                        preCliente.vApellido = model.apellido;
                        preCliente.vEmail = model.email;
                        preCliente.vCelular = model.phone;
                        preCliente.dFechaCreacion = DateTime.Now;
                        preCliente.vTipoCliente = model.tipoPersona;
                        preCliente.SecretId = Guid.NewGuid();
                        preCliente.iEstadoNavegacion = 0;
                        preCliente.nroDocumentoContacto = model.nroDocumentoContacto;
                        preCliente.nombreEmpresa = model.nombreEmpresa;
                        preCliente.vTipoRegistro = TipoRegistroPreCliente.NuevoRegistro;
                        preCliente.vEstadoRegistro = (int)EstadoRegistroCliente.preRegistro;
                        preCliente.Seguimiento = SeguimientoRegistro.IngresoClaveSMS;
                        preCliente.envioMSM = false;
                        preCliente.envioValidacion = false;

                        //   string claveSMS_ = context.Database.SqlQuery<string>(string.Format("SELECT dbo.fn_GenerarClaveSMS({0},'N')", System.Configuration.ConfigurationManager.AppSettings["longSerialSMS"])).FirstOrDefault();

                        string claveSMS_ = "123456";

                        preCliente.vClaveSMS = claveSMS_;
                        result.data = preCliente.SecretId.ToString();
                        context.Tb_MD_Pre_Clientes.Add(preCliente);


                        result.success = true;

                        context.SaveChanges();
                        transaction.Commit();
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

        public BaseResponse<string> generarSMSCliente(Guid secredId)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                try
                {
                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId == secredId);

                    if (cliente == null)
                        throw new Exception(ErroresValidacion.ClienteNoExiste);


                    // string claveSMS = context.Database.SqlQuery<string>(string.Format("SELECT dbo.fn_GenerarClaveSMS({0},'N')", System.Configuration.ConfigurationManager.AppSettings["longSerialSMS"])).FirstOrDefault();

                    string claveSMS = "123456";

                    SendMsMPhone.VerificacionMsM_RegistroBasico(cliente.vCelular, claveSMS);

                    cliente.vClaveSMS = claveSMS;
                    cliente.envioMSM = true;
                    result.success = true;
                    context.SaveChanges();

                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.ex = ex;
                    result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }


            }


            return result;
        }

        public BaseResponse<string> envioMsMCliente(Guid secredId)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                try
                {
                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId == secredId);

                    if (cliente == null)
                        throw new Exception(ErroresValidacion.ClienteNoExiste);

                    if (cliente.envioMSM)
                        throw new Exception("Mensaje ya usado.");
                    else
                        cliente.envioMSM = true;



                    result.data = cliente.vCelular;
                    result.success = true;
                    context.SaveChanges();
                    SendMsMPhone.VerificacionMsM_RegistroBasico(cliente.vCelular, cliente.vClaveSMS);

                }
                catch (Exception ex)
                {
                    result.success = false;
                    result.ex = ex;
                    result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }


            }

            return result;
        }

        public BaseResponse<string> validarSMS(ValiarSMSRequest model)
        {
            DateTime ahora = DateTime.Now;

            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid msid = Guid.Parse(model.sid);
                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId.Equals(msid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);

                        if (cliente.vClaveSMS.Equals(model.clavemsm))
                        {
                            cliente.vEstadoRegistro = (int)EstadoRegistroCliente.ingresoDeSmS;
                            cliente.dFechaValidacionPaso1 = ahora;
                            cliente.SecretId = Guid.NewGuid();
                            cliente.iEstadoNavegacion = 1;
                            cliente.Seguimiento = SeguimientoRegistro.RegistroBatosPrincipales;

                            context.SaveChanges();
                            transaction.Commit();

                            result.data = cliente.SecretId.ToString();
                            result.success = true;
                        }
                        else
                        {
                            throw new Exception("El código ingresado es incorrecto.");
                        }



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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> registroPeronaNatural(PersonaNatutalRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Validacion
                        Tb_MD_Pre_Per_Natural persona = null;

                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);

                        persona = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.Id_Pre_Cliente == cliente.idPreCliente);

                        #endregion
                        bool existe = true;
                        if (persona == null)
                        {
                            existe = false;
                            persona = new Tb_MD_Pre_Per_Natural();
                            persona.vNumDocumento = model.nroDocumento;
                            persona.vMailContacto = model.email;
                            persona.vTipoDocumento = model.tipoDocumento;

                            persona.vTelefonoMovil = model.celular;
                            persona.vPreCelular = model.preCelular;
                            persona.dFechaCreacion = DateTime.Now;
                            persona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                            persona.Id_Pre_Cliente = cliente.idPreCliente;

                            cliente.iEstadoNavegacion = 2;


                        }

                        persona.vNombre = model.nombres;
                        persona.vApellido = model.apePaterno;
                        persona.vApellidoMat = model.apeMaterno;

                        if (!persona.vFechaNacimiento.HasValue)
                            persona.vFechaNacimiento = new DateTime(model.fnAnio, model.fnMes, model.fnDia);

                        if (string.IsNullOrEmpty(persona.vIdPaisOrigen))
                            persona.vIdPaisOrigen = model.pais;

                        persona.iCodDepartamento = model.departamento;

                        persona.iCodProvincia = model.provincia;

                        persona.iCodDistrito = model.distrito;



                        persona.vDireccion = model.direccion;


                        persona.iOrigenFondos = model.origenFondos;



                        persona.vFlgExpuestoPoliticamente = model.expuesto;
                        persona.vFlgSituacionLaboral = model.sictuacionLaboral;


                        if (model.expuesto.Equals("N"))
                        {
                            persona.NombreEntidadExpuesto = string.Empty;
                            persona.CargoExpuesto = string.Empty;
                        }
                        else
                        {
                            persona.NombreEntidadExpuesto = model.entidadNombreExpuesto;
                            persona.CargoExpuesto = model.cargoExpuesto;
                        }


                        if (existe == false)
                            context.Tb_MD_Pre_Per_Natural.Add(persona);

                        cliente.vNombre = string.Format("{0}", model.nombres);
                        cliente.vApellido = model.apePaterno;
                        cliente.vApellido = string.Format("{0} {1}", model.apePaterno, model.apeMaterno);
                        cliente.NombreCliente = string.Format("{0} {1} {2}", model.nombres, model.apePaterno, model.apeMaterno);
                        cliente.vTipoDocumento = model.tipoDocumento;
                        cliente.iCodDepartamento = model.departamento;
                        cliente.iCodDistrito = model.distrito;
                        cliente.SecretId = Guid.NewGuid();
                        cliente.vEstadoRegistro = (int)EstadoRegistroCliente.datosBasicos;
                        cliente.dFechaValidacionPaso2 = DateTime.Now;
                        cliente.Seguimiento = SeguimientoRegistro.RegistroDatosBancarios;
                        cliente.vNroDocumento = model.nroDocumento;

                        context.SaveChanges();
                        transaction.Commit();

                        result.data = cliente.SecretId.ToString();
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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<List<CuentaBancariaClienteResponse>> getDatosBancariosXRegistro(GetDatosPersonaNatural model)
        {
            BaseResponse<List<CuentaBancariaClienteResponse>> result = new BaseResponse<List<CuentaBancariaClienteResponse>>();
            result.data = new List<CuentaBancariaClienteResponse>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);



                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];
                        result.data = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.iIdCliente == cliente.idPreCliente && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new CuentaBancariaClienteResponse
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

        public BaseResponse<string> registroDatosBancarios(DatoBancariaCliente_Request model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Validacion

                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);


                        List<DatoBancariaRequest> nuevasCuentas = model.cuentas.Where(x => x.codigo == 0).ToList();

                        if (nuevasCuentas.Count > 0)
                        {
                            IList<Tb_MD_Pre_ClientesDatosBancos> TCuentas;
                            TCuentas = new List<Tb_MD_Pre_ClientesDatosBancos>();
                            foreach (var cb in nuevasCuentas)
                            {
                                Tb_MD_Pre_ClientesDatosBancos cuenta = new Tb_MD_Pre_ClientesDatosBancos();
                                cuenta.vBanco = cb.banco;
                                cuenta.vMoneda = cb.moneda;
                                cuenta.iTipoCuenta = cb.tipoCuenta;
                                cuenta.vNroCuenta = cb.nroCuenta;
                                cuenta.vCCI = cb.nroCCI;
                                cuenta.vSecredId = Guid.NewGuid();
                                cuenta.iIdCliente = cliente.idPreCliente;
                                cuenta.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                cuenta.vTipoPersona = cliente.vTipoCliente;
                                cuenta.vNroDocumento = cliente.vNroDocumento;
                                cuenta.dFechaCreacion = DateTime.Now;

                                TCuentas.Add(cuenta);
                            }
                            context.Tb_MD_Pre_ClientesDatosBancos.AddRange(TCuentas);
                        }

                        List<long> idsCuentas = model.cuentas.Where(x => x.codigo > 0).Select(x => x.codigo).ToList();
                        List<Tb_MD_Pre_ClientesDatosBancos> cuentasActivas = new List<Tb_MD_Pre_ClientesDatosBancos>();
                        cuentasActivas = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.vEstadoRegistro == EstadoRegistroTabla.Activo && x.iIdCliente == cliente.idPreCliente && !idsCuentas.Contains(x.iIdDatosBank)).ToList();

                        if (cuentasActivas.Count > 0)
                        {
                            foreach (var cb in cuentasActivas)
                            {
                                cb.vEstadoRegistro = EstadoRegistroTabla.Eliminado;
                            }
                        }


                        cliente.Seguimiento = SeguimientoRegistro.PreProcesoValidacion;
                        cliente.dFechaValidacionPaso3 = DateTime.Now;
                        cliente.vEstadoRegistro = (int)EstadoRegistroCliente.datosBancarios;
                        // cliente.SecretId = Guid.NewGuid();

                        if (cliente.vTipoCliente == TipoCliente.PersonaNatural)
                        {
                            if (cliente.iEstadoNavegacion == 2)
                            {
                                cliente.iEstadoNavegacion = 3;
                            }
                        }
                        else
                        {
                            if (cliente.iEstadoNavegacion == 3)
                            {
                                cliente.iEstadoNavegacion = 4;
                            }
                        }




                        context.SaveChanges();
                        transaction.Commit();
                        result.success = true;
                        result.data = cliente.SecretId.ToString();

                        #endregion
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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }

        public async Task<BaseResponse<PersonaNatutalRequest>> getDatosPersonaNaturalRegistroInit(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            try
            {
                result.data = new PersonaNatutalRequest();
                Tb_MD_Per_Natural persona = null;
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));

                    if (persona != null)
                    {
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

                        Tb_MD_Expuesto_Politicamente expuesto = null;
                        if (persona.vFlgExpuestoPoliticamente.Equals("S"))
                        {
                            expuesto = context.Tb_MD_Expuesto_Politicamente.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento) && x.dFechaFinActividad == null);

                            if (expuesto != null)
                            {
                                result.data.entidadNombreExpuesto = expuesto.vNombreEntidad;
                                result.data.cargoExpuesto = expuesto.vCargo;
                            }

                        }

                    }
                    else
                    {
                        result.data = await QuertiumServices.GetDatosPersonaNatural(model.nroDocumento);
                        result.data.expuesto = "N";
                        result.data.sictuacionLaboral = "I";
                        result.data.origenFondos = 0;


                    }


                }



                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public BaseResponse<PersonaNatutalRequest> getDatosPersonaNaturalXRegistro(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();

            try
            {
                result.data = new PersonaNatutalRequest();
                Tb_MD_Pre_Per_Natural persona = null;
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Guid sid = Guid.NewGuid();
                    try
                    {
                        sid = Guid.Parse(model.sid);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                    }

                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId.Equals(sid));

                    if (cliente == null)
                        throw new Exception(ErroresValidacion.ClienteNoExiste);

                    persona = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.Id_Pre_Cliente == cliente.idPreCliente);

                    if (persona != null)
                    {
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

                        result.data.entidadNombreExpuesto = persona.NombreEntidadExpuesto;
                        result.data.cargoExpuesto = persona.CargoExpuesto;

                    }
                    else
                    {
                        throw new Exception("No se enontraron los datos de la persona.");
                    }

                }

                result.success = true;

            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public BaseResponse<string> enviarAprobacionRegistroCliente(GetDatosPersonaNatural model)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            try
            {
                Guid sid = Guid.NewGuid();
                try
                {
                    sid = Guid.Parse(model.sid);
                }
                catch (Exception)
                {
                    throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                }
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));
                    if (cliente == null)
                        throw new Exception(ErroresValidacion.ClienteNoExiste);



                    cliente.EstadoValidacion = "P";
                    cliente.EstadoValidacion_Fideicomiso = "P";
                    cliente.UsuarioValidacion_Operador = null;
                    cliente.UsuarioValidacion_Fideicomiso = null;
                    cliente.FechaValidacion_Operador = null;
                    cliente.FechaValidacion_Fideicomiso = null;
                    cliente.dFechaEnvioValidacion = DateTime.Now;
                    cliente.vTipoValidacion = "N"; /*Tipo de validacion Nuevo*/

                    if (cliente.envioValidacion == false)
                    {
                        cliente.envioValidacion = true;
                    }


                    if (cliente.vTipoCliente == TipoCliente.PersonaNatural)
                        cliente.iEstadoNavegacion = 4;
                    else
                        cliente.iEstadoNavegacion = 5;


                    cliente.Seguimiento = SeguimientoRegistro.RegistroProcesoValidacion;

                    context.SaveChanges();
                    result.data = cliente.SecretId.ToString();
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }

        public BaseResponse<PersonaNatutalRequest> getDatosPersonaNatural(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();


            try
            {

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = new PersonaNatutalRequest();
                    Tb_MD_Per_Natural persona = null;
                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));

                    if (persona != null)
                    {
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
                    {
                        result.data = null;
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

        public BaseResponse<PersonaJuridicaReuest> getDatosPersonaJuridica(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaReuest> result = new BaseResponse<PersonaJuridicaReuest>();
            result.data = new PersonaJuridicaReuest();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Per_Juridica empresa = null;
                    empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento));

                    if (empresa != null)
                    {
                        result.data.nombre = empresa.vRazonSocial;
                        result.data.actividadEconomica = empresa.ActividadEconomica;
                        result.data.origenFondos = empresa.OrigenFondos;
                        result.data.pais = empresa.vIdPaisOrigen;
                        result.data.departamento = empresa.iCodDepartamento;
                        result.data.provincia = empresa.iCodProvincia;
                        result.data.distrito = empresa.iCodDistrito;
                        result.data.direccion = empresa.vDireccion;

                        // datos de representante legal

                        List<Tb_MD_Cuentas_Email> emailsForEmpresa = new List<Tb_MD_Cuentas_Email>();
                        emailsForEmpresa = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica.Equals(empresa.vNumDocumento)).ToList();
                        List<PersonaNatutalRequest> lstAccionistas = new List<PersonaNatutalRequest>();

                        lstAccionistas = (from a in context.Tb_MD_Accionistas
                                          join pn in context.Tb_MD_Per_Natural on a.IdPersonaNatural equals pn.vNumDocumento
                                          where a.IdEmpresa.Equals(empresa.vNumDocumento)
                                          select new PersonaNatutalRequest
                                          {
                                              tipoDocumento = pn.vTipoDocumento,
                                              nroDocumento = pn.vNumDocumento,
                                              nombres = pn.vNombre,
                                              apePaterno = pn.vApellido,
                                              apeMaterno = pn.vApellidoMat,
                                              preCelular = pn.vPreCelular,
                                              celular = pn.vTelefonoMovil
                                          }).ToList();


                        result.data.repreLegal = new PersonaNatutalRequest();
                        var repreLegal = empresa.Tb_MD_Per_Natural;

                        result.data.repreLegal.tipoDocumento = repreLegal.vTipoDocumento;
                        result.data.repreLegal.nroDocumento = repreLegal.vNumDocumento;
                        result.data.repreLegal.nombres = repreLegal.vNombre;
                        result.data.repreLegal.apePaterno = repreLegal.vApellido;
                        result.data.repreLegal.apeMaterno = repreLegal.vApellidoMat;
                        result.data.repreLegal.preCelular = repreLegal.vPreCelular;
                        result.data.repreLegal.celular = repreLegal.vTelefonoMovil;
                        result.data.repreLegal.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.RepresentanteLegal) && x.vNumDocumento.Equals(repreLegal.vNumDocumento)).vMailContacto;

                        // datos de los accionistas
                        result.data.accionistas = new List<PersonaNatutalRequest>();

                        foreach (var acc in lstAccionistas)
                        {
                            acc.email = emailsForEmpresa.FirstOrDefault(x => x.vRol.Equals(RolPersonaEmpresa.Accionista) && x.vNumDocumento.Equals(acc.nroDocumento)).vMailContacto ?? "";

                            result.data.accionistas.Add(acc);
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
        // nuevo 
        public BaseResponse<PersonaJuridicaReuest> getDatosPersonaJuridicaForRegistro(GetDatosPersonaNatural model)
        {
            BaseResponse<PersonaJuridicaReuest> result = new BaseResponse<PersonaJuridicaReuest>();
            result.data = new PersonaJuridicaReuest();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    #region Cliente Validacion
                    Guid sid = Guid.NewGuid();
                    try
                    {
                        sid = Guid.Parse(model.sid);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                    }

                    Tb_MD_Pre_Clientes cliente = null;
                    cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                    if (cliente == null)
                        throw new Exception(ErroresValidacion.ClienteNoExiste);
                    #endregion

                    Tb_MD_Pre_Per_Juridica empresa = null;
                    empresa = context.Tb_MD_Pre_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.nroDocumento) && x.idPreCliente == cliente.idPreCliente);


                    if (empresa != null)
                    {
                        result.data.nombre = empresa.vRazonSocial;
                        result.data.actividadEconomica = empresa.ActividadEconomica;
                        result.data.origenFondos = empresa.OrigenFondos;
                        result.data.pais = empresa.vIdPaisOrigen;
                        result.data.departamento = empresa.iCodDepartamento;
                        result.data.provincia = empresa.iCodProvincia;
                        result.data.distrito = empresa.iCodDistrito;
                        result.data.direccion = empresa.vDireccion;
                        result.data.estadoValidacion = cliente.EstadoValidacion;

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
                                                   select new PersonaNatutalRequest
                                                   {
                                                       tipoDocumento = a.vTipoDocumento,
                                                       nroDocumento = a.IdPersonaNatural,
                                                       nombres = a.vNombre,
                                                       apePaterno = a.vApellido,
                                                       apeMaterno = a.vApellidoMat,
                                                       preCelular = a.vPreCelular,
                                                       celular = a.vTelefonoMovil,
                                                       email = a.vMailContacto
                                                   }).ToList();


                        result.data.repreLegal = new PersonaNatutalRequest();
                        var repreLegal = empresa.Tb_MD_Pre_Per_Natural;

                        result.data.repreLegal.tipoDocumento = repreLegal.vTipoDocumento;
                        result.data.repreLegal.nroDocumento = repreLegal.vNumDocumento;
                        result.data.repreLegal.nombres = repreLegal.vNombre;
                        result.data.repreLegal.apePaterno = repreLegal.vApellido;
                        result.data.repreLegal.apeMaterno = repreLegal.vApellidoMat;
                        result.data.repreLegal.preCelular = repreLegal.vPreCelular;
                        result.data.repreLegal.celular = repreLegal.vTelefonoMovil;
                        result.data.repreLegal.email = repreLegal.vMailContacto;

                        // datos de los accionistas

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

        public BaseResponse<string> registrarPersonaJuridica(PersonaJuridicaRegistroRequest model, HttpPostedFile file)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        if (file != null)
                        {
                            //string logo = "";
                            //string rutaCorta = ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                            //ConfigurationManager.AppSettings["RUTA_LOGO_BANCO"];
                            //string rutaRaiz = System.Web.HttpContext.Current.Server.MapPath("~" + rutaCorta);

                            string nombreImagen = model.empresa.ruc;
                            string extension = Path.GetExtension(file.FileName);
                            string ruc = model.empresa.ruc;
                            Byte[] FileBytes;

                            Stream fs = file.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            FileBytes = br.ReadBytes((Int32)fs.Length);

                            List<Tb_MD_Documentos> documentoValida = context.Tb_MD_Documentos.Where(x => x.vNombre == ruc && x.iEstadoRegistro == EstadoRegistroTabla.Activo).ToList();

                            if (documentoValida.Count > 0)
                            {
                                documentoValida.ForEach(x =>
                                {
                                    x.iEstadoRegistro = EstadoRegistroTabla.NoActivo;
                                });
                            }

                            Tb_MD_Documentos documento = new Tb_MD_Documentos();
                            documento.vNombre = model.empresa.ruc;
                            documento.vExtension = extension;
                            documento.vArchivo = FileBytes;
                            documento.vTipo = "FR";
                            documento.iEstadoRegistro = EstadoRegistroTabla.Activo;

                            context.Tb_MD_Documentos.Add(documento);

                        }

                        #region Cliente Validacion
                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);
                        #endregion


                        bool existe = true;
                        Tb_MD_Pre_Per_Juridica empresa = null;
                        empresa = context.Tb_MD_Pre_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(cliente.vNroDocumento) && x.idPreCliente == cliente.idPreCliente);
                        if (empresa == null)
                        {
                            existe = false;
                            empresa = new Tb_MD_Pre_Per_Juridica();
                            empresa.idPreCliente = cliente.idPreCliente;
                            empresa.vNumDocumento = model.empresa.ruc;
                            empresa.dFechaCreacion = DateTime.Now;
                            empresa.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                            cliente.vTipoDocumento = System.Configuration.ConfigurationManager.AppSettings["td_empresa"];
                            cliente.iEstadoNavegacion = 2;
                        }

                        empresa.vNombreEntidad = model.empresa.nombre;
                        empresa.vRazonSocial = model.empresa.nombre;
                        empresa.ActividadEconomica = model.empresa.actividadEconomica;
                        empresa.OrigenFondos = model.empresa.origenFondos;
                        empresa.vIdPaisOrigen = model.empresa.pais;
                        empresa.iCodDepartamento = model.empresa.departamento;
                        empresa.iCodProvincia = model.empresa.provincia;
                        empresa.iCodDistrito = model.empresa.distrito;
                        empresa.vDireccion = model.empresa.direccion;
                        cliente.NombreCliente = model.empresa.nombre;
                        cliente.iCodDepartamento = model.empresa.departamento;
                        cliente.iCodDistrito = model.empresa.distrito;

                        if (!existe)
                            context.Tb_MD_Pre_Per_Juridica.Add(empresa);


                        #region Representante Legal
                        Tb_MD_Pre_Per_Natural repreLegal = null;
                        repreLegal = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.repreLegal.nroDocumento) && x.Id_Pre_Cliente == cliente.idPreCliente);
                        {

                            if (repreLegal == null)
                            {
                                if (repreLegal != null)
                                    context.Tb_MD_Pre_Per_Natural.Remove(repreLegal);

                                repreLegal = new Tb_MD_Pre_Per_Natural();
                                repreLegal.dFechaCreacion = DateTime.Now;
                                repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                repreLegal.vNombre = model.repreLegal.nombres;
                                repreLegal.vApellido = model.repreLegal.apePaterno;
                                repreLegal.vApellidoMat = model.repreLegal.apeMaterno;
                                repreLegal.vPreCelular = model.repreLegal.preCelular;
                                repreLegal.vTelefonoMovil = model.repreLegal.celular;
                                repreLegal.Id_Pre_Cliente = cliente.idPreCliente;
                                repreLegal.vMailContacto = model.repreLegal.email;
                                repreLegal.vFlgExpuestoPoliticamente = "N";

                                context.Tb_MD_Pre_Per_Natural.Add(repreLegal);

                            }

                        }

                        empresa.Tb_MD_Pre_Per_Natural = repreLegal;

                        #endregion

                        #region Persona Autorizada
                        {
                            List<Tb_MD_Pre_Accionistas> mis_accionistas_db = context.Tb_MD_Pre_Accionistas.Where(x => x.IdPreCliente == cliente.idPreCliente).ToList();
                            if (mis_accionistas_db.Count > 0)
                                context.Tb_MD_Pre_Accionistas.RemoveRange(mis_accionistas_db);

                            foreach (var ac in model.accionistas)
                            {
                                Tb_MD_Pre_Accionistas accionista = new Tb_MD_Pre_Accionistas();
                                accionista.dFechaCreacion = DateTime.Now;
                                accionista.IdPersonaNatural = ac.nroDocumento;
                                accionista.vTipoDocumento = ac.tipoDocumento ?? "DNI";
                                accionista.vNombre = ac.nombres;
                                accionista.vApellido = ac.apePaterno;
                                accionista.vApellidoMat = ac.apeMaterno;
                                accionista.vPreCelular = ac.preCelular;
                                accionista.vTelefonoMovil = ac.celular;
                                accionista.vMailContacto = ac.email;
                                accionista.IdPreCliente = cliente.idPreCliente;
                                accionista.IdEmpresa = empresa.vNumDocumento;
                                accionista.EstadoRegistro = EstadoRegistroTabla.Activo;
                                context.Tb_MD_Pre_Accionistas.Add(accionista);

                            }
                        }
                        #endregion

                        cliente.Seguimiento = SeguimientoRegistro.RegistroPersonaAutorizada;
                        cliente.dFechaValidacionPaso2 = DateTime.Now;


                        cliente.SecretId = Guid.NewGuid();

                        context.SaveChanges();
                        transaction.Commit();

                        result.data = cliente.SecretId.ToString();
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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> registrarPersonaAutorizada(PersonaAutorizadaRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Cliente Validacion
                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);
                        #endregion

                        Tb_MD_Pre_Per_Juridica empresa = null;
                        empresa = context.Tb_MD_Pre_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(cliente.vNroDocumento) && x.idPreCliente == cliente.idPreCliente);

                        if (empresa == null)
                            throw new Exception("No se puede encontrear la empresa, para esta persona autorizada.");

                        //foreach (var pa in model.autorizados)
                        //{
                        //Tb_MD_Per_Natural perAutorizada = null;
                        //perAutorizada = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(pa.nroDocumento));
                        //{
                        //if (perAutorizada == null)
                        //{
                        //    perAutorizada = new Tb_MD_Per_Natural();
                        //    perAutorizada.dFechaCreacion = DateTime.Now;
                        //    perAutorizada.vTipoDocumento = pa.tipoDocumento;
                        //    perAutorizada.vNumDocumento = pa.nroDocumento;
                        //    perAutorizada.vNombre = pa.nombres;
                        //    perAutorizada.vApellido = pa.apePaterno;
                        //    perAutorizada.vApellidoMat = pa.apeMaterno;
                        //    perAutorizada.vPreCelular = pa.preCelular;
                        //    perAutorizada.vTelefonoMovil = pa.celular;
                        //    perAutorizada.vMailContacto = pa.email;
                        //    perAutorizada.vEstadoRegistro = EstadoRegistroTabla.Activo;


                        //    context.Tb_MD_Per_Natural.Add(perAutorizada);

                        //}

                        PersonaNatutalRequest pa = model.autorizados[0];
                        Tb_MD_Pre_Empresa_PersonaAutorizada empreaPerAuth = null;

                        empreaPerAuth = context.Tb_MD_Pre_Empresa_PersonaAutorizada.FirstOrDefault(x => x.IdPreCliente == cliente.idPreCliente && x.IdPersonaAutorizada.Equals(pa.nroDocumento));

                        if (empreaPerAuth != null)
                            context.Tb_MD_Pre_Empresa_PersonaAutorizada.Remove(empreaPerAuth);

                        empreaPerAuth = new Tb_MD_Pre_Empresa_PersonaAutorizada();
                        empreaPerAuth.IdPersonaAutorizada = pa.nroDocumento;
                        empreaPerAuth.vNombre = pa.nombres;
                        empreaPerAuth.vApellido = pa.apePaterno;
                        empreaPerAuth.vApellidoMat = pa.apeMaterno;
                        empreaPerAuth.vTipoDocumento = pa.tipoDocumento;
                        empreaPerAuth.vPreCelular = pa.preCelular;
                        empreaPerAuth.vTelefonoMovil = pa.celular;
                        empreaPerAuth.IdEmpresa = empresa.vNumDocumento;
                        empreaPerAuth.IdPreCliente = cliente.idPreCliente;
                        empreaPerAuth.IdCargo = pa.cargo;
                        empreaPerAuth.Principal = true;
                        empreaPerAuth.vMailContacto = pa.email;
                        empreaPerAuth.EstadoRegistro = EstadoRegistroTabla.Activo;
                        empreaPerAuth.FechaCreacion = DateTime.Now;
                        context.Tb_MD_Pre_Empresa_PersonaAutorizada.Add(empreaPerAuth);

                        //Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email();
                        //cuenta.iIdPerjuridica = empresa.vNumDocumento;
                        //cuenta.vNumDocumento = perAutorizada.vNumDocumento;
                        //cuenta.vMailContacto = pa.email;
                        //cuenta.vEstadoRegsitro = EstadoRegistroTabla.NoActivo;
                        //cuenta.vRol = RolPersonaEmpresa.Autorizado;
                        //context.Tb_MD_Cuentas_Email.Add(cuenta);
                        //    }
                        //}



                        cliente.vEstadoRegistro = (int)EstadoRegistroCliente.datosBasicos;
                        cliente.Seguimiento = SeguimientoRegistro.RegistroDatosBancarios;
                        cliente.iEstadoNavegacion = 3;
                        // cliente.SecretId = Guid.NewGuid();

                        context.SaveChanges();
                        transaction.Commit();

                        result.success = true;
                        result.data = cliente.SecretId.ToString();

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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> validarRegistroOperador(string id)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Cliente Validacion
                        Guid sid = Guid.NewGuid();
                        try
                        {
                            sid = Guid.Parse(id);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);
                        #endregion

                        cliente.dFechaValidacionPaso4 = DateTime.Now;
                        cliente.Seguimiento = SeguimientoRegistro.CrearPassword;


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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }

                }
            }

            return result;
        }

        public BaseResponse<PersonaNatutalRequest> getPersonaAutorizadaForRegistro(RegistroCrearPassWord_Request model)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            result.data = new PersonaNatutalRequest();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Cliente Validacion
                        Guid sid = Guid.NewGuid();
                        DateTime ahora = DateTime.Now;
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Pre_Clientes cliente = null;
                        cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                        if (cliente == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);
                        #endregion

                        Tb_MD_Pre_Empresa_PersonaAutorizada autorizado = null;
                        autorizado = context.Tb_MD_Pre_Empresa_PersonaAutorizada.FirstOrDefault(x => x.IdPreCliente == cliente.idPreCliente && x.Principal == true);

                        if (autorizado != null)
                        {
                            result.data.nroDocumento = autorizado.IdPersonaAutorizada;
                            result.data.tipoDocumento = autorizado.vTipoDocumento;
                            result.data.nombres = autorizado.vNombre;
                            result.data.apeMaterno = autorizado.vApellidoMat;
                            result.data.apePaterno = autorizado.vApellido;
                            result.data.preCelular = autorizado.vPreCelular;
                            result.data.celular = autorizado.vTelefonoMovil;
                            result.data.email = autorizado.vMailContacto;
                            result.data.cargo = autorizado.IdCargo;
                        }
                        else
                        {
                            result.data.nroDocumento = "";
                            result.data.tipoDocumento = "DNI";
                            result.data.nombres = "";
                            result.data.apeMaterno = "";
                            result.data.apePaterno = "";
                            result.data.preCelular = "51";
                            result.data.celular = "";
                            result.data.email = "";

                        }

                        result.success = true;
                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                    }
                }
            }

            return result;
        }

        public BaseResponse<RegistroPassWpord_Response2> CrearPasswprd(RegistroCrearPassWord_Request model)
        {
            BaseResponse<RegistroPassWpord_Response2> result = new BaseResponse<RegistroPassWpord_Response2>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        if (model.opcionUsuario == 1)
                        {
                            #region Cliente Validacion
                            Guid sid = Guid.NewGuid();
                            DateTime ahora = DateTime.Now;
                            try
                            {
                                sid = Guid.Parse(model.sid);
                            }
                            catch (Exception)
                            {
                                throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                            }

                            Tb_MD_Pre_Clientes cliente = null;
                            cliente = context.Tb_MD_Pre_Clientes.First(x => x.SecretId.Equals(sid));

                            if (cliente == null)
                                throw new Exception(ErroresValidacion.ClienteNoExiste);
                            #endregion


                            Tb_MD_Clientes tb_cliente = new Tb_MD_Clientes();

                            tb_cliente.vNombre = cliente.vNombre;
                            tb_cliente.vApellido = cliente.vApellido;
                            tb_cliente.vEmail = cliente.vEmail;
                            tb_cliente.vCelular = cliente.vCelular;
                            tb_cliente.vTipoCliente = cliente.vTipoCliente;
                            tb_cliente.iEstadoRegistro = EstadoRegistroTabla.Activo;
                            tb_cliente.vTipoDocumento = cliente.vTipoDocumento;
                            tb_cliente.vNroDocumento = cliente.vNroDocumento;
                            tb_cliente.iCodDepartamento = cliente.iCodDepartamento;
                            tb_cliente.iCodDistrito = cliente.iCodDistrito;
                            tb_cliente.SecretId = Guid.NewGuid();
                            tb_cliente.dFechaCreacion = ahora;
                            tb_cliente.NombreCliente = cliente.NombreCliente;
                            tb_cliente.idPreCliente = cliente.idPreCliente;
                            context.Tb_MD_Clientes.Add(tb_cliente);

                            context.SaveChanges();

                            /*Notificacion*/

                            Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                            notificacion.IdUsuario = "";
                            notificacion.IdCliente = tb_cliente.iIdCliente;
                            notificacion.Titulo = "Cuenta Activa";
                            notificacion.Mensaje = "Por unica vez para cumplir con la reglamentación de la unidad de inteligencia financieras UIF - SBS";
                            notificacion.Tipo = 1;
                            notificacion.Url = "";
                            notificacion.Fecha = DateTime.Now.AddMinutes(5);
                            notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                            context.Tb_MD_Notificacion.Add(notificacion);


                            if (cliente.vTipoCliente == TipoCliente.PersonaNatural)
                            {
                                #region Registro Persona Natural

                                Tb_MD_Pre_Per_Natural prePersona = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.Id_Pre_Cliente == cliente.idPreCliente);

                                Tb_MD_Per_Natural persona = new Tb_MD_Per_Natural
                                {
                                    vNumDocumento = prePersona.vNumDocumento,
                                    vDireccion = prePersona.vDireccion,
                                    vTelefonoMovil = prePersona.vTelefonoMovil,
                                    vOcupacion = prePersona.vOcupacion,
                                    vMailContacto = prePersona.vMailContacto,
                                    vIdPaisOrigen = prePersona.vIdPaisOrigen,
                                    vFlgExpuestoPoliticamente = prePersona.vFlgExpuestoPoliticamente,
                                    iCodDistrito = prePersona.iCodDistrito,
                                    dFechaCreacion = ahora,
                                    iCodDepartamento = prePersona.iCodDepartamento,
                                    vNombre = prePersona.vNombre,
                                    vApellido = prePersona.vApellido,
                                    vApellidoMat = prePersona.vApellidoMat,
                                    vTipoDocumento = prePersona.vTipoDocumento,
                                    vFechaNacimiento = prePersona.vFechaNacimiento,
                                    iOrigenFondos = prePersona.iOrigenFondos,
                                    vEstadoRegistro = EstadoRegistroTabla.Activo,
                                    vPreCelular = prePersona.vPreCelular,
                                    iCodProvincia = prePersona.iCodProvincia,
                                    vFlgSituacionLaboral = prePersona.vFlgSituacionLaboral

                                };

                                context.Tb_MD_Per_Natural.Add(persona);

                                if (prePersona.vFlgExpuestoPoliticamente.Equals("S"))
                                {
                                    Tb_MD_Expuesto_Politicamente expuesto;

                                    expuesto = new Tb_MD_Expuesto_Politicamente();
                                    expuesto.vNumDocumento = prePersona.vNumDocumento;
                                    expuesto.vNombreEntidad = prePersona.NombreEntidadExpuesto;
                                    expuesto.vCargo = prePersona.CargoExpuesto;


                                    context.Tb_MD_Expuesto_Politicamente.Add(expuesto);
                                }



                                #endregion

                                #region Registro Usuario
                                string clave = Encrypt.EncryptKey(model.password);
                                Tb_MD_ClienteUsuario usuario = new Tb_MD_ClienteUsuario();
                                usuario.Email = cliente.vEmail;
                                usuario.EstadoREgistro = EstadoRegistroTabla.Activo;
                                usuario.Password = clave;
                                usuario.Creado = ahora;
                                usuario.IdCliente = tb_cliente.iIdCliente;
                                //usuario.NombreCliente = string.Format("{0} {1}", cliente.vNombre, cliente.vApellido);
                                usuario.NombreCliente = string.Format("{0}", cliente.vNombre);
                                usuario.TipoCliente = cliente.vTipoCliente;
                                usuario.vNroDocumento = cliente.vNroDocumento;

                                string[] nomnbres = cliente.vNombre.Split(' ');
                                if (nomnbres.Length > 1)
                                {
                                    string nombre1 = "";
                                    string nombre2 = "";

                                    if (nomnbres[0].Length > 0)
                                    {
                                        nombre1 = nomnbres[0].Substring(0, 1);
                                    }
                                    if (nomnbres[1].Length > 0)
                                    {
                                        nombre2 = nomnbres[1].Substring(0, 1);
                                    }
                                    else
                                    {
                                        nombre2 = cliente.vApellido.Substring(0, 1);
                                    }
                                    //usuario.Iniciales = string.Format("{0}{1}", nombre1, nombre2);
                                    usuario.Iniciales = string.Format("{0}", nombre1);
                                }
                                else
                                {
                                    //usuario.Iniciales = string.Format("{0}{1}", cliente.vNombre.Substring(0, 1), cliente.vApellido.Substring(0, 1));
                                    usuario.Iniciales = string.Format("{0}", cliente.vNombre.Substring(0, 1));
                                }


                                context.Tb_MD_ClienteUsuario.Add(usuario);

                                #endregion
                                context.SaveChanges();
                                result.data = new RegistroPassWpord_Response2
                                {
                                    email = cliente.vEmail,
                                    IdCliente = tb_cliente.iIdCliente,
                                    IdUsuario = usuario.IdUsuario,
                                    Iniciales = usuario.Iniciales,
                                    NombreCliente = usuario.NombreCliente,
                                    vNroDocumento = usuario.vNroDocumento,
                                    TipoCliente = usuario.TipoCliente
                                };

                            }
                            else
                            {
                                #region Registro Persona Juridica
                                Tb_MD_Pre_Per_Juridica preEmpresa = context.Tb_MD_Pre_Per_Juridica.FirstOrDefault(x => x.idPreCliente == cliente.idPreCliente);

                                Tb_MD_Pre_Per_Natural representante = preEmpresa.Tb_MD_Pre_Per_Natural;

                                Tb_MD_Per_Juridica validaEmpresa = null;

                                validaEmpresa = context.Tb_MD_Per_Juridica.Where(x => x.vNumDocumento == preEmpresa.vNumDocumento).FirstOrDefault();
                                if (validaEmpresa != null)
                                {
                                    throw new Exception("Empresa ya has sido registrada Anteriormente, comuniquese con dicha empresa para registrarle un usuario");

                                }


                                Tb_MD_Per_Juridica empresa = new Tb_MD_Per_Juridica
                                {
                                    vNumDocumento = preEmpresa.vNumDocumento,
                                    vRazonSocial = preEmpresa.vRazonSocial,
                                    vDireccion = preEmpresa.vDireccion,
                                    dFechaCreacion = DateTime.Now,
                                    vEstadoRegsitro = EstadoRegistroTabla.Activo,
                                    vNombreEntidad = preEmpresa.vNombreEntidad,
                                    iCodDistrito = preEmpresa.iCodDistrito,
                                    iCodDepartamento = preEmpresa.iCodDepartamento,
                                    vRepresentanteLegal = representante.vNumDocumento,
                                    ActividadEconomica = preEmpresa.ActividadEconomica,
                                    OrigenFondos = preEmpresa.OrigenFondos,
                                    vIdPaisOrigen = preEmpresa.vIdPaisOrigen,
                                    iCodProvincia = preEmpresa.iCodProvincia
                                };



                                #region Representante Legal
                                {
                                    Tb_MD_Per_Natural persona = null;
                                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(representante.vNumDocumento));

                                    if (persona == null)
                                    {
                                        persona = new Tb_MD_Per_Natural();
                                        persona.vTipoDocumento = representante.vTipoDocumento;
                                        persona.vNumDocumento = representante.vNumDocumento;
                                        persona.vNombre = representante.vNombre;
                                        persona.dFechaCreacion = ahora;
                                        persona.vApellido = representante.vApellido;
                                        persona.vApellidoMat = representante.vApellidoMat;
                                        persona.vPreCelular = representante.vPreCelular;
                                        persona.vTelefonoMovil = representante.vTelefonoMovil;
                                        persona.vMailContacto = representante.vMailContacto;
                                        persona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Per_Natural.Add(persona);
                                        //2
                                        context.SaveChanges();
                                    }

                                    Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email
                                    {
                                        iIdPerjuridica = empresa.vNumDocumento,
                                        vEstadoRegsitro = EstadoRegistroTabla.Activo,
                                        vMailContacto = representante.vMailContacto,
                                        vNumDocumento = representante.vNumDocumento,
                                        vRol = RolPersonaEmpresa.RepresentanteLegal
                                    };

                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                    //2
                                    context.SaveChanges();

                                }


                                #endregion

                                context.Tb_MD_Per_Juridica.Add(empresa);
                                //2
                                context.SaveChanges();

                                #region Accionistas
                                List<Tb_MD_Pre_Accionistas> PreAccionistas = context.Tb_MD_Pre_Accionistas.Where(x => x.IdPreCliente == cliente.idPreCliente).ToList();

                                foreach (var pa in PreAccionistas)
                                {
                                    Tb_MD_Per_Natural persona = null;
                                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(pa.IdPersonaNatural));

                                    if (persona == null)
                                    {
                                        persona = new Tb_MD_Per_Natural();
                                        persona.vTipoDocumento = pa.vTipoDocumento;
                                        persona.vNumDocumento = pa.IdPersonaNatural;
                                        persona.vNombre = pa.vNombre;
                                        persona.vApellido = pa.vApellido;
                                        persona.dFechaCreacion = ahora;
                                        persona.vApellidoMat = pa.vApellidoMat;
                                        persona.vPreCelular = pa.vPreCelular;
                                        persona.vTelefonoMovil = pa.vTelefonoMovil;
                                        persona.vMailContacto = pa.vMailContacto == null ? "NO" : pa.vMailContacto;
                                        persona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Per_Natural.Add(persona);
                                    }

                                    Tb_MD_Accionistas accionosta = new Tb_MD_Accionistas
                                    {
                                        dFechaCreacion = DateTime.Now,
                                        EstadoRegistro = EstadoRegistroTabla.Activo,
                                        IdEmpresa = empresa.vNumDocumento,
                                        IdPersonaNatural = persona.vNumDocumento
                                    };

                                    context.Tb_MD_Accionistas.Add(accionosta);

                                    Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email
                                    {
                                        iIdPerjuridica = empresa.vNumDocumento,
                                        vEstadoRegsitro = EstadoRegistroTabla.Activo,
                                        vMailContacto = persona.vMailContacto == null ? "NO" : persona.vMailContacto,
                                        vNumDocumento = persona.vNumDocumento,
                                        vRol = RolPersonaEmpresa.Accionista
                                    };

                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                }
                                //1
                                context.SaveChanges();
                                #endregion

                                #region Registro Usuario
                                string clave = Encrypt.EncryptKey(model.password);
                                Tb_MD_ClienteUsuario usuario = new Tb_MD_ClienteUsuario();
                                usuario.Email = cliente.vEmail;
                                usuario.EstadoREgistro = EstadoRegistroTabla.Activo;
                                usuario.Password = clave;
                                usuario.Creado = ahora;
                                usuario.IdCliente = tb_cliente.iIdCliente;
                                usuario.NombreCliente = string.Format("{0}", cliente.nombreEmpresa);
                                usuario.TipoCliente = cliente.vTipoCliente;
                                usuario.vNroDocumento = cliente.vNroDocumento;
                                usuario.Iniciales = string.Format("{0}", cliente.nombreEmpresa.Substring(0, 1));

                                context.Tb_MD_ClienteUsuario.Add(usuario);
                                context.SaveChanges();
                                #endregion

                                #region Persona Autoriazadas

                                List<Tb_MD_Pre_Empresa_PersonaAutorizada> preAutizados = context.Tb_MD_Pre_Empresa_PersonaAutorizada.Where(x => x.IdPreCliente == cliente.idPreCliente).ToList();

                                foreach (var pa in preAutizados)
                                {
                                    Tb_MD_Per_Natural persona = null;
                                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(pa.IdPersonaAutorizada));

                                    if (persona == null)
                                    {
                                        persona = new Tb_MD_Per_Natural();
                                        persona.vTipoDocumento = pa.vTipoDocumento;
                                        persona.vNumDocumento = pa.IdPersonaAutorizada;
                                        persona.vNombre = pa.vNombre;
                                        persona.dFechaCreacion = ahora;
                                        persona.vApellido = pa.vApellido;
                                        persona.vApellidoMat = pa.vApellidoMat;
                                        persona.vPreCelular = pa.vPreCelular;
                                        persona.vTelefonoMovil = pa.vTelefonoMovil;
                                        persona.vMailContacto = pa.vMailContacto;
                                        persona.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Per_Natural.Add(persona);
                                    }

                                    Tb_MD_Empresa_PersonaAutorizada autorizado = new Tb_MD_Empresa_PersonaAutorizada
                                    {
                                        EstadoRegistro = EstadoRegistroTabla.Activo,
                                        FechaCreacion = ahora,
                                        IdCargo = pa.IdCargo,
                                        IdEmpresa = empresa.vNumDocumento,
                                        IdPersonaAutorizada = persona.vNumDocumento,
                                        Principal = true,
                                        SecredId = Guid.NewGuid(),
                                        Usuario = usuario.IdUsuario
                                    };

                                    context.Tb_MD_Empresa_PersonaAutorizada.Add(autorizado);

                                    Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email
                                    {
                                        iIdPerjuridica = empresa.vNumDocumento,
                                        vEstadoRegsitro = EstadoRegistroTabla.Activo,
                                        vMailContacto = persona.vMailContacto,
                                        vNumDocumento = persona.vNumDocumento,
                                        vRol = RolPersonaEmpresa.Autorizado
                                    };

                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                }

                                #endregion



                                #endregion

                                context.SaveChanges();
                                result.data = new RegistroPassWpord_Response2
                                {
                                    email = cliente.vEmail,
                                    IdCliente = tb_cliente.iIdCliente,
                                    IdUsuario = usuario.IdUsuario,
                                    Iniciales = usuario.Iniciales,
                                    NombreCliente = usuario.NombreCliente,
                                    vNroDocumento = usuario.vNroDocumento,
                                    TipoCliente = usuario.TipoCliente
                                };
                            }

                            #region Registro Datos Bancarios
                            List<Tb_MD_Pre_ClientesDatosBancos> cuentas = new List<Tb_MD_Pre_ClientesDatosBancos>();
                            cuentas = context.Tb_MD_Pre_ClientesDatosBancos.Where(x => x.iIdCliente == cliente.idPreCliente && x.vEstadoRegistro == EstadoRegistroTabla.Activo).ToList();

                            foreach (var c in cuentas)
                            {
                                Tb_MD_ClientesDatosBancos cuenta = new Tb_MD_ClientesDatosBancos
                                {
                                    dFechaCreacion = ahora,
                                    iIdCliente = tb_cliente.iIdCliente,
                                    vBanco = c.vBanco,
                                    vMoneda = c.vMoneda,
                                    vNroCuenta = c.vNroCuenta,
                                    iTipoCuenta = c.iTipoCuenta,
                                    vEstadoRegistro = EstadoRegistroTabla.Activo,
                                    vTipoPersona = tb_cliente.vTipoCliente,
                                    vNroDocumento = tb_cliente.vNroDocumento,
                                    vSecredId = Guid.NewGuid()

                                };

                                context.Tb_MD_ClientesDatosBancos.Add(cuenta);
                            }

                            #endregion

                            cliente.Finalizado = true;
                            cliente.Seguimiento = SeguimientoRegistro.Finalizado;
                            cliente.dFechaValidacionPaso5 = ahora;
                            cliente.vEstadoRegistro = (int)EstadoRegistroCliente.creacionPassword;

                        }
                        else
                        {

                            string clave = Encrypt.EncryptKey(model.password);
                            Tb_MD_ClienteUsuario usuario = context.Tb_MD_ClienteUsuario.Where(y => y.Email == model.email).FirstOrDefault();

                            if (usuario != null)
                            {
                                usuario.Email = model.email;
                                usuario.EstadoREgistro = EstadoRegistroTabla.Activo;
                                usuario.Password = clave;
                                usuario.Modificado = DateTime.Now;
                                usuario.SecredId = null;
                                result.data = new RegistroPassWpord_Response2
                                {
                                    email = usuario.Email,
                                    IdCliente = usuario.Tb_MD_Clientes.iIdCliente,
                                    IdUsuario = usuario.IdUsuario,
                                    Iniciales = usuario.Iniciales,
                                    NombreCliente = usuario.NombreCliente,
                                    vNroDocumento = usuario.vNroDocumento,
                                    TipoCliente = usuario.TipoCliente
                                };

                            }

                        }

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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }


            return result;

        }

        /*---Registro Fideicomiso---*/

        public BaseResponse<string> registrarPersonaFideicomiso(PersonaJuridicaFideicomisoRegistroRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        Tb_MD_Per_Juridica empresa = null;
                        //empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(cliente.vNroDocumento));
                        empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.empresa.ruc));

                    

                        if (empresa == null)
                        {
                            empresa = new Tb_MD_Per_Juridica();
                            empresa.vNumDocumento = model.empresa.ruc;
                            empresa.vNombreEntidad = model.empresa.nombre;
                            empresa.vRazonSocial = model.empresa.nombre;
                            empresa.ActividadEconomica = model.empresa.actividadEconomica;
                            empresa.OrigenFondos = model.empresa.origenFondos;
                            empresa.dFechaCreacion = DateTime.Now;
                            empresa.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                            //empresa.vIdPaisOrigen = model.empresa.pais;
                            //empresa.iCodDepartamento = model.empresa.departamento;
                            //empresa.iCodProvincia = model.empresa.provincia;
                            //empresa.iCodDistrito = model.empresa.distrito;
                            //empresa.vDireccion = model.empresa.direccion;
                            if (model.tipoEmpresa == "Parnert")
                            {
                                empresa.IsPartner = true;
                                empresa.IsFideicomiso = false;
                            }

                            if (model.tipoEmpresa == "Fideicomiso")
                            {
                                empresa.IsPartner = false;
                                empresa.IsFideicomiso = true;
                            }

                            if (model.tipoEmpresa == "Canal" || model.tipoEmpresa == "LMD")
                            {
                                empresa.IsPartner = true;
                                empresa.IsFideicomiso = false;
                            }


                            //cliente.NombreCliente = model.empresa.nombre;
                            //cliente.vTipoDocumento = System.Configuration.ConfigurationManager.AppSettings["td_empresa"];
                            //cliente.iCodDepartamento = model.empresa.departamento;
                            //cliente.iCodDistrito = model.empresa.distrito;

                            context.Tb_MD_Per_Juridica.Add(empresa);

                            Tb_MD_Per_Natural repreLegal = null;
                            repreLegal = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.repreLegal.nroDocumento));
                          
                                if (repreLegal == null)
                                {
                                    repreLegal = new Tb_MD_Per_Natural();
                                    repreLegal.dFechaCreacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;
                                    //repreLegal.vPreCelular = model.repreLegal.preCelular;
                                    //repreLegal.vTelefonoMovil = model.repreLegal.celular;
                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Per_Natural.Add(repreLegal);

                                }

                            Tb_MD_Cuentas_Email cuentaRep = context.Tb_MD_Cuentas_Email.FirstOrDefault(x=>x.iIdPerjuridica==empresa.vNumDocumento && x.vNumDocumento==repreLegal.vNumDocumento);

                            if (cuentaRep == null) {
                                cuentaRep = new Tb_MD_Cuentas_Email();
                                cuentaRep.iIdPerjuridica = empresa.vNumDocumento;
                                cuentaRep.vNumDocumento = repreLegal.vNumDocumento;
                                cuentaRep.vMailContacto = model.repreLegal.email;
                                cuentaRep.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                cuentaRep.vRol = RolPersonaEmpresa.RepresentanteLegal;
                                context.Tb_MD_Cuentas_Email.Add(cuentaRep);
                            }

                           
                     

                            empresa.vRepresentanteLegal = repreLegal.vNumDocumento;



                            foreach (var ac in model.accionistas)
                            {
                                Tb_MD_Per_Natural PerAcc = null;
                                PerAcc = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(ac.nroDocumento));
                                if (PerAcc == null)
                                {
                                    PerAcc = new Tb_MD_Per_Natural();
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    //PerAcc.vPreCelular = ac.preCelular;
                                    //PerAcc.vTelefonoMovil = ac.celular;
                                    //PerAcc.vMailContacto = ac.email;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Per_Natural.Add(PerAcc);


                                }

                                Tb_MD_Cuentas_Email cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica == empresa.vNumDocumento && x.vNumDocumento == PerAcc.vNumDocumento);

                                if (cuenta == null)
                                {
                                    cuenta = new Tb_MD_Cuentas_Email();
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = PerAcc.vNumDocumento;
                                    cuenta.vMailContacto = ac.email;
                                    cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                    cuenta.vRol = RolPersonaEmpresa.Accionista;
                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                }


                                Tb_MD_Accionistas accionista = context.Tb_MD_Accionistas.FirstOrDefault(x => x.IdEmpresa == empresa.vNumDocumento && x.IdPersonaNatural == PerAcc.vNumDocumento);
                                if (accionista == null)
                                {

                                    accionista = new Tb_MD_Accionistas();
                                    accionista.dFechaCreacion = DateTime.Now;
                                    accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                    accionista.IdEmpresa = empresa.vNumDocumento;
                                    context.Tb_MD_Accionistas.Add(accionista);
                                }
                            }

                            /*Persona Autorizada*/

                            Tb_MD_Per_Natural perAutorizada = null;
                            perAutorizada = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.personaAutorizada.nroDocumento));
                       
                                if (perAutorizada == null)
                                {
                                    perAutorizada = new Tb_MD_Per_Natural();
                                    perAutorizada.dFechaCreacion = DateTime.Now;
                                    perAutorizada.vTipoDocumento = model.personaAutorizada.tipoDocumento;
                                    perAutorizada.vNumDocumento = model.personaAutorizada.nroDocumento;
                                    perAutorizada.vNombre = model.personaAutorizada.nombres;
                                    perAutorizada.vApellido = model.personaAutorizada.apePaterno;
                                    perAutorizada.vApellidoMat = model.personaAutorizada.apeMaterno;
                                    perAutorizada.vPreCelular = model.personaAutorizada.preCelular;
                                    perAutorizada.vTelefonoMovil = model.personaAutorizada.celular;
                                    perAutorizada.vMailContacto = model.personaAutorizada.email;
                                    perAutorizada.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Per_Natural.Add(perAutorizada);
                                }


                            Tb_MD_Empresa_PersonaAutorizada empreaPerAuth = context.Tb_MD_Empresa_PersonaAutorizada.FirstOrDefault(x => x.IdEmpresa == empresa.vNumDocumento && x.IdPersonaAutorizada == perAutorizada.vNumDocumento);

                            if (empreaPerAuth == null) {
                                empreaPerAuth = new Tb_MD_Empresa_PersonaAutorizada();
                                empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                empreaPerAuth.IdEmpresa = empresa.vNumDocumento;
                                empreaPerAuth.IdCargo = model.personaAutorizada.cargo;
                                empreaPerAuth.EstadoRegistro = EstadoRegistroTabla.Activo;
                                empreaPerAuth.FechaCreacion = DateTime.Now;
                                context.Tb_MD_Empresa_PersonaAutorizada.Add(empreaPerAuth);
                            }
                                

                                Tb_MD_Cuentas_Email cuentaAut = new Tb_MD_Cuentas_Email();

                            cuentaAut = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica == empresa.vNumDocumento && x.vNumDocumento == perAutorizada.vNumDocumento);

                            if (cuentaAut == null) {
                                cuentaAut = new Tb_MD_Cuentas_Email();
                                cuentaAut.iIdPerjuridica = empresa.vNumDocumento;
                                cuentaAut.vNumDocumento = perAutorizada.vNumDocumento;
                                cuentaAut.vMailContacto = model.personaAutorizada.email;
                                cuentaAut.vEstadoRegsitro = EstadoRegistroTabla.NoActivo;
                                cuentaAut.vRol = RolPersonaEmpresa.Autorizado;
                                context.Tb_MD_Cuentas_Email.Add(cuentaAut);
                            }
                        
                         

                            /*Cuentas Bancarias */
                            IList<Tb_MD_Cuentas_Bancarias> TCuentas;
                            TCuentas = new List<Tb_MD_Cuentas_Bancarias>();
                            foreach (var cb in model.cuentas)
                            {
                                Tb_MD_Cuentas_Bancarias cuenta = new Tb_MD_Cuentas_Bancarias();
                                cuenta.vCodEntidad = cb.banco;
                                cuenta.vMonedaCuenta = cb.moneda;
                                cuenta.iTipoCuenta = cb.tipoCuenta;
                                cuenta.vNumCuenta = cb.nroCuenta;
                                cuenta.dFechaCreacion = DateTime.Now;
                                cuenta.vNumDocumento = model.empresa.ruc;
                                cuenta.iEstadoRegistro = EstadoRegistroTabla.Activo;

                                TCuentas.Add(cuenta);
                            }
                            context.Tb_MD_Cuentas_Bancarias.AddRange(TCuentas);

                        }


                        context.SaveChanges();
                        transaction.Commit();

                        //result.data = cliente.SecretId.ToString();
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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }
        /* Listar datos Empresa*/

        public BaseResponse<EmpresaRegistroRequest> ListarDatosEmpresa(string NroRuc)
        {
            BaseResponse<EmpresaRegistroRequest> valorRegistrados = new BaseResponse<EmpresaRegistroRequest>();

            try
            {

                valorRegistrados.data = new EmpresaRegistroRequest();

                EmpresaRegistroRequest entidad = new EmpresaRegistroRequest();
                PersonaJuridicaReuest datosEmpresa = new PersonaJuridicaReuest();
                PersonaNatutalRequest repreLegal = new PersonaNatutalRequest();
                List<PersonaNatutalRequest> listaPersonaAutorizada = new List<PersonaNatutalRequest>();

                List<PersonaNatutalRequest> listaAccionistas = new List<PersonaNatutalRequest>();
                string tipoEmpresa = "";

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    //var usuarioEmpresa = context.Tb_MD_Cuentas_Email.Where(x => x.vNumDocumento == NroRuc).FirstOrDefault();
                    var empresa = context.Tb_MD_Per_Juridica.Where(x => x.vNumDocumento == NroRuc).FirstOrDefault();
                    var persolegal = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica == NroRuc && x.vRol == RolPersonaEmpresa.RepresentanteLegal && x.vEstadoRegsitro == EstadoRegistroTabla.Activo).FirstOrDefault();
                    //var persoAuto = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica == usuarioEmpresa.iIdPerjuridica && x.vRol == RolPersonaEmpresa.Autorizado).FirstOrDefault();

                    List<String> persoAcc = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica == NroRuc && x.vRol == RolPersonaEmpresa.Accionista).Select(y => y.vNumDocumento).ToList();
                    List<String> persoAuto = context.Tb_MD_Empresa_PersonaAutorizada.Where(x => x.IdEmpresa == NroRuc && x.EstadoRegistro == EstadoRegistroTabla.Activo).Select(y => y.IdPersonaAutorizada).ToList();

                    Tb_MD_Per_Natural legal = null;
                    if (persolegal != null)
                    {
                        legal = context.Tb_MD_Per_Natural.Where(x => x.vNumDocumento == persolegal.vNumDocumento).FirstOrDefault();
                    }

                    var autorizados = context.Tb_MD_Per_Natural.Where(x => persoAuto.Contains(x.vNumDocumento)).ToList();
                    //var cargo=context.Tb_Empresa_PersonaAutorizada.Where(x=>x.IdEmpresa==persoAuto.iIdPerjuridica && x.IdPersonaAutorizada==persoAuto.vNumDocumento).FirstOrDefault();
                    var accionista = context.Tb_MD_Per_Natural.Where(x => persoAcc.Contains(x.vNumDocumento)).ToList();
                    //RolPersonaEmpresa.Autorizado;

                    if (empresa != null)
                    {
                        datosEmpresa.ruc = empresa.vNumDocumento;
                        datosEmpresa.nombre = empresa.vRazonSocial;
                        datosEmpresa.actividadEconomica = empresa.ActividadEconomica;
                        datosEmpresa.origenFondos = empresa.OrigenFondos;
                        if (empresa.IsPartner)
                        {
                            tipoEmpresa = "Parnert";
                        }
                        if (empresa.IsFideicomiso)
                        {
                            tipoEmpresa = "Fideicomiso";
                        }
                    }

                    if (legal != null)
                    {
                        repreLegal.tipoDocumento = legal.vTipoDocumento;
                        repreLegal.nroDocumento = legal.vNumDocumento;
                        repreLegal.nombres = legal.vNombre;
                        repreLegal.apePaterno = legal.vApellido;
                        repreLegal.apeMaterno = legal.vApellidoMat;
                        repreLegal.email = legal.vMailContacto;
                    }

                    //if (persoAuto != null)
                    //{
                    //    personaAuto.tipoDocumento = autorizado.vTipoDocumento;
                    //    personaAuto.nroDocumento = autorizado.vNumDocumento;
                    //    personaAuto.nombres = autorizado.vNombre;
                    //    personaAuto.apePaterno = autorizado.vApellido;
                    //    personaAuto.apeMaterno = autorizado.vApellidoMat;
                    //    personaAuto.email = autorizado.vMailContacto;
                    //    personaAuto.preCelular = autorizado.vPreCelular;
                    //    personaAuto.celular = autorizado.vTelefonoMovil;
                    //    personaAuto.cargo = cargo.IdCargo;
                    //}

                    autorizados.ForEach(m =>
                    {
                        PersonaNatutalRequest pa = new PersonaNatutalRequest();
                        pa.tipoDocumento = m.vTipoDocumento;
                        pa.nroDocumento = m.vNumDocumento;
                        pa.nombres = m.vNombre;
                        pa.apePaterno = m.vApellido;
                        pa.apeMaterno = m.vApellidoMat;

                        pa.email = m.vMailContacto;
                        pa.preCelular = m.vPreCelular;
                        pa.celular = m.vTelefonoMovil;
                        pa.cargo = context.Tb_MD_Empresa_PersonaAutorizada.Where(x => x.IdEmpresa == NroRuc && x.IdPersonaAutorizada == m.vNumDocumento).Select(y => y.IdCargo).FirstOrDefault();
                        pa.estado = m.vEstadoRegistro;
                        listaPersonaAutorizada.Add(pa);
                    });

                    accionista.ForEach(m =>
                    {
                        PersonaNatutalRequest acc = new PersonaNatutalRequest();
                        acc.tipoDocumento = m.vTipoDocumento;
                        acc.nroDocumento = m.vNumDocumento;
                        acc.nombres = m.vNombre;
                        acc.apePaterno = m.vApellido;
                        acc.apeMaterno = m.vApellidoMat;
                        acc.email = m.vMailContacto;
                        acc.estado = m.vEstadoRegistro;
                        listaAccionistas.Add(acc);
                    });

                    entidad.accionistas = listaAccionistas;
                    entidad.tipoEmpresa = tipoEmpresa;
                    entidad.empresa = datosEmpresa;
                    entidad.personaAutorizadas = listaPersonaAutorizada;
                    entidad.repreLegal = repreLegal;
                    valorRegistrados.data = entidad;

                    //valorRegistrados.data = context.Database.SqlQuery<VerificacionPagoResponse>("exec Proc_Sel_Verificacion_Pago @PageNumber,@ItemsPerPage,@ModoLista", pageParam, itemsParam, modoLista).ToList<VerificacionPagoResponse>();


                }




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

        /* Listar datos Cuentas Bancarias*/

        public BaseResponse<EmpresaRegistroRequest> ListarDatosCuentasBancarias(string nroRuc)
        {
            BaseResponse<EmpresaRegistroRequest> valorRegistrados = new BaseResponse<EmpresaRegistroRequest>();
            try
            {

                valorRegistrados.data = new EmpresaRegistroRequest();

                EmpresaRegistroRequest entidad = new EmpresaRegistroRequest();
                List<DatoBancariaRequest> listaCuentaBancarias = new List<DatoBancariaRequest>();


                using (MesaDineroContext context = new MesaDineroContext())
                {
                    //var usuarioEmpresa = context.Tb_MD_Cuentas_Email.Where(x => x.vNumDocumento.Equals(nroRuc)).FirstOrDefault();



                    var cuentas = context.Tb_MD_Cuentas_Bancarias.Where(x => x.vNumDocumento == nroRuc && x.iEstadoRegistro == EstadoRegistroTabla.Activo).ToList();

                    string host = ConfigurationManager.AppSettings["HostAdmin"];
                    cuentas.ForEach(m =>
                    {
                        int tipocuenta = Convert.ToInt16(m.iTipoCuenta);
                        DatoBancariaRequest pa = new DatoBancariaRequest();
                        var entidadFinanciera = context.Tb_MD_Entidades_Financieras.Find(m.vCodEntidad);
                        var moneda = context.Tb_MD_TipoMoneda.Find(m.vMonedaCuenta);
                        var TipoCuenta = context.Tb_MD_TipoCuentaBancaria.Find(tipocuenta);

                        pa.codigo = m.nIdCuentaEmpresa;
                        pa.banco = m.vCodEntidad;
                        pa.moneda = m.vMonedaCuenta;
                        pa.monedatext = moneda.vDesMoneda;
                        pa.tipoCuenta = m.iTipoCuenta;
                        pa.tipoCuentaText = TipoCuenta.Nombre;
                        pa.nroCuenta = m.vNumCuenta;
                        pa.logo = host + entidadFinanciera.vLogoEntidad;
                        pa.estado = 1;
                        listaCuentaBancarias.Add(pa);
                    });


                    entidad.cuentas = listaCuentaBancarias;

                    valorRegistrados.data = entidad;

                }




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

        /*---Editar datos Empresa ---*/

        public BaseResponse<string> EditarDatosPersonaJuridica(EmpresaRegistroRequest model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Tb_MD_Per_Juridica empresa = null;
                        empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.empresa.ruc));
                        if (empresa == null)
                        {
                            throw new Exception("No se encontro Empresa");
                        }
                        if (empresa != null)
                        {
                            empresa.vNombreEntidad = model.empresa.nombre;
                            empresa.vRazonSocial = model.empresa.nombre;
                            empresa.ActividadEconomica = model.empresa.actividadEconomica;
                            empresa.OrigenFondos = model.empresa.origenFondos;
                            empresa.dFechaModificacion = DateTime.Now;
                            empresa.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                            //empresa.vIdPaisOrigen = model.empresa.pais;
                            //empresa.iCodDepartamento = model.empresa.departamento;
                            //empresa.iCodProvincia = model.empresa.provincia;
                            //empresa.iCodDistrito = model.empresa.distrito;
                            //empresa.vDireccion = model.empresa.direccion;
                            if (model.tipoEmpresa == "Parnert")
                            {
                                empresa.IsPartner = true;
                                empresa.IsFideicomiso = false;
                            }

                            if (model.tipoEmpresa == "Fideicomiso")
                            {
                                empresa.IsPartner = false;
                                empresa.IsFideicomiso = true;
                            }

                            if (model.tipoEmpresa == "Canal" || model.tipoEmpresa == "LMD")
                            {
                                empresa.IsPartner = true;
                                empresa.IsFideicomiso = false;
                            }


                            //cliente.NombreCliente = model.empresa.nombre;
                            //cliente.vTipoDocumento = System.Configuration.ConfigurationManager.AppSettings["td_empresa"];
                            //cliente.iCodDepartamento = model.empresa.departamento;
                            //cliente.iCodDistrito = model.empresa.distrito;

                            //context.Tb_MD_Per_Juridica.Add(empresa);

                            Tb_MD_Per_Natural repreLegal = null;
                            repreLegal = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.repreLegal.nroDocumento));
                            {
                                if (repreLegal != null)
                                {
                                    repreLegal.dFechaModificacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;
                                    //repreLegal.vPreCelular = model.repreLegal.preCelular;
                                    //repreLegal.vTelefonoMovil = model.repreLegal.celular;
                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";

                                    //context.SaveChanges();

                                    Tb_MD_Cuentas_Email cuenta = null;
                                    cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vNumDocumento == repreLegal.vNumDocumento && x.vEstadoRegsitro == EstadoRegistroTabla.Activo);


                                    if (cuenta != null)
                                    {

                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = repreLegal.vNumDocumento;
                                        cuenta.vMailContacto = model.repreLegal.email;
                                        cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                        cuenta.vRol = RolPersonaEmpresa.RepresentanteLegal;

                                    }

                                    if (cuenta == null)
                                    {
                                        Tb_MD_Cuentas_Email cuentaAnterior = null;
                                        cuentaAnterior = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vRol == RolPersonaEmpresa.RepresentanteLegal && x.vEstadoRegsitro == EstadoRegistroTabla.Activo);
                                        if (cuentaAnterior != null)
                                        {
                                            cuentaAnterior.vEstadoRegsitro = EstadoRegistroTabla.Eliminado;
                                        }

                                        cuenta = new Tb_MD_Cuentas_Email();
                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = repreLegal.vNumDocumento;
                                        cuenta.vMailContacto = model.repreLegal.email;
                                        cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                        cuenta.vRol = RolPersonaEmpresa.RepresentanteLegal;
                                        context.Tb_MD_Cuentas_Email.Add(cuenta);
                                    }
                                }

                                if (repreLegal == null)
                                {
                                    repreLegal = new Tb_MD_Per_Natural();
                                    repreLegal.dFechaCreacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;
                                    //repreLegal.vPreCelular = model.repreLegal.preCelular;
                                    //repreLegal.vTelefonoMovil = model.repreLegal.celular;
                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Per_Natural.Add(repreLegal);
                                    //throw new Exception("No se encontro Representante Legal");

                                    Tb_MD_Cuentas_Email cuentaAnterior = null;
                                    cuentaAnterior = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vRol == RolPersonaEmpresa.RepresentanteLegal && x.vEstadoRegsitro == EstadoRegistroTabla.Activo);

                                    if (cuentaAnterior != null)
                                    {
                                        cuentaAnterior.vEstadoRegsitro = EstadoRegistroTabla.Eliminado;
                                    }

                                    Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email();
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = repreLegal.vNumDocumento;
                                    cuenta.vMailContacto = model.repreLegal.email;
                                    cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                    cuenta.vRol = RolPersonaEmpresa.RepresentanteLegal;
                                    context.Tb_MD_Cuentas_Email.Add(cuenta);

                                }





                            }

                            empresa.vRepresentanteLegal = repreLegal.vNumDocumento;

                            foreach (var ac in model.accionistas)
                            {
                                Tb_MD_Per_Natural PerAcc = null;
                                PerAcc = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(ac.nroDocumento));

                                if (PerAcc != null)
                                {
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = ac.estado;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    //Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email();
                                    //cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    //cuenta.vNumDocumento = PerAcc.vNumDocumento;
                                    //cuenta.vMailContacto = ac.email;
                                    //cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                    //cuenta.vRol = RolPersonaEmpresa.Accionista;

                                    //context.Tb_MD_Cuentas_Email.Add(cuenta);

                                    Tb_MD_Accionistas accionista;
                                    accionista = context.Tb_MD_Accionistas.FirstOrDefault(x => x.IdEmpresa.Equals(model.empresa.ruc) && x.IdPersonaNatural == PerAcc.vNumDocumento);

                                    if (accionista != null)
                                    {

                                        accionista.dFechaCreacion = DateTime.Now;
                                        accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                        accionista.IdEmpresa = empresa.vNumDocumento;
                                        accionista.EstadoRegistro = ac.estado;

                                    }

                                    if (accionista == null)
                                    {
                                        accionista = new Tb_MD_Accionistas();
                                        accionista.dFechaCreacion = DateTime.Now;
                                        accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                        accionista.IdEmpresa = empresa.vNumDocumento;
                                        accionista.EstadoRegistro = ac.estado;
                                        context.Tb_MD_Accionistas.Add(accionista);
                                    }
                                }

                                if (PerAcc == null)
                                {
                                    PerAcc = new Tb_MD_Per_Natural();
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    //PerAcc.vPreCelular = ac.preCelular;
                                    //PerAcc.vTelefonoMovil = ac.celular;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Per_Natural.Add(PerAcc);

                                    Tb_MD_Accionistas accionista = new Tb_MD_Accionistas();
                                    accionista.dFechaCreacion = DateTime.Now;
                                    accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                    accionista.IdEmpresa = empresa.vNumDocumento;
                                    accionista.EstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Accionistas.Add(accionista);

                                }



                                Tb_MD_Cuentas_Email cuenta = null;
                                cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vNumDocumento == PerAcc.vNumDocumento);

                                if (cuenta != null)
                                {
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = PerAcc.vNumDocumento;
                                    cuenta.vMailContacto = ac.email;
                                    cuenta.vEstadoRegsitro = ac.estado;
                                    cuenta.vRol = RolPersonaEmpresa.Accionista;

                                    //context.Entry(cuenta).State = EntityState.Modified;
                                }

                                if (cuenta == null)
                                {
                                    cuenta = new Tb_MD_Cuentas_Email();
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = PerAcc.vNumDocumento;
                                    cuenta.vMailContacto = ac.email;
                                    cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                    cuenta.vRol = RolPersonaEmpresa.Accionista;
                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                }
                            }

                            /*Persona Autorizada*/

                            foreach (var pa in model.personaAutorizadas)
                            {
                                Tb_MD_Per_Natural perAutorizada = null;
                                perAutorizada = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(pa.nroDocumento));
                                {

                                    if (perAutorizada != null)
                                    {
                                        perAutorizada.dFechaModificacion = DateTime.Now;
                                        perAutorizada.vTipoDocumento = pa.tipoDocumento;
                                        perAutorizada.vNumDocumento = pa.nroDocumento;
                                        perAutorizada.vNombre = pa.nombres;
                                        perAutorizada.vApellido = pa.apePaterno;
                                        perAutorizada.vApellidoMat = pa.apeMaterno;
                                        perAutorizada.vPreCelular = pa.preCelular;
                                        perAutorizada.vTelefonoMovil = pa.celular;
                                        perAutorizada.vMailContacto = pa.email;
                                        perAutorizada.vEstadoRegistro = pa.estado;
                                    }

                                    if (perAutorizada == null)
                                    {
                                        perAutorizada = new Tb_MD_Per_Natural();
                                        perAutorizada.dFechaCreacion = DateTime.Now;
                                        perAutorizada.vTipoDocumento = pa.tipoDocumento;
                                        perAutorizada.vNumDocumento = pa.nroDocumento;
                                        perAutorizada.vNombre = pa.nombres;
                                        perAutorizada.vApellido = pa.apePaterno;
                                        perAutorizada.vApellidoMat = pa.apeMaterno;
                                        perAutorizada.vPreCelular = pa.preCelular;
                                        perAutorizada.vTelefonoMovil = pa.celular;
                                        perAutorizada.vMailContacto = pa.email;
                                        perAutorizada.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Per_Natural.Add(perAutorizada);
                                    }


                                    Tb_MD_Empresa_PersonaAutorizada empreaPerAuth = new Tb_MD_Empresa_PersonaAutorizada();

                                    empreaPerAuth = context.Tb_MD_Empresa_PersonaAutorizada.FirstOrDefault(x => x.IdEmpresa.Equals(model.empresa.ruc) && x.IdPersonaAutorizada == perAutorizada.vNumDocumento);

                                    if (empreaPerAuth != null)
                                    {
                                        empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                        empreaPerAuth.IdEmpresa = empresa.vNumDocumento;
                                        empreaPerAuth.IdCargo = pa.cargo;
                                        empreaPerAuth.EstadoRegistro = Convert.ToByte(pa.estado);
                                        empreaPerAuth.FechaCreacion = DateTime.Now;
                                        //context.Tb_Empresa_PersonaAutorizada.Add(empreaPerAuth);
                                    }

                                    if (empreaPerAuth == null)
                                    {
                                        empreaPerAuth = new Tb_MD_Empresa_PersonaAutorizada();
                                        empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                        empreaPerAuth.IdEmpresa = empresa.vNumDocumento;
                                        empreaPerAuth.IdCargo = pa.cargo;
                                        empreaPerAuth.EstadoRegistro = EstadoRegistroTabla.Activo;
                                        empreaPerAuth.FechaCreacion = DateTime.Now;
                                        context.Tb_MD_Empresa_PersonaAutorizada.Add(empreaPerAuth);
                                    }


                                    Tb_MD_Cuentas_Email cuenta = null;
                                    cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vNumDocumento == perAutorizada.vNumDocumento);

                                    if (cuenta != null)
                                    {
                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = perAutorizada.vNumDocumento;
                                        cuenta.vMailContacto = pa.email;
                                        cuenta.vEstadoRegsitro = pa.estado;
                                        cuenta.vRol = RolPersonaEmpresa.Autorizado;
                                        //context.Tb_MD_Cuentas_Email.Add(cuenta);
                                    }

                                    if (cuenta == null)
                                    {
                                        cuenta = new Tb_MD_Cuentas_Email();
                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = perAutorizada.vNumDocumento;
                                        cuenta.vMailContacto = pa.email;
                                        cuenta.vEstadoRegsitro = EstadoRegistroTabla.NoActivo;
                                        cuenta.vRol = RolPersonaEmpresa.Autorizado;
                                        context.Tb_MD_Cuentas_Email.Add(cuenta);
                                    }


                                    /*  */

                                }







                            }

                            ///*Cuentas Bancarias */
                            //IList<Tb_MD_Cuentas_Bancarias> TCuentas;
                            //TCuentas = new List<Tb_MD_Cuentas_Bancarias>();
                            //foreach (var cb in model.cuentas)
                            //{
                            //    Tb_MD_Cuentas_Bancarias cuenta = new Tb_MD_Cuentas_Bancarias();
                            //    cuenta.vCodEntidad = cb.banco;
                            //    cuenta.vMonedaCuenta = cb.moneda;
                            //    cuenta.vTipoCuenta = cb.tipoCuenta.ToString();
                            //    cuenta.vNumCuenta = cb.nroCuenta;
                            //    cuenta.dFechaCreacion = DateTime.Now;
                            //    cuenta.vNumDocumento = model.empresa.ruc;
                            //    //cuenta.vCCI = cb.nroCCI;
                            //    //cuenta.vSecredId = Guid.NewGuid();
                            //    //cuenta.iIdCliente = cliente.idPreCliente;
                            //    cuenta.vEstadoRegistro = EstadoRegistroTabla.Activo.ToString();
                            //    //cuenta.vTipoPersona = cliente.vTipoCliente;

                            //    //cuenta.dFechaCreacion = DateTime.Now;

                            //    TCuentas.Add(cuenta);
                            //}
                            //context.Tb_MD_Cuentas_Bancarias.AddRange(TCuentas);

                        }

                        //cliente.Seguimiento = SeguimientoRegistro.RegistroPersonaAutorizada;
                        //cliente.dFechaValidacionPaso2 = DateTime.Now;
                        //cliente.SecretId = Guid.NewGuid();

                        context.SaveChanges();
                        transaction.Commit();

                        //result.data = cliente.SecretId.ToString();
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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        /*---Editar datos Cuentas Bancarias ---*/

        public BaseResponse<string> EditarDatosCuentaBancarias(EmpresaRegistroRequest model, string nroRuc)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var usuarioEmpresa = context.Tb_MD_Per_Juridica.Where(y => y.vNumDocumento.Equals(nroRuc)).FirstOrDefault();

                        if (usuarioEmpresa != null)
                        {
                            model.cuentas.ForEach(x =>
                            {

                                Tb_MD_Cuentas_Bancarias cuentas = null;
                                cuentas = context.Tb_MD_Cuentas_Bancarias.Find(x.codigo);

                                if (cuentas != null)
                                {
                                    if (x.estado == Convert.ToInt16(EstadoRegistroTabla.Eliminado))
                                    {

                                        cuentas.iEstadoRegistro = EstadoRegistroTabla.Eliminado;
                                    }
                                }

                                if (cuentas == null)
                                {
                                    cuentas = new Tb_MD_Cuentas_Bancarias();
                                    cuentas.vNumDocumento = usuarioEmpresa.vNumDocumento;
                                    cuentas.vCodEntidad = x.banco;
                                    cuentas.vNumCuenta = x.nroCuenta;
                                    cuentas.dFechaCreacion = DateTime.Now;
                                    cuentas.iTipoCuenta = x.tipoCuenta;
                                    cuentas.vMonedaCuenta = x.moneda;

                                    cuentas.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Cuentas_Bancarias.Add(cuentas);
                                }

                            });

                        }
                        else
                        {
                            throw new Exception("No se encontro empresa");
                        }


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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        /* Crear Password Admin*/
        public BaseResponse<RegistroPassWpord_Response2> CrearPasswprdAdm(RegistroCrearPassWord_Request model)
        {
            BaseResponse<RegistroPassWpord_Response2> result = new BaseResponse<RegistroPassWpord_Response2>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Cliente Validacion
                        Guid sid = Guid.NewGuid();
                        DateTime ahora = DateTime.Now;
                        try
                        {
                            sid = Guid.Parse(model.sid);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con el servicio, puede ser que su key de sesión no sea correcta.");
                        }

                        Tb_MD_Mae_Usuarios usuario = null;
                        usuario = context.Tb_MD_Mae_Usuarios.First(x => x.SecretId == sid);

                        if (usuario == null)
                            throw new Exception(ErroresValidacion.ClienteNoExiste);
                        #endregion

                        string clave = Encrypt.EncryptKey(model.password);

                        usuario.vEstadoRegistro = EstadoRegistroTabla.Activo;
                        usuario.vPassword = clave;

                        Tb_MD_Per_Natural persona = context.Tb_MD_Per_Natural.Find(usuario.vNroDocumento);

                        context.SaveChanges();
                        transaction.Commit();
                        result.success = true;
                        //persona.vNumDocumento
                        result.data = new RegistroPassWpord_Response2
                        {
                            email = persona.vMailContacto,
                            IdCliente = 10,
                            IdUsuario = usuario.iIdUsuario,
                            Iniciales = "",
                            NombreCliente = persona.vNombre + " " + persona.vApellido,
                            vNroDocumento = usuario.vNroDocumento,
                            RucEmpresa = usuario.vRucEmpresa,
                            TipoCliente = 1
                        };

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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }


            return result;

        }


        /* Modificar Password Admin*/
        public BaseResponse<string> ModificarPasswprdAdm(ModificarPassWord_Request model, string nombre)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        string clave = Encrypt.EncryptKey(model.passwordActual);

                        Tb_MD_Mae_Usuarios usuario = null;
                        usuario = context.Tb_MD_Mae_Usuarios.First(x => x.vPassword == clave && x.vEmailUsuario == nombre);

                        if (usuario == null)
                            throw new Exception("Contraseña actual Incorrecta");

                        if (model.password.Equals(model.rePassword))
                        {
                            string claveNueva = Encrypt.EncryptKey(model.password);
                            usuario.vPassword = claveNueva;
                        }
                        else
                        {
                            throw new Exception("Contraseña nueva no coinciden");
                        }

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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }


            return result;

        }

        public ExportFiles descargarDocumento(string ruc)
        {
            ExportFiles file = new ExportFiles();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Documentos documento = context.Tb_MD_Documentos.Where(x => x.vNombre == ruc && x.iEstadoRegistro == EstadoRegistroTabla.Activo).FirstOrDefault();

                    file.Name = documento.vNombre;
                    file.Extension = documento.vExtension;
                    file.FileBytes = documento.vArchivo;
                }

            }
            catch (Exception ex)
            {
                //result.success = false;
                //result.ex = ex;
                //result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return file;

        }

        /* Modificar Password Cliente*/
        public BaseResponse<string> ModificarPassw(ModificarPassWord_Request model, string nombre)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        string clave = Encrypt.EncryptKey(model.passwordActual);

                        Tb_MD_ClienteUsuario usuario = null;
                        usuario = context.Tb_MD_ClienteUsuario.First(x => x.Password == clave && x.Email == nombre);

                        if (usuario == null)
                            throw new Exception("Contraseña actual Incorrecta");

                        if (model.password.Equals(model.rePassword))
                        {
                            string claveNueva = Encrypt.EncryptKey(model.password);
                            usuario.Password = claveNueva;
                        }
                        else
                        {
                            throw new Exception("Contraseña nueva no coinciden");
                        }

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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }


            return result;

        }

        /*Datos Fideicomiso*/

        public BaseResponse<FideicomisoResponse> ListarDatosFideicomiso(string idbanco)
        {
            BaseResponse<FideicomisoResponse> valorRegistrados = new BaseResponse<FideicomisoResponse>();

            try
            {
                valorRegistrados.data = new FideicomisoResponse();

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    string banco = "BCP";
                    string moneda = "USD";
                    int tipoCuenta = 0;

                    if (idbanco != null)
                    {
                        int id = Convert.ToInt16(idbanco);
                        Tb_MD_ClientesDatosBancos clienteBanco = null;
                        clienteBanco = context.Tb_MD_ClientesDatosBancos.Find(id);

                        if (clienteBanco != null)
                        {
                            banco = clienteBanco.vBanco;
                            moneda = clienteBanco.vMoneda;
                            tipoCuenta = clienteBanco.Tb_MD_TipoCuentaBancaria.IdTipoCuenta;
                        }

                    }

                    FideicomisoResponse fideicomiso = new FideicomisoResponse();

                    Tb_MD_Per_Juridica datosFideicomiso = null;
                    datosFideicomiso = context.Tb_MD_Per_Juridica.Where(x => x.IsFideicomiso == true).FirstOrDefault();

                    if (datosFideicomiso != null)
                    {
                        fideicomiso.numeroDocumento = datosFideicomiso.vNumDocumento;
                        fideicomiso.nombreEmpresa = datosFideicomiso.vNombreEntidad;

                        var cuentaBancaria = datosFideicomiso.Tb_MD_Cuentas_Bancarias.Where(x => x.vCodEntidad == banco && x.vMonedaCuenta == moneda && x.iTipoCuenta == tipoCuenta).FirstOrDefault();

                        if (cuentaBancaria != null)
                        {
                            fideicomiso.codigobanco = cuentaBancaria.vCodEntidad;
                            fideicomiso.banco = cuentaBancaria.Tb_MD_Entidades_Financieras.vDesEntidad;
                            fideicomiso.codigoMoneda = cuentaBancaria.vMonedaCuenta;
                            fideicomiso.moneda = cuentaBancaria.Tb_MD_TipoMoneda.vDesMoneda;
                            fideicomiso.codigoTipoCuenta = cuentaBancaria.iTipoCuenta.ToString();
                            fideicomiso.tipoCuenta = cuentaBancaria.Tb_MD_TipoCuentaBancaria.Nombre;
                            fideicomiso.numeroCuenta = cuentaBancaria.vNumCuenta;
                            fideicomiso.numeroCuentaInter = cuentaBancaria.vNumCCI;
                        }
                        else
                        {
                            cuentaBancaria = datosFideicomiso.Tb_MD_Cuentas_Bancarias.Where(x => x.vCodEntidad == "BCP" && x.vMonedaCuenta == moneda).FirstOrDefault();
                            if (cuentaBancaria != null)
                            {
                                fideicomiso.codigobanco = cuentaBancaria.vCodEntidad;
                                fideicomiso.banco = cuentaBancaria.Tb_MD_Entidades_Financieras.vDesEntidad;
                                fideicomiso.codigoMoneda = cuentaBancaria.vMonedaCuenta;
                                fideicomiso.moneda = cuentaBancaria.Tb_MD_TipoMoneda.vDesMoneda;
                                fideicomiso.codigoTipoCuenta = cuentaBancaria.iTipoCuenta.ToString();
                                fideicomiso.tipoCuenta = cuentaBancaria.Tb_MD_TipoCuentaBancaria.Nombre;
                                fideicomiso.numeroCuenta = cuentaBancaria.vNumCuenta;
                                fideicomiso.numeroCuentaInter = cuentaBancaria.vNumCCI;
                            }
                            //&& x.iTipoCuenta == tipoCuenta
                        }


                    }

                    valorRegistrados.data = fideicomiso;
                }

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



        /*Lista cuentas Cliente*/

        public BaseResponse<EmpresaRegistroRequest> ListarDatosCuentasBancarias_Clientes(int IdCliente)
        {
            BaseResponse<EmpresaRegistroRequest> valorRegistrados = new BaseResponse<EmpresaRegistroRequest>();
            try
            {

                valorRegistrados.data = new EmpresaRegistroRequest();

                EmpresaRegistroRequest entidad = new EmpresaRegistroRequest();
                List<DatoBancariaRequest> listaCuentaBancarias = new List<DatoBancariaRequest>();


                using (MesaDineroContext context = new MesaDineroContext())
                {

                    var cuentas = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == IdCliente && (x.vEstadoRegistro == EstadoRegistroTabla.Activo || x.vEstadoRegistro == EstadoRegistroTabla.PorVerificar || x.vEstadoRegistro == EstadoRegistroTabla.Observado)).ToList();

                    string host = ConfigurationManager.AppSettings["HostWeb"];
                    cuentas.ForEach(m =>
                    {
                        int tipocuenta = Convert.ToInt16(m.iTipoCuenta);
                        DatoBancariaRequest pa = new DatoBancariaRequest();
                        var entidadFinanciera = context.Tb_MD_Entidades_Financieras.Find(m.vBanco);
                        var moneda = context.Tb_MD_TipoMoneda.Find(m.vMoneda);
                        var TipoCuenta = context.Tb_MD_TipoCuentaBancaria.Find(tipocuenta);
                        string estado = "";
                        if (m.vEstadoRegistro == EstadoRegistroTabla.Activo)
                        {
                            estado = "Activo";
                        }
                        else if (m.vEstadoRegistro == EstadoRegistroTabla.PorVerificar)
                        {
                            estado = "Por Verificar";
                        }
                        else if (m.vEstadoRegistro == EstadoRegistroTabla.Observado)
                        {
                            estado = "Observado";
                        }

                        pa.codigo = m.iIdDatosBank;
                        pa.banco = m.vBanco;
                        pa.moneda = m.vMoneda;
                        pa.monedatext = moneda.vDesMoneda;
                        pa.tipoCuenta = m.iTipoCuenta ?? 0;
                        pa.tipoCuentaText = TipoCuenta.Nombre;
                        pa.nroCuenta = m.vNroCuenta;
                        pa.logo = host + entidadFinanciera.vLogoEntidad;
                        pa.estado = Convert.ToInt16(m.vEstadoRegistro);
                        pa.estadoNombre = estado;

                        listaCuentaBancarias.Add(pa);
                    });


                    entidad.cuentas = listaCuentaBancarias;

                    valorRegistrados.data = entidad;

                }

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


        /*---Editar datos Cuentas Bancarias ---*/
        //16-06-2019
        public BaseResponse<string> EditarDatosCuentaBancariasCliente(EmpresaRegistroRequest model, int IdCliente, int tipocliente)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var usuarioCliente = context.Tb_MD_Clientes.Where(y => y.iIdCliente.Equals(IdCliente)).FirstOrDefault();
                        var usuarioPreCliente = context.Tb_MD_Pre_Clientes.Where(y => y.idPreCliente == usuarioCliente.idPreCliente).FirstOrDefault();

                        int indElimina = 0;
                        int indAgrega = 0;
                        if (usuarioCliente != null)
                        {
                            model.cuentas.ForEach(x =>
                            {

                                Tb_MD_ClientesDatosBancos cuentas = null;
                                cuentas = context.Tb_MD_ClientesDatosBancos.Where(y => y.iIdDatosBank == x.codigo).FirstOrDefault();

                                if (cuentas != null)
                                {
                                    if (x.estado == Convert.ToInt16(EstadoRegistroTabla.Eliminado))
                                    {

                                        cuentas.vEstadoRegistro = EstadoRegistroTabla.Eliminado;
                                        indElimina = 9;
                                    }
                                }

                                if (cuentas == null)
                                {
                                    cuentas = new Tb_MD_ClientesDatosBancos();
                                    cuentas.vNroDocumento = usuarioCliente.vNroDocumento;
                                    cuentas.vBanco = x.banco;
                                    cuentas.vNroCuenta = x.nroCuenta;
                                    cuentas.dFechaCreacion = DateTime.Now;
                                    cuentas.iTipoCuenta = x.tipoCuenta;
                                    cuentas.vMoneda = x.moneda;
                                    cuentas.iIdCliente = IdCliente;
                                    cuentas.vTipoPersona = tipocliente;
                                    cuentas.vEstadoRegistro = EstadoRegistroTabla.PorVerificar;
                                    context.Tb_MD_ClientesDatosBancos.Add(cuentas);
                                    indAgrega = 1;
                                }

                            });

                            if (indElimina == 9 && indAgrega == 0)
                            {
                                result.other = "9";
                            }
                            else {
                                result.other = "1";
                                /*Notificacion*/

                                Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                                notificacion.IdUsuario = "";
                                notificacion.IdCliente = IdCliente;
                                notificacion.Titulo = "Cuenta Bancaria a Verificacion";
                                notificacion.Mensaje = "Usted ha registado una nueva cuenta, la cual procedera a verificacion. En breve se le atendera su solicitud.";
                                notificacion.Tipo = 1;
                                notificacion.Url = "";
                                notificacion.Fecha = DateTime.Now.AddDays(1);
                                notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                context.Tb_MD_Notificacion.Add(notificacion);

                                /*--------*/
                                /*Pre-Clientes*/

                                if (usuarioPreCliente != null)
                                {
                                    model.cuentas.ForEach(x =>
                                    {
                                        Tb_MD_Pre_ClientesDatosBancos cuentas = null;
                                        cuentas = context.Tb_MD_Pre_ClientesDatosBancos.Where(y => y.iIdCliente == usuarioPreCliente.idPreCliente && y.vNroCuenta == x.nroCuenta).FirstOrDefault();

                                        if (cuentas != null)
                                        {
                                            if (x.estado == Convert.ToInt16(EstadoRegistroTabla.Eliminado))
                                            {

                                                cuentas.vEstadoRegistro = EstadoRegistroTabla.Eliminado;
                                            }
                                        }

                                        if (cuentas == null)
                                        {
                                            cuentas = new Tb_MD_Pre_ClientesDatosBancos();
                                            cuentas.vNroDocumento = usuarioCliente.vNroDocumento;
                                            cuentas.vBanco = x.banco;
                                            cuentas.vNroCuenta = x.nroCuenta;
                                            cuentas.dFechaCreacion = DateTime.Now;
                                            cuentas.iTipoCuenta = x.tipoCuenta;
                                            cuentas.vMoneda = x.moneda;
                                            cuentas.iIdCliente = usuarioPreCliente.idPreCliente;
                                            cuentas.vTipoPersona = tipocliente;
                                            cuentas.vEstadoRegistro = EstadoRegistroTabla.PorVerificar;
                                            context.Tb_MD_Pre_ClientesDatosBancos.Add(cuentas);
                                        }

                                    });



                                    Tb_MD_Pre_Clientes pre_cliente = null;
                                    pre_cliente = context.Tb_MD_Pre_Clientes.Find(usuarioPreCliente.idPreCliente);
                                    pre_cliente.vTipoValidacion = "CB"; /* CB: Cuentas Bancarias*/
                                    pre_cliente.EstadoValidacion = "P";
                                    pre_cliente.EstadoValidacion_Fideicomiso = "P";
                                    pre_cliente.envioValidacion = true;
                                    pre_cliente.Finalizado = false;


                                }

                            }
                         


                        }
                        else
                        {
                            throw new Exception("No se encontro empresa");
                        }


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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }


        /* Listar datos Empresa*/

        public BaseResponse<EmpresaRegistroRequest> ListarDatosEmpresaCliente(string NroRuc, string tipo)
        {
            BaseResponse<EmpresaRegistroRequest> valorRegistrados = new BaseResponse<EmpresaRegistroRequest>();

            try
            {

                valorRegistrados.data = new EmpresaRegistroRequest();

                EmpresaRegistroRequest entidad = new EmpresaRegistroRequest();
                PersonaJuridicaReuest datosEmpresa = new PersonaJuridicaReuest();
                PersonaNatutalRequest repreLegal = new PersonaNatutalRequest();
                List<PersonaNatutalRequest> listaPersonaAutorizada = new List<PersonaNatutalRequest>();

                List<PersonaNatutalRequest> listaAccionistas = new List<PersonaNatutalRequest>();
                string tipoEmpresa = tipo;

                using (MesaDineroContext context = new MesaDineroContext())
                {

                    var empresa = context.Tb_MD_Per_Juridica.Where(x => x.vNumDocumento == NroRuc).FirstOrDefault();
                    var persolegal = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica == NroRuc && x.vRol == RolPersonaEmpresa.RepresentanteLegal && x.vEstadoRegsitro == EstadoRegistroTabla.Activo).FirstOrDefault();

                    List<String> persoAcc = context.Tb_MD_Cuentas_Email.Where(x => x.iIdPerjuridica == NroRuc && x.vRol == RolPersonaEmpresa.Accionista).Select(y => y.vNumDocumento).ToList();
                    List<String> persoAuto = context.Tb_MD_Empresa_PersonaAutorizada.Where(x => x.IdEmpresa == NroRuc && x.EstadoRegistro == EstadoRegistroTabla.Activo).Select(y => y.IdPersonaAutorizada).ToList();

                    Tb_MD_Per_Natural legal = null;
                    if (persolegal != null)
                    {
                        legal = context.Tb_MD_Per_Natural.Where(x => x.vNumDocumento == persolegal.vNumDocumento).FirstOrDefault();
                    }

                    var autorizados = context.Tb_MD_Per_Natural.Where(x => persoAuto.Contains(x.vNumDocumento)).ToList();

                    var accionista = context.Tb_MD_Per_Natural.Where(x => persoAcc.Contains(x.vNumDocumento)).ToList();
                    //RolPersonaEmpresa.Autorizado;

                    if (empresa != null)
                    {
                        datosEmpresa.ruc = empresa.vNumDocumento;
                        datosEmpresa.nombre = empresa.vRazonSocial;
                        datosEmpresa.actividadEconomica = empresa.ActividadEconomica;
                        datosEmpresa.origenFondos = empresa.OrigenFondos;
                        datosEmpresa.modoEdicion = 1;
                        var cliente = context.Tb_MD_Clientes.Where(x => x.vNroDocumento == NroRuc).FirstOrDefault();

                        if (cliente != null)
                        {
                            int idcliente = Convert.ToInt16(cliente.idPreCliente);
                            var pre_cliente = context.Tb_MD_Pre_Clientes.Find(idcliente);

                            if (pre_cliente != null)
                            {
                                if (pre_cliente.EstadoValidacion_Fideicomiso == "A")
                                {
                                    datosEmpresa.modoEdicion = 1;
                                }
                                else
                                {
                                    datosEmpresa.modoEdicion = 0;
                                }
                            }

                        }

                        //if (empresa.IsPartner)
                        //{
                        //    tipoEmpresa = "Parnert";
                        //}
                        //if (empresa.IsFideicomiso)
                        //{
                        //    tipoEmpresa = "Fideicomiso";
                        //}
                    }

                    if (legal != null)
                    {
                        repreLegal.tipoDocumento = legal.vTipoDocumento;
                        repreLegal.nroDocumento = legal.vNumDocumento;
                        repreLegal.nombres = legal.vNombre;
                        repreLegal.apePaterno = legal.vApellido;
                        repreLegal.apeMaterno = legal.vApellidoMat;
                        repreLegal.email = legal.vMailContacto;
                    }



                    autorizados.OrderBy(x => x.dFechaCreacion).ToList().ForEach(m =>
                    {
                        PersonaNatutalRequest pa = new PersonaNatutalRequest();
                        pa.tipoDocumento = m.vTipoDocumento;
                        pa.nroDocumento = m.vNumDocumento;
                        pa.nombres = m.vNombre;
                        pa.apePaterno = m.vApellido;
                        pa.apeMaterno = m.vApellidoMat;

                        pa.email = m.vMailContacto;
                        pa.preCelular = m.vPreCelular;
                        pa.celular = m.vTelefonoMovil;
                        pa.cargo = context.Tb_MD_Empresa_PersonaAutorizada.Where(x => x.IdEmpresa == NroRuc && x.IdPersonaAutorizada == m.vNumDocumento).Select(y => y.IdCargo).FirstOrDefault();
                        pa.estado = m.vEstadoRegistro;
                        listaPersonaAutorizada.Add(pa);
                    });

                    accionista.ForEach(m =>
                    {
                        PersonaNatutalRequest acc = new PersonaNatutalRequest();
                        acc.tipoDocumento = m.vTipoDocumento;
                        acc.nroDocumento = m.vNumDocumento;
                        acc.nombres = m.vNombre;
                        acc.apePaterno = m.vApellido;
                        acc.apeMaterno = m.vApellidoMat;
                        acc.email = m.vMailContacto;
                        acc.estado = m.vEstadoRegistro;
                        listaAccionistas.Add(acc);
                    });

                    entidad.accionistas = listaAccionistas;
                    entidad.tipoEmpresa = tipoEmpresa;
                    entidad.empresa = datosEmpresa;
                    entidad.personaAutorizadas = listaPersonaAutorizada;
                    entidad.repreLegal = repreLegal;
                    valorRegistrados.data = entidad;

                    //valorRegistrados.data = context.Database.SqlQuery<VerificacionPagoResponse>("exec Proc_Sel_Verificacion_Pago @PageNumber,@ItemsPerPage,@ModoLista", pageParam, itemsParam, modoLista).ToList<VerificacionPagoResponse>();


                }




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


        /*---Editar datos Empresa Cliente---*/

        public BaseResponse<string> EditarDatosPersonaJuridicaCliente(EmpresaRegistroRequest model, int idcliente, string AbreviaturaCurrenUser)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        /*Modificacion  Tablas Orginales*/

                        Tb_MD_Per_Juridica empresa = null;
                        empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.empresa.ruc));
                        if (empresa == null)
                        {
                            throw new Exception("No se encontro Empresa");
                        }

                        if (empresa != null)
                        {
                            empresa.vNombreEntidad = model.empresa.nombre;
                            empresa.vRazonSocial = model.empresa.nombre;
                            empresa.ActividadEconomica = model.empresa.actividadEconomica;
                            empresa.OrigenFondos = model.empresa.origenFondos;
                            empresa.dFechaModificacion = DateTime.Now;
                            empresa.vEstadoRegsitro = EstadoRegistroTabla.Activo;


                            Tb_MD_Per_Natural repreLegal = null;
                            repreLegal = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.repreLegal.nroDocumento));
                            {
                                if (repreLegal != null)
                                {
                                    repreLegal.dFechaModificacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;

                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";

                                    Tb_MD_Cuentas_Email cuenta = null;
                                    cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vNumDocumento == repreLegal.vNumDocumento && x.vEstadoRegsitro == EstadoRegistroTabla.Activo);


                                    if (cuenta != null)
                                    {

                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = repreLegal.vNumDocumento;
                                        cuenta.vMailContacto = model.repreLegal.email;
                                        cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                        cuenta.vRol = RolPersonaEmpresa.RepresentanteLegal;

                                    }

                                    if (cuenta == null)
                                    {
                                        Tb_MD_Cuentas_Email cuentaAnterior = null;
                                        cuentaAnterior = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vRol == RolPersonaEmpresa.RepresentanteLegal && x.vEstadoRegsitro == EstadoRegistroTabla.Activo);
                                        if (cuentaAnterior != null)
                                        {
                                            cuentaAnterior.vEstadoRegsitro = EstadoRegistroTabla.Eliminado;
                                        }

                                        cuenta = new Tb_MD_Cuentas_Email();
                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = repreLegal.vNumDocumento;
                                        cuenta.vMailContacto = model.repreLegal.email;
                                        cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                        cuenta.vRol = RolPersonaEmpresa.RepresentanteLegal;
                                        context.Tb_MD_Cuentas_Email.Add(cuenta);
                                    }
                                }

                                if (repreLegal == null)
                                {
                                    repreLegal = new Tb_MD_Per_Natural();
                                    repreLegal.dFechaCreacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;

                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Per_Natural.Add(repreLegal);


                                    Tb_MD_Cuentas_Email cuentaAnterior = null;
                                    cuentaAnterior = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vRol == RolPersonaEmpresa.RepresentanteLegal && x.vEstadoRegsitro == EstadoRegistroTabla.Activo);

                                    if (cuentaAnterior != null)
                                    {
                                        cuentaAnterior.vEstadoRegsitro = EstadoRegistroTabla.Eliminado;
                                    }

                                    Tb_MD_Cuentas_Email cuenta = new Tb_MD_Cuentas_Email();
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = repreLegal.vNumDocumento;
                                    cuenta.vMailContacto = model.repreLegal.email;
                                    cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                    cuenta.vRol = RolPersonaEmpresa.RepresentanteLegal;
                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                }
                            }

                            empresa.vRepresentanteLegal = repreLegal.vNumDocumento;

                            foreach (var ac in model.accionistas)
                            {
                                Tb_MD_Per_Natural PerAcc = null;
                                PerAcc = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(ac.nroDocumento));

                                if (PerAcc != null)
                                {
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = ac.estado;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    Tb_MD_Accionistas accionista;
                                    accionista = context.Tb_MD_Accionistas.FirstOrDefault(x => x.IdEmpresa.Equals(model.empresa.ruc) && x.IdPersonaNatural == PerAcc.vNumDocumento);

                                    if (accionista != null)
                                    {

                                        accionista.dFechaCreacion = DateTime.Now;
                                        accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                        accionista.IdEmpresa = empresa.vNumDocumento;
                                        accionista.EstadoRegistro = ac.estado;

                                    }

                                    if (accionista == null)
                                    {
                                        accionista = new Tb_MD_Accionistas();
                                        accionista.dFechaCreacion = DateTime.Now;
                                        accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                        accionista.IdEmpresa = empresa.vNumDocumento;
                                        accionista.EstadoRegistro = ac.estado;
                                        context.Tb_MD_Accionistas.Add(accionista);
                                    }
                                }

                                if (PerAcc == null)
                                {
                                    PerAcc = new Tb_MD_Per_Natural();
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Per_Natural.Add(PerAcc);

                                    Tb_MD_Accionistas accionista = new Tb_MD_Accionistas();
                                    accionista.dFechaCreacion = DateTime.Now;
                                    accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                    accionista.IdEmpresa = empresa.vNumDocumento;
                                    accionista.EstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Accionistas.Add(accionista);
                                }
                                Tb_MD_Cuentas_Email cuenta = null;
                                cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vNumDocumento == PerAcc.vNumDocumento);

                                if (cuenta != null)
                                {
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = PerAcc.vNumDocumento;
                                    cuenta.vMailContacto = ac.email;
                                    cuenta.vEstadoRegsitro = ac.estado;
                                    cuenta.vRol = RolPersonaEmpresa.Accionista;

                                }

                                if (cuenta == null)
                                {
                                    cuenta = new Tb_MD_Cuentas_Email();
                                    cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                    cuenta.vNumDocumento = PerAcc.vNumDocumento;
                                    cuenta.vMailContacto = ac.email;
                                    cuenta.vEstadoRegsitro = EstadoRegistroTabla.Activo;
                                    cuenta.vRol = RolPersonaEmpresa.Accionista;
                                    context.Tb_MD_Cuentas_Email.Add(cuenta);
                                }
                            }

                            /*Persona Autorizada*/

                            foreach (var pa in model.personaAutorizadas)
                            {
                                Tb_MD_Per_Natural perAutorizada = null;
                                perAutorizada = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(pa.nroDocumento));
                                {

                                    if (perAutorizada != null)
                                    {
                                        perAutorizada.dFechaModificacion = DateTime.Now;
                                        perAutorizada.vTipoDocumento = pa.tipoDocumento;
                                        perAutorizada.vNumDocumento = pa.nroDocumento;
                                        perAutorizada.vNombre = pa.nombres;
                                        perAutorizada.vApellido = pa.apePaterno;
                                        perAutorizada.vApellidoMat = pa.apeMaterno;
                                        perAutorizada.vPreCelular = pa.preCelular;
                                        perAutorizada.vTelefonoMovil = pa.celular;
                                        perAutorizada.vMailContacto = pa.email;
                                        perAutorizada.vEstadoRegistro = pa.estado;
                                    }

                                    if (perAutorizada == null)
                                    {
                                        perAutorizada = new Tb_MD_Per_Natural();
                                        perAutorizada.dFechaCreacion = DateTime.Now;
                                        perAutorizada.vTipoDocumento = pa.tipoDocumento;
                                        perAutorizada.vNumDocumento = pa.nroDocumento;
                                        perAutorizada.vNombre = pa.nombres;
                                        perAutorizada.vApellido = pa.apePaterno;
                                        perAutorizada.vApellidoMat = pa.apeMaterno;
                                        perAutorizada.vPreCelular = pa.preCelular;
                                        perAutorizada.vTelefonoMovil = pa.celular;
                                        perAutorizada.vMailContacto = pa.email;
                                        perAutorizada.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Per_Natural.Add(perAutorizada);

                                    }


                                    Tb_MD_Empresa_PersonaAutorizada empreaPerAuth = new Tb_MD_Empresa_PersonaAutorizada();

                                    empreaPerAuth = context.Tb_MD_Empresa_PersonaAutorizada.FirstOrDefault(x => x.IdEmpresa.Equals(model.empresa.ruc) && x.IdPersonaAutorizada == perAutorizada.vNumDocumento);

                                    if (empreaPerAuth != null)
                                    {
                                        empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                        empreaPerAuth.IdEmpresa = empresa.vNumDocumento;
                                        empreaPerAuth.IdCargo = pa.cargo;
                                        empreaPerAuth.SecredId = Guid.NewGuid();
                                        empreaPerAuth.EstadoRegistro = Convert.ToByte(pa.estado);
                                        empreaPerAuth.FechaModificacion = DateTime.Now;

                                    }

                                    if (empreaPerAuth == null)
                                    {
                                        empreaPerAuth = new Tb_MD_Empresa_PersonaAutorizada();
                                        empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                        empreaPerAuth.IdEmpresa = empresa.vNumDocumento;
                                        empreaPerAuth.IdCargo = pa.cargo;
                                        empreaPerAuth.SecredId = Guid.NewGuid();
                                        empreaPerAuth.EstadoRegistro = EstadoRegistroTabla.Activo;
                                        empreaPerAuth.FechaCreacion = DateTime.Now;
                                        context.Tb_MD_Empresa_PersonaAutorizada.Add(empreaPerAuth);

                                        Tb_MD_ClienteUsuario usuarioAutorizado = null;
                                        usuarioAutorizado = context.Tb_MD_ClienteUsuario.FirstOrDefault(y => y.Email == pa.email);

                                        if (usuarioAutorizado != null)
                                        {
                                            throw new Exception("Ya tienes una cuenta con este Correo " + pa.email + ". Ingrese otro.");
                                        }
                                        else
                                        {
                                            usuarioAutorizado = new Tb_MD_ClienteUsuario();
                                            usuarioAutorizado.Email = pa.email;
                                            usuarioAutorizado.Creado = DateTime.Now;
                                            usuarioAutorizado.IdCliente = idcliente;
                                            usuarioAutorizado.TipoCliente = TipoCliente.PersonaJuridica;
                                            usuarioAutorizado.vNroDocumento = model.empresa.ruc;
                                            usuarioAutorizado.NombreCliente = model.empresa.nombre;
                                            usuarioAutorizado.SecredId = empreaPerAuth.SecredId;
                                            usuarioAutorizado.Iniciales = AbreviaturaCurrenUser;
                                            usuarioAutorizado.EstadoREgistro = EstadoRegistroTabla.PorVerificar;
                                            context.Tb_MD_ClienteUsuario.Add(usuarioAutorizado);

                                        }

                                    }


                                    Tb_MD_Cuentas_Email cuenta = null;
                                    cuenta = context.Tb_MD_Cuentas_Email.FirstOrDefault(x => x.iIdPerjuridica.Equals(model.empresa.ruc) && x.vNumDocumento == perAutorizada.vNumDocumento);

                                    if (cuenta != null)
                                    {
                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = perAutorizada.vNumDocumento;
                                        cuenta.vMailContacto = pa.email;
                                        cuenta.vEstadoRegsitro = pa.estado;
                                        cuenta.vRol = RolPersonaEmpresa.Autorizado;

                                    }

                                    if (cuenta == null)
                                    {
                                        cuenta = new Tb_MD_Cuentas_Email();
                                        cuenta.iIdPerjuridica = empresa.vNumDocumento;
                                        cuenta.vNumDocumento = perAutorizada.vNumDocumento;
                                        cuenta.vMailContacto = pa.email;
                                        cuenta.vEstadoRegsitro = EstadoRegistroTabla.NoActivo;
                                        cuenta.vRol = RolPersonaEmpresa.Autorizado;
                                        context.Tb_MD_Cuentas_Email.Add(cuenta);
                                    }

                                }

                            }


                            /*Notificacion*/

                            Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                            notificacion.IdUsuario = "";
                            notificacion.IdCliente = idcliente;
                            notificacion.Titulo = "Datos a Verificacion";
                            notificacion.Mensaje = "Usted ha enviado sus datos a verificacion. En breve se le atendera su solicitud.";
                            notificacion.Tipo = 1;
                            notificacion.Url = "";
                            notificacion.Fecha = DateTime.Now.AddMinutes(5);
                            notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                            context.Tb_MD_Notificacion.Add(notificacion);

                            /*--------*/

                        }

                        /*-------------------------------------------------------------------*/

                        /*Modificacion Precliente envio Evaluacion */



                        /*Modificacion  Tablas Copia*/

                        Tb_MD_Clientes cli = null;
                        cli = context.Tb_MD_Clientes.Find(idcliente);



                        Tb_MD_Pre_Clientes pre_cliente = null;
                        pre_cliente = context.Tb_MD_Pre_Clientes.Find(cli.idPreCliente);
                        pre_cliente.vTipoValidacion = "C"; /* C: Cambios*/
                        pre_cliente.EstadoValidacion = "P";
                        pre_cliente.EstadoValidacion_Fideicomiso = "P";
                        pre_cliente.envioValidacion = true;
                        pre_cliente.Seguimiento = SeguimientoRegistro.PostCrearPasswords;
                        pre_cliente.Finalizado = false;

                        Tb_MD_Pre_Per_Juridica pre_empresa = null;
                        pre_empresa = context.Tb_MD_Pre_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(model.empresa.ruc));
                        if (pre_empresa == null)
                        {
                            throw new Exception("No se encontro Empresa");
                        }

                        if (pre_empresa != null)
                        {
                            pre_empresa.vNombreEntidad = model.empresa.nombre;
                            pre_empresa.vRazonSocial = model.empresa.nombre;
                            pre_empresa.ActividadEconomica = model.empresa.actividadEconomica;
                            pre_empresa.OrigenFondos = model.empresa.origenFondos;
                            pre_empresa.dFechaModificacion = DateTime.Now;
                            pre_empresa.vEstadoRegsitro = EstadoRegistroTabla.Activo;


                            Tb_MD_Pre_Per_Natural repreLegal = null;
                            repreLegal = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(model.repreLegal.nroDocumento));
                            {
                                if (repreLegal != null)
                                {
                                    repreLegal.dFechaModificacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;

                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";


                                }

                                if (repreLegal == null)
                                {
                                    repreLegal = new Tb_MD_Pre_Per_Natural();
                                    repreLegal.dFechaCreacion = DateTime.Now;
                                    repreLegal.vTipoDocumento = model.repreLegal.tipoDocumento;
                                    repreLegal.vNumDocumento = model.repreLegal.nroDocumento;
                                    repreLegal.vNombre = model.repreLegal.nombres;
                                    repreLegal.vApellido = model.repreLegal.apePaterno;
                                    repreLegal.vApellidoMat = model.repreLegal.apeMaterno;

                                    repreLegal.vMailContacto = model.repreLegal.email;
                                    repreLegal.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    repreLegal.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Pre_Per_Natural.Add(repreLegal);


                                }
                            }

                            empresa.vRepresentanteLegal = repreLegal.vNumDocumento;

                            foreach (var ac in model.accionistas)
                            {
                                Tb_MD_Pre_Per_Natural PerAcc = null;
                                PerAcc = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(ac.nroDocumento));

                                if (PerAcc != null)
                                {
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = ac.estado;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    Tb_MD_Pre_Accionistas accionista;
                                    accionista = context.Tb_MD_Pre_Accionistas.FirstOrDefault(x => x.IdEmpresa.Equals(model.empresa.ruc) && x.IdPersonaNatural == PerAcc.vNumDocumento);

                                    if (accionista != null)
                                    {

                                        accionista.dFechaCreacion = DateTime.Now;
                                        accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                        accionista.IdEmpresa = pre_empresa.vNumDocumento;
                                        accionista.vTipoDocumento = ac.tipoDocumento;
                                        accionista.IdPersonaNatural = ac.nroDocumento;
                                        accionista.vNombre = ac.nombres;
                                        accionista.vApellido = ac.apePaterno;
                                        accionista.vApellidoMat = ac.apeMaterno;
                                        accionista.IdPreCliente = Convert.ToInt16(cli.idPreCliente);
                                        accionista.vMailContacto = ac.email;

                                        accionista.EstadoRegistro = ac.estado;

                                    }

                                    if (accionista == null)
                                    {
                                        accionista = new Tb_MD_Pre_Accionistas();
                                        accionista.dFechaCreacion = DateTime.Now;
                                        accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                        accionista.IdEmpresa = pre_empresa.vNumDocumento;
                                        accionista.vTipoDocumento = ac.tipoDocumento;
                                        accionista.IdPersonaNatural = ac.nroDocumento;
                                        accionista.vNombre = ac.nombres;
                                        accionista.vApellido = ac.apePaterno;
                                        accionista.vApellidoMat = ac.apeMaterno;
                                        accionista.IdPreCliente = Convert.ToInt16(cli.idPreCliente);
                                        accionista.vMailContacto = ac.email;
                                        accionista.EstadoRegistro = ac.estado;
                                        context.Tb_MD_Pre_Accionistas.Add(accionista);
                                    }
                                }

                                if (PerAcc == null)
                                {
                                    PerAcc = new Tb_MD_Pre_Per_Natural();
                                    PerAcc.dFechaCreacion = DateTime.Now;
                                    PerAcc.vTipoDocumento = ac.tipoDocumento;
                                    PerAcc.vNumDocumento = ac.nroDocumento;
                                    PerAcc.vNombre = ac.nombres;
                                    PerAcc.vApellido = ac.apePaterno;
                                    PerAcc.vApellidoMat = ac.apeMaterno;
                                    PerAcc.vMailContacto = ac.email;
                                    PerAcc.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                    PerAcc.vFlgExpuestoPoliticamente = "N";

                                    context.Tb_MD_Pre_Per_Natural.Add(PerAcc);

                                    Tb_MD_Pre_Accionistas accionista = new Tb_MD_Pre_Accionistas();
                                    accionista.dFechaCreacion = DateTime.Now;
                                    accionista.IdPersonaNatural = PerAcc.vNumDocumento;
                                    accionista.IdEmpresa = empresa.vNumDocumento;
                                    accionista.vTipoDocumento = ac.tipoDocumento;
                                    accionista.IdPersonaNatural = ac.nroDocumento;
                                    accionista.vNombre = ac.nombres;
                                    accionista.vApellido = ac.apePaterno;
                                    accionista.vApellidoMat = ac.apeMaterno;
                                    accionista.IdPreCliente = Convert.ToInt16(cli.idPreCliente);
                                    accionista.vMailContacto = ac.email;
                                    accionista.EstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Pre_Accionistas.Add(accionista);
                                }

                            }

                            /*Persona Autorizada*/

                            foreach (var pa in model.personaAutorizadas)
                            {
                                Tb_MD_Pre_Per_Natural perAutorizada = null;
                                perAutorizada = context.Tb_MD_Pre_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(pa.nroDocumento));
                                {

                                    if (perAutorizada != null)
                                    {
                                        perAutorizada.dFechaModificacion = DateTime.Now;
                                        perAutorizada.vTipoDocumento = pa.tipoDocumento;
                                        perAutorizada.vNumDocumento = pa.nroDocumento;
                                        perAutorizada.vNombre = pa.nombres;
                                        perAutorizada.vApellido = pa.apePaterno;
                                        perAutorizada.vApellidoMat = pa.apeMaterno;
                                        perAutorizada.vPreCelular = pa.preCelular;
                                        perAutorizada.vTelefonoMovil = pa.celular;
                                        perAutorizada.Id_Pre_Cliente = Convert.ToInt16(cli.idPreCliente);
                                        perAutorizada.vMailContacto = pa.email;
                                        perAutorizada.vEstadoRegistro = pa.estado;
                                    }

                                    if (perAutorizada == null)
                                    {
                                        perAutorizada = new Tb_MD_Pre_Per_Natural();
                                        perAutorizada.dFechaCreacion = DateTime.Now;
                                        perAutorizada.vTipoDocumento = pa.tipoDocumento;
                                        perAutorizada.vNumDocumento = pa.nroDocumento;
                                        perAutorizada.vNombre = pa.nombres;
                                        perAutorizada.vApellido = pa.apePaterno;
                                        perAutorizada.vApellidoMat = pa.apeMaterno;
                                        perAutorizada.vPreCelular = pa.preCelular;
                                        perAutorizada.vTelefonoMovil = pa.celular;
                                        perAutorizada.vMailContacto = pa.email;
                                        perAutorizada.Id_Pre_Cliente = Convert.ToInt16(cli.idPreCliente);
                                        perAutorizada.vEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Pre_Per_Natural.Add(perAutorizada);
                                    }


                                    Tb_MD_Pre_Empresa_PersonaAutorizada empreaPerAuth = new Tb_MD_Pre_Empresa_PersonaAutorizada();

                                    empreaPerAuth = context.Tb_MD_Pre_Empresa_PersonaAutorizada.FirstOrDefault(x => x.IdEmpresa.Equals(model.empresa.ruc) && x.IdPersonaAutorizada == perAutorizada.vNumDocumento);

                                    if (empreaPerAuth != null)
                                    {
                                        empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                        empreaPerAuth.IdEmpresa = pre_empresa.vNumDocumento;
                                        empreaPerAuth.IdCargo = pa.cargo;
                                        empreaPerAuth.vNombre = pa.nombres;
                                        empreaPerAuth.vApellido = pa.apePaterno;
                                        empreaPerAuth.vApellidoMat = pa.apeMaterno;
                                        empreaPerAuth.vPreCelular = pa.preCelular;
                                        empreaPerAuth.vTelefonoMovil = pa.celular;
                                        empreaPerAuth.vMailContacto = pa.email;
                                        empreaPerAuth.IdPreCliente = Convert.ToInt16(cli.idPreCliente);
                                        empreaPerAuth.EstadoRegistro = Convert.ToByte(pa.estado);
                                        empreaPerAuth.FechaModificacion = DateTime.Now;

                                    }

                                    if (empreaPerAuth == null)
                                    {
                                        empreaPerAuth = new Tb_MD_Pre_Empresa_PersonaAutorizada();
                                        empreaPerAuth.IdPersonaAutorizada = perAutorizada.vNumDocumento;
                                        empreaPerAuth.IdEmpresa = pre_empresa.vNumDocumento;
                                        empreaPerAuth.IdCargo = pa.cargo;
                                        empreaPerAuth.vNombre = pa.nombres;
                                        empreaPerAuth.vApellido = pa.apePaterno;
                                        empreaPerAuth.vApellidoMat = pa.apeMaterno;
                                        empreaPerAuth.vPreCelular = pa.preCelular;
                                        empreaPerAuth.vTelefonoMovil = pa.celular;
                                        empreaPerAuth.vMailContacto = pa.email;
                                        empreaPerAuth.IdPreCliente = Convert.ToInt16(cli.idPreCliente);
                                        empreaPerAuth.EstadoRegistro = EstadoRegistroTabla.Activo;
                                        empreaPerAuth.FechaCreacion = DateTime.Now;
                                        context.Tb_MD_Pre_Empresa_PersonaAutorizada.Add(empreaPerAuth);


                                    }

                                }

                            }
                        }

                        /*-------------------------------------------------------------------*/


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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }


        /*Datos Fideicomiso*/

        public BaseResponse<bool> VerificarCliente(int idcliente)
        {
            BaseResponse<bool> valorRegistrados = new BaseResponse<bool>();


            valorRegistrados.data = true;
            using (MesaDineroContext context = new MesaDineroContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        Tb_MD_Clientes cliente = null;
                        cliente = context.Tb_MD_Clientes.Where(m => m.iIdCliente == idcliente).FirstOrDefault();
                        if (cliente != null)
                        {
                            if (cliente.bFlagActivo == false)
                            {
                                valorRegistrados.data = false;
                            }
                            else
                            {
                                List<Tb_MD_Subasta> subasta = context.Tb_MD_Subasta.Where(x => x.IdCliente == cliente.iIdCliente &&
                                    x.vEstadoSubasta == EstadoSubasta.Confirmada).ToList();
                                bool encontro = false;

                                subasta.ForEach(x =>
                                {
                                    int tiempo = (int)((x.dFechaConfirmacionOperacion.Value.AddSeconds(x.nTiempoConfitmacionPago ?? 0)) - DateTime.Now).TotalSeconds;
                                    if (tiempo < 0)
                                    {
                                        encontro = true;
                                        x.vEstadoSubasta = "Y";
                                        x.vEstadoRegistro = EstadoRegistroTabla.NoActivo.ToString();
                                    }
                                });

                                if (encontro == true)
                                {
                                    //Tb_MD_Clientes cliente = context.Tb_MD_Clientes.Where(m => m.iIdCliente == subasta.IdCliente).FirstOrDefault();
                                    cliente.bFlagActivo = false;
                                    Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                                    notificacion.IdUsuario = "";
                                    notificacion.IdCliente = cliente.iIdCliente;
                                    notificacion.Titulo = "Subasta Incumplida";
                                    notificacion.Mensaje = "Usted confirmo la subasta con su contraseña, sin embargo no confirmo su pago con su numero de voucher. Comuniquese con Mesa de Ayuda";
                                    notificacion.Tipo = 0;
                                    notificacion.Url = "";
                                    notificacion.Fecha = DateTime.Now.AddDays(1);
                                    notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                    context.Tb_MD_Notificacion.Add(notificacion);
                                    valorRegistrados.data = false;

                                }
                            }
                        }
                        valorRegistrados.success = true;
                        context.SaveChanges();
                        transaction.Commit();


                    }
                    catch (Exception ex)
                    {
                        valorRegistrados.success = false;
                        valorRegistrados.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }

            }

            return valorRegistrados;
        }


        public BaseResponse<List<ListaNotificacionesResponse>> ListaNotificaciones(int idcliente)
        {
            BaseResponse<List<ListaNotificacionesResponse>> valorRegistrados = new BaseResponse<List<ListaNotificacionesResponse>>();


            valorRegistrados.data = new List<ListaNotificacionesResponse>();
            using (MesaDineroContext context = new MesaDineroContext())
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string ruta = "";
                        List<ListaNotificacionesResponse> listaNot = new List<ListaNotificacionesResponse>();
                        ListaNotificacionesResponse notificacion = null;

                        List<Tb_MD_Notificacion> notificacionbd = new List<Tb_MD_Notificacion>();
                        notificacionbd = context.Tb_MD_Notificacion.Where(m => m.IdCliente == idcliente && DateTime.Now <= m.Fecha && m.iEstadoRegistro == EstadoRegistroTabla.Activo).ToList();

                        notificacionbd.ForEach(x =>
                        {
                            notificacion = new ListaNotificacionesResponse();
                            notificacion.IdNotificacion = x.IdNotificacion;
                            notificacion.IdCliente = x.IdCliente;
                            notificacion.Mensaje = x.Mensaje;
                            notificacion.Titulo = x.Titulo;
                            notificacion.Tipo = x.Tipo;
                            if (x.Url != null)
                            {
                                if (x.Url.Trim().Length == 0)
                                {
                                    ruta = "";
                                }
                                else
                                {
                                    ruta = ConfigurationManager.AppSettings["HostWeb"].ToString() + x.Url;
                                }
                            }

                            notificacion.Url = ruta;
                            notificacion.Fecha = x.Fecha;
                            listaNot.Add(notificacion);

                        });

                        valorRegistrados.data = listaNot;
                        //if (notificacion != null)
                        //{

                        //}
                        valorRegistrados.success = true;
                        context.SaveChanges();
                        transaction.Commit();


                    }
                    catch (Exception ex)
                    {
                        valorRegistrados.success = false;
                        valorRegistrados.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }

            }

            return valorRegistrados;
        }
    }
}

