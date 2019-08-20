using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MesaDinero.Domain.DataAccess
{
    public partial class CommonDataAccess
    {

        public BaseResponse<List<ComboListItemString>> getAllTipoMoneda()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            result.data = new List<ComboListItemString>();

            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_TipoMoneda.Where(x=>x.iEstadoRegistro==EstadoRegistroTabla.Activo).Select(x => new ComboListItemString
                    {
                        value = x.vCodMoneda,
                        text = x.vDesMoneda + " (" + x.vCodMoneda + ")"
                    }).ToList();
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

        public BaseResponse<List<EntidadFinancieraResponse>> getEntidadesFinancieras()
        {
            BaseResponse<List<EntidadFinancieraResponse>> result = new BaseResponse<List<EntidadFinancieraResponse>>();
            result.data = new List<EntidadFinancieraResponse>();

            string host = ConfigurationManager.AppSettings["HostAdmin"];


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Entidades_Financieras.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo && x.VTipo=="B").Select(x => new EntidadFinancieraResponse
                    {
                        codigo = x.vCodEntidad,
                        nombre = x.vDesEntidad,
                        icon = host + x.vLogoEntidad,
                        formatoNroCuenta = x.vFormatoCuentaBancaria,
                        formatoCCI = x.vFormatoCCI
                    }).ToList();
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

        public int getTiempoCofirmacionMsM()
        {
            int? result = 0;

            using (MesaDineroContext context = new MesaDineroContext())
            {
                Tb_MD_Tiempos tiempo = null;
                tiempo = context.Tb_MD_Tiempos.FirstOrDefault(x => x.vCodTransaccion.Equals(Tiempos.ConfirmacionSms));

                result = tiempo == null ? 0 : tiempo.nTiempoStandar;
            }

            return result.Value;
        }

        public BaseResponse<List<ComboListItem>> getAllTipoCuentaBanacaria()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_TipoCuentaBancaria.Where(x => x.EstadoRegistro == EstadoRegistroTabla.Activo).OrderBy(x => x.Nombre).Select(x => new ComboListItem
                    {
                        value = x.IdTipoCuenta,
                        text = x.Nombre
                    }).ToList();
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                //result.ex = ex;
                result.success = false;
                result.other = ex.StackTrace.ToString();
            }

            return result;
        }

        public BaseResponse<List<ComboListItemString>> getAllPaises()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            result.data = new List<ComboListItemString>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Pais.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new ComboListItemString
                    {
                        value = x.IdPais,
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

        public BaseResponse<List<ComboListItem>> getDepartamentoForPais(string pais)
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Departamento.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo).Where(x => x.idPais.Equals(pais)).Select(x => new ComboListItem
                    {
                        value = x.IdDepartamento,
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

        public BaseResponse<List<ComboListItem>> getProvinciaForDepartamento(string pais, int dep)
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Ubigeo.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo).Where(x => x.CodPais.Equals(pais) && x.CodDepartamento == dep).Select(x => new ComboListItem
                    {
                        value = x.Tb_MD_Provincia.IdProvincia,
                        text = x.Tb_MD_Provincia.Nombre
                    }).Distinct().ToList();
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

        public BaseResponse<List<ComboListItem>> getDistritoForProvincia(string pais, int dep, int prov)
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Ubigeo.Where(x => x.CodPais.Equals(pais) && x.CodDepartamento == dep && x.CodProvincia == prov).Select(x => new ComboListItem
                    {
                        value = x.Tb_MD_Distrito.IdDistrito,
                        text = x.Tb_MD_Distrito.Nombre
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

        public BaseResponse<List<ComboListItem>> getAllOrigenFondos()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_OrigenFondo.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo).OrderBy(x => x.Descripcion).Select(x => new ComboListItem
                    {
                        value = x.IdOrigenFondos,
                        text = x.Descripcion
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

        public BaseResponse<List<ComboListItem>> getAllActividadEconomica()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_ActividadEconomica.Where(x=>x.iEstadoRegistro==EstadoRegistroTabla.Activo).OrderBy(x => x.Nombre).Select(x => new ComboListItem
                    {
                        value = x.IdActividadEconomica,
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

        public BaseResponse<List<ComboListItemString>> getAllTipoDocumentoPersona()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            result.data = new List<ComboListItemString>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_TipoDocumento.Where(x => x.Tipo.Equals("P") && x.EstadoRegistro==EstadoRegistroTabla.Activo).OrderBy(x => x.Nombre).Select(x => new ComboListItemString
                    {
                        value = x.IdTipoDocumento,
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

        public Tb_MD_Pre_Clientes getPreClienteBySecredId(Guid secredId)
        {
            Tb_MD_Pre_Clientes entity = null;
            using (MesaDineroContext context = new MesaDineroContext())
            {
                entity = context.Tb_MD_Pre_Clientes.FirstOrDefault(x => x.SecretId.Equals(secredId));
            }
            return entity;
        }

        public Tb_MD_Pre_Clientes getUsuarioClienteBySecredId(Guid secredId)
        {
            Tb_MD_Pre_Clientes entity = null;
            using (MesaDineroContext context = new MesaDineroContext())
            {
                int idcliente = 0;
                var cliente=context.Tb_MD_ClienteUsuario.Where(y => y.SecredId==secredId).FirstOrDefault();

                if (cliente != null) {
                    idcliente = Convert.ToInt16( cliente.Tb_MD_Clientes.idPreCliente);
                    entity = context.Tb_MD_Pre_Clientes.Where(x => x.idPreCliente == idcliente).FirstOrDefault();
                }
                
            }
            return entity;
        }

        public Tb_MD_ClienteUsuario getUsuarioAutorizadoBySecredId(Guid secredId)
        {
            Tb_MD_ClienteUsuario entity = null;
            using (MesaDineroContext context = new MesaDineroContext())
            {
                entity = context.Tb_MD_ClienteUsuario.Where(y => y.SecredId==secredId).FirstOrDefault();
            }
            return entity;
        }

        public BaseResponse<List<ComboListItem>> getAllCargos()
        {
            BaseResponse<List<ComboListItem>> result = new BaseResponse<List<ComboListItem>>();
            result.data = new List<ComboListItem>();


            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Cargo.Where(x => x.iEstadoRegistro == EstadoRegistroTabla.Activo).OrderBy(x => x.Nombre).Select(x => new ComboListItem
                    {
                        value = x.IdCargo,
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

        public BaseResponse<string> compararPassword(string clave, int user)
        {

            BaseResponse<string> result = new BaseResponse<string>();

            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Mae_Usuarios usuario = context.Tb_MD_Mae_Usuarios.FirstOrDefault(x => x.iIdUsuario == user);

                    if (!usuario.vPassword.Equals(clave))
                        throw new Exception("El password es incorrecto.");
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

        /*Nuevos Filtros */
        public BaseResponse<List<ComboListItemString>> getAllEntidadBancaria()
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            result.data = new List<ComboListItemString>();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Tb_MD_Entidades_Financieras.Where(p => p.iEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new ComboListItemString
                    {
                        value = x.vCodEntidad,
                        text = x.vDesEntidad
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
        /*--------------------------*/

        public BaseResponse<List<ComboListItemString>> getAllComboEmpresas(string filtro)
        {
            BaseResponse<List<ComboListItemString>> result = new BaseResponse<List<ComboListItemString>>();
            result.data = new List<ComboListItemString>();

            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    if (filtro == "")
                    {
                        result.data = context.Tb_MD_Per_Juridica.Select(x => new ComboListItemString
                        {
                            value = x.vNumDocumento,
                            text = x.vRazonSocial
                        }).ToList();
                    }

                    if (filtro.ToUpper() == TipoUsuario.Lmd)
                    {
                        result.data = context.Tb_MD_Per_Juridica.Where(x => x.IsLmd == true).Select(x => new ComboListItemString
                        {
                            value = x.vNumDocumento,
                            text = x.vRazonSocial
                        }).ToList();
                    }

                    if (filtro.ToUpper() == TipoUsuario.Partner)
                    {
                        result.data = context.Tb_MD_Per_Juridica.Where(x => x.IsPartner == true).Select(x => new ComboListItemString
                        {
                            value = x.vNumDocumento,
                            text = x.vRazonSocial
                        }).ToList();
                    }

                    if (filtro.ToUpper() == TipoUsuario.Fideicomiso)
                    {
                        result.data = context.Tb_MD_Per_Juridica.Where(x => x.IsFideicomiso == true).Select(x => new ComboListItemString
                        {
                            value = x.vNumDocumento,
                            text = x.vRazonSocial
                        }).ToList();
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

        public async Task<BaseResponse<PersonaNatutalRequest>> getDatosPersonaSimple(string nroDocumento)
        {
            BaseResponse<PersonaNatutalRequest> result = new BaseResponse<PersonaNatutalRequest>();
            result.data = new PersonaNatutalRequest();
            Tb_MD_Per_Natural persona = null;
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {

                    persona = context.Tb_MD_Per_Natural.FirstOrDefault(x => x.vNumDocumento.Equals(nroDocumento));
                }

                if (persona != null)
                {
                    result.data.tipoDocumento = persona.vTipoDocumento;
                    result.data.nombres = persona.vNombre;
                    result.data.apePaterno = persona.vApellido;
                    result.data.apeMaterno = persona.vApellidoMat;
                    result.data.email = persona.vMailContacto;
                    result.data.preCelular = persona.vPreCelular;
                    result.data.celular = persona.vTelefonoMovil;
                }
                else
                {
                    result.data = await QuertiumServices.GetDatosPersonaNatural(nroDocumento);
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

        public async Task<BaseResponse<PersonaJuridicaReuest>> getDatosPersonaJuridicaSimple(string ruc)
        {

            BaseResponse<PersonaJuridicaReuest> result = new BaseResponse<PersonaJuridicaReuest>();
            result.data = new PersonaJuridicaReuest();
            Tb_MD_Per_Juridica empresa = null;

            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {

                    empresa = context.Tb_MD_Per_Juridica.FirstOrDefault(x => x.vNumDocumento.Equals(ruc));
                }

                if (empresa != null)
                {
                    result.data.nombre = empresa.vNombreEntidad;
                    result.data.direccion = empresa.vDireccion;
                }
                else
                {
                    result.data = await QuertiumServices.getDatosPersonaJuridica(ruc);
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw;
            }

            return result;
        }

        public Tb_MD_Mae_Usuarios getClienteAdmBySecredId(Guid secredId)
        {
            Tb_MD_Mae_Usuarios entity = null;
            using (MesaDineroContext context = new MesaDineroContext())
            {
                entity = context.Tb_MD_Mae_Usuarios.FirstOrDefault(x => x.SecretId == secredId);
            }
            return entity;
        }


    }
}
