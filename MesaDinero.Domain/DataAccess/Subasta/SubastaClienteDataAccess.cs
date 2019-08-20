using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.Model.operaciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess
{
    public partial class SubastaClienteDataAccess
    {

        public BaseResponse<SubastaProceso_Response> selecconionarPartnert(string subasta_, int partner)
        {
            BaseResponse<SubastaProceso_Response> result = new BaseResponse<SubastaProceso_Response>();
            result.data = new SubastaProceso_Response();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        Tb_MD_Subasta_Detalle detalle;
                        detalle = context.Tb_MD_Subasta_Detalle.FirstOrDefault(x => x.iIdSubastaDEtalle == partner);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        subasta.TipoCambioGanador = partner;

                        context.SaveChanges();
                        transaction.Commit();


                        if (subasta.vTipoOperacion == "C")
                        {
                            result.data.valorRecibe = subasta.nMontiRecibeCliente ?? 0M;
                            result.data.valorEnvio = (subasta.nMontiRecibeCliente ?? 0M) * (detalle.TipoCambio ?? 0);
                            result.data.monto = (subasta.nMontiRecibeCliente ?? 0M);
                        }

                        else
                        {
                            result.data.valorEnvio = subasta.nMontoEnviaCliente ?? 0;
                            result.data.valorRecibe = (subasta.nMontoEnviaCliente ?? 0M) * (detalle.TipoCambio ?? 0);
                            result.data.monto = (subasta.nMontoEnviaCliente ?? 0M);
                        }

                        result.data.tipoCambio = detalle.TipoCambio ?? 0;
                        result.data.partner = detalle.RazonSocial;

                        result.success = true;
                    }
                    catch (Exception ex)
                    {
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        result.success = false;
                        transaction.Rollback();
                    }
                }
            }

            return result;
        }

        public BaseResponse<SubastaCliente_PartnerPuja> getPartnerTipoCambioActualizado(string subasta_)
        {
            BaseResponse<SubastaCliente_PartnerPuja> result = new BaseResponse<SubastaCliente_PartnerPuja>();
            result.data = new SubastaCliente_PartnerPuja();
            result.data.puja = new List<Subasta_PartnerSubastaPuja>();
            result.data.seleccion = new SubastaProceso_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                Guid secredId = Guid.NewGuid();

                try
                {
                    secredId = Guid.Parse(subasta_);
                }
                catch (Exception)
                {
                    throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                }

                Tb_MD_Subasta subasta = context.Tb_MD_Subasta.FirstOrDefault(X => X.SecredId == secredId);

                List<Tb_MD_Subasta_Detalle> detalles = new List<Tb_MD_Subasta_Detalle>();
                detalles = context.Tb_MD_Subasta_Detalle.Where(x => x.SecredId_Subasta == secredId).ToList();
                if (detalles.Count > 0)
                {
                    string operacion = detalles[0].vTipoDetalle;

                    if (operacion == "C")
                    {
                        result.data.puja = detalles.OrderBy(x => x.TipoCambio).Select(x => new Subasta_PartnerSubastaPuja()
                        {
                            tipoCambioText = string.Format("{0:#.000}", x.TipoCambio),
                            //tipoCambioText =  x.TipoCambio.,
                            tipoCambio = x.TipoCambio,
                            nombre = x.RazonSocial,
                            codigo = x.iIdSubastaDEtalle,
                            id = x.vNumDocPartner,
                            subasta = x.SecredId_Subasta.ToString()
                        }).ToList();
                    }
                    else
                    {
                        result.data.puja = detalles.OrderByDescending(x => x.TipoCambio).Select(x => new Subasta_PartnerSubastaPuja()
                        {
                            tipoCambioText = string.Format("{0:#.000}", x.TipoCambio),
                            tipoCambio = x.TipoCambio,
                            nombre = x.RazonSocial,
                            codigo = x.iIdSubastaDEtalle,
                            subasta = x.SecredId_Subasta.ToString(),
                            id = x.vNumDocPartner
                        }).ToList();
                    }

                    Subasta_PartnerSubastaPuja myPartner = null;
                    if (!subasta.TipoCambioGanador.HasValue)
                    {
                        myPartner = result.data.puja.First();
                        result.other2 = myPartner.codigo;
                    }
                    else
                    {
                        myPartner = result.data.puja.First(x => x.codigo == subasta.TipoCambioGanador);
                        result.other2 = myPartner.codigo;
                    }

                    if (subasta.vTipoOperacion == "C")
                    {
                        result.data.seleccion.valorRecibe = subasta.nMontiRecibeCliente ?? 0M;
                        result.data.seleccion.valorEnvio = (subasta.nMontiRecibeCliente ?? 0M) * (myPartner.tipoCambio ?? 0);
                        result.data.seleccion.monto = (subasta.nMontiRecibeCliente ?? 0M);
                    }

                    else
                    {
                        result.data.seleccion.valorEnvio = subasta.nMontoEnviaCliente ?? 0;
                        result.data.seleccion.valorRecibe = (subasta.nMontoEnviaCliente ?? 0M) * (myPartner.tipoCambio ?? 0);
                        result.data.seleccion.monto = (subasta.nMontoEnviaCliente ?? 0M);
                    }

                    result.data.seleccion.tipoCambio = myPartner.tipoCambio ?? 0;
                    result.data.seleccion.partner = myPartner.nombre;

                    foreach (var p in result.data.puja)
                    {
                        result.other = result.other + string.Format("{0},{1},", p.codigo, p.tipoCambio);
                    }


                }
            }

            return result;

        }

        public BaseResponse<SubastaCurrentTime> getTiempoSubasta(string subasta_)
        {
            BaseResponse<SubastaCurrentTime> result = new BaseResponse<SubastaCurrentTime>();
            result.data = new SubastaCurrentTime();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        if (subasta.vEstadoSubasta == "C")
                        {
                            result.data.tiempo = (int)((subasta.dFechaConfirmacionOperacion.Value.AddSeconds(subasta.nTiempoConfitmacionPago ?? 0)) - DateTime.Now).TotalSeconds;
                            result.data.estado = "D";

                            if (result.data.tiempo < 0) {
                                subasta.vEstadoSubasta = "Y";
                                Tb_MD_Clientes cliente = context.Tb_MD_Clientes.Where(m => m.iIdCliente == subasta.IdCliente).FirstOrDefault();
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
                                context.SaveChanges();
                                transaction.Commit();
                            }

                        }
                        else
                        {
                            if (subasta.vEstadoSubasta != "Y") {
                                DateTime tiempoSubastaFin = subasta.dFechaInicioSubasta.Value.AddSeconds(subasta.nTiempoSubasta ?? 0);
                                int restanteSubastaActiva = (int)(tiempoSubastaFin - DateTime.Now).TotalSeconds;
                                if (restanteSubastaActiva <= 0)
                                {
                                    result.data.tiempo = (int)((tiempoSubastaFin.AddSeconds(subasta.nTiempoConfitmacionOperacion ?? 0)) - DateTime.Now).TotalSeconds;
                                    result.data.estado = "B";
                                }
                                else
                                {
                                    result.data.tiempo = restanteSubastaActiva;
                                    result.data.estado = "A";
                                }

                            }
                          

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

        public BaseResponse<SubastaProceso_Response> getModelLoadSubastaActiva(string subasta_)
        {
            BaseResponse<SubastaProceso_Response> result = new BaseResponse<SubastaProceso_Response>();
            result.data = new SubastaProceso_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta aya concluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");


                        result.data.initSubasta = subasta.bInitSubasta ? 1 : 0;
                        
                        result.success = true;

                        List<Tb_MD_Subasta_Detalle> detalles = new List<Tb_MD_Subasta_Detalle>();
                        detalles = context.Tb_MD_Subasta_Detalle.Where(x => x.SecredId_Subasta == secredId).ToList();
                        if (detalles.Count > 0)
                        {
                            string operacion = detalles[0].vTipoDetalle;

                            if (operacion == "C")
                            {
                                result.data.partners = detalles.OrderBy(x => x.TipoCambio).Select(x => new Subasta_PartnerSubastaPuja()
                                {
                                    tipoCambioText = string.Format("{0:#.000}", x.TipoCambio),
                                    tipoCambio = x.TipoCambio,
                                    nombre = x.RazonSocial,
                                    codigo = x.iIdSubastaDEtalle,
                                    id = x.vNumDocPartner,
                                    subasta = subasta_,
                                }).ToList();
                            }
                            else
                            {
                                result.data.partners = detalles.OrderByDescending(x => x.TipoCambio).Select(x => new Subasta_PartnerSubastaPuja()
                                {

                                    tipoCambioText = string.Format("{0:#.000}", x.TipoCambio),
                                    tipoCambio = x.TipoCambio,
                                    nombre = x.RazonSocial,
                                    codigo = x.iIdSubastaDEtalle,
                                    id = x.vNumDocPartner,
                                    subasta = subasta_,
                                }).ToList();
                            }

                        }


                        foreach (var p in result.data.partners)
                        {
                            result.other = result.other + string.Format("{0},{1},", p.codigo, p.tipoCambio);
                        }

                        Subasta_PartnerSubastaPuja myPartner = null;
                        if (!subasta.TipoCambioGanador.HasValue)
                        {
                            myPartner = result.data.partners.First();
                            result.data.codigoSeleccion = myPartner.codigo;
                        }
                        else
                        {
                            myPartner = result.data.partners.First(x => x.codigo == subasta.TipoCambioGanador);
                            result.data.codigoSeleccion = myPartner.codigo;
                        }


                        foreach (var t in result.data.partners)
                        {
                            if (t.codigo == result.data.codigoSeleccion)
                            {
                                t.classCss = "item item-s-k Activo";
                            }
                            else
                            {
                                t.classCss = "item item-s-k";
                            }
                            t.tipoCambioText = string.Format("{0:#.000}", t.tipoCambio);
                        }


                        result.data.subasta = subasta.nNumeroSubasta;
                        result.data.monedaEnvio = subasta.vMonedaEnviaCliente;
                        result.data.monedaRecive = subasta.vMonedaRecibeCliente;
                        result.data.tipoCambio = myPartner.tipoCambio ?? 0;

                        if (subasta.vTipoOperacion == "C")
                        {
                            result.data.valorRecibe = subasta.nMontiRecibeCliente ?? 0M;
                            result.data.valorEnvio = (subasta.nMontiRecibeCliente ?? 0M) * (myPartner.tipoCambio ?? 0);
                            result.data.monto = (subasta.nMontiRecibeCliente ?? 0M);
                        }

                        else
                        {
                            result.data.valorEnvio = subasta.nMontoEnviaCliente ?? 0;
                            result.data.valorRecibe = (subasta.nMontoEnviaCliente ?? 0M) * (myPartner.tipoCambio ?? 0);
                            result.data.monto = (subasta.nMontoEnviaCliente ?? 0M);
                        }


                        result.data.partner = myPartner.nombre;
                        result.data.tiempo = subasta.nTiempoSubasta ?? 0;
                        result.data.tiempoConfirmacion = subasta.nTiempoConfirmacion ?? 0;
                        result.data.opr = subasta.vTipoOperacion;

                        result.data.sid = subasta.SecredId.ToString();

                        subasta.bInitSubasta = true;
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        result.success = false;
                    }
                }
            }

            return result;
        }

        public BaseResponse<SubastaProceso_Response> crearSubastaInit(string operacion, decimal monto, string moneda, string monedaEnvio, string monedaRecibe, int IdCliente)
        {
            BaseResponse<SubastaProceso_Response> result = new BaseResponse<SubastaProceso_Response>();
            result.data = new SubastaProceso_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        Tb_MD_Clientes cliente = null;
                        cliente = context.Tb_MD_Clientes.FirstOrDefault(x => x.iIdCliente == IdCliente);

                        List<string> IdsTiempos = new List<string> { "T_Sb", "T_SbCf", "T_SbCo", "TSbCp" };

                        List<Tb_MD_Tiempos> tiempos = context.Tb_MD_Tiempos.Where(x => IdsTiempos.Contains(x.vCodTransaccion)).ToList();

                        Tb_MD_Subasta subasta = new Tb_MD_Subasta();
                        subasta.dFechaCreacion = DateTime.Now;

                        if (operacion == "C")
                            subasta.nMontiRecibeCliente = monto;
                        else
                            subasta.nMontoEnviaCliente = monto;

                        subasta.vEstadoSubasta = EstadoSubasta.Abandonada;
                        subasta.vTipoOperacion = operacion;
                        subasta.vMonedaEnviaCliente = monedaEnvio;
                        subasta.vMonedaRecibeCliente = monedaRecibe;
                        subasta.vEstadoSubasta = "A";
                        subasta.nTiempoSubasta = tiempos.First(x => x.vCodTransaccion.Equals("T_Sb")).nTiempoStandar ?? 0;
                        subasta.nTiempoConfirmacion = tiempos.First(x => x.vCodTransaccion.Equals("T_SbCf")).nTiempoStandar ?? 0;
                        subasta.nTiempoConfitmacionOperacion = tiempos.First(x => x.vCodTransaccion.Equals("T_SbCo")).nTiempoStandar ?? 0;
                        subasta.nTiempoConfitmacionPago = tiempos.First(x => x.vCodTransaccion.Equals("TSbCp")).nTiempoStandar ?? 0;
                        subasta.SecredId = Guid.NewGuid();
                        subasta.IdCliente = IdCliente;
                        subasta.NombreCliente = cliente.NombreCliente;
                        context.Tb_MD_Subasta.Add(subasta);


                        List<TiposCambioGarantizado_SubastaProceso_Request> partners = new List<TiposCambioGarantizado_SubastaProceso_Request>();

                        #region Get Partner's

                        var montoParam = new SqlParameter { ParameterName = "monto", Value = monto };
                        var operacionParam = new SqlParameter { ParameterName = "operacion", Value = operacion };
                        var monedaParam = new SqlParameter { ParameterName = "moneda", Value = moneda };

                        partners = context.Database.SqlQuery<TiposCambioGarantizado_SubastaProceso_Request>("exec proc_self_partners_subasta_proceso @monto,@operacion,@moneda", montoParam, operacionParam, monedaParam).ToList<TiposCambioGarantizado_SubastaProceso_Request>();

                        #endregion

                        foreach (var sd in partners)
                        {
                            Tb_MD_Subasta_Detalle dSubasta = new Tb_MD_Subasta_Detalle();
                            dSubasta.vTipoDetalle = subasta.vTipoOperacion;
                            dSubasta.vNumDocPartner = sd.nroDocumento;
                            dSubasta.nValorRangoMinimo = sd.valorMinimo;
                            dSubasta.nValorRangoMaximo = sd.valorMaximo;

                            dSubasta.nValorCompra = sd.valorCompra;
                            dSubasta.nValorVenta = sd.valorVenta;
                            dSubasta.dFechaCreacion = DateTime.Now;
                            dSubasta.RazonSocial = sd.razonSocial;
                            dSubasta.Tb_MD_Subasta = subasta;
                            dSubasta.SecredId_Subasta = subasta.SecredId;
                            dSubasta.UltimoTipoCambioGarantizado = true;
                            dSubasta.TipoCambio = sd.tipoCambio;

                            context.Tb_MD_Subasta_Detalle.Add(dSubasta);


                        }
                        context.SaveChanges();

                        //result.data.partners = context.Tb_MD_Subasta_Detalle.Where(x => x.nNumeroSubasta == subasta.nNumeroSubasta).Select(x => new SubastaCliente_PartnerPuja { 
                        // tipoCambio = x.TipoCambio,
                        // nombre  = x.RazonSocial,
                        // codigo = x.iIdSubastaDEtalle,
                        // id = x.vNumDocPartner
                        //}).ToList();

                        subasta.dFechaInicioSubasta = DateTime.Now;
                        context.SaveChanges();
                        

                        TiposCambioGarantizado_SubastaProceso_Request myPartner = null;
                        myPartner = partners.FirstOrDefault();

                        if (myPartner == null)
                            throw new Exception("No se encontraron participantes que avalen el monto que intentas vender/comprar.");

                        result.success = true;

                        result.data.subasta = subasta.nNumeroSubasta;
                        result.data.monedaEnvio = monedaEnvio;
                        result.data.monedaRecive = monedaRecibe;
                        result.data.tipoCambio = myPartner.tipoCambio;

                        if (operacion == "C")
                        {
                            result.data.valorRecibe = monto;
                            result.data.valorEnvio = monto * myPartner.tipoCambio;
                        }

                        else
                        {
                            result.data.valorEnvio = monto;
                            result.data.valorRecibe = monto * myPartner.tipoCambio;
                        }


                        result.data.partner = myPartner.razonSocial;
                        result.data.tiempo = subasta.nTiempoSubasta ?? 0;
                        result.data.tiempoConfirmacion = subasta.nTiempoConfirmacion ?? 0;
                        result.data.opr = subasta.vTipoOperacion;
                        result.data.monto = monto;
                        result.data.sid = subasta.SecredId.ToString();

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
                        //esult.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> ConfirmarPartnerSubasta(string subasta_, int partnetGanador)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {

                    try
                    {

                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        Tb_MD_Subasta_Detalle detalle = null;
                        detalle = context.Tb_MD_Subasta_Detalle.FirstOrDefault(x => x.iIdSubastaDEtalle == partnetGanador);
                        if (detalle == null)
                            throw new Exception("No se encuetra la subasta.");

                        subasta.TipoCambioGanador = partnetGanador;
                        subasta.vEstadoSubasta = "B";

                        if (subasta.vTipoOperacion == "C")
                        {
                            subasta.nMontoEnviaCliente = subasta.nMontiRecibeCliente * detalle.TipoCambio;
                        }
                        else
                        {
                            subasta.nMontiRecibeCliente = subasta.nMontoEnviaCliente * detalle.TipoCambio;
                        }



                        subasta.dFechaSeleccion = DateTime.Now;

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

        public BaseResponse<Subasta_ConfirmarOperacion_Response> getModelLoadConfirmarSubasta(string subasta_)
        {
            BaseResponse<Subasta_ConfirmarOperacion_Response> result = new BaseResponse<Subasta_ConfirmarOperacion_Response>();
            result.data = new Subasta_ConfirmarOperacion_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");


                        Tb_MD_Subasta_Detalle detalle = null;
                        detalle = context.Tb_MD_Subasta_Detalle.FirstOrDefault(x => x.iIdSubastaDEtalle == subasta.TipoCambioGanador);
                        if (detalle == null)
                            throw new Exception("No se encuetra la subasta.");



                        result.data.transaccion = subasta.nNumeroSubasta;
                        if (subasta.vTipoOperacion == "C")
                        {
                            result.data.monedaEnvio = "S/.";
                            result.data.monedaRecibe = "$";

                        }
                        else
                        {
                            result.data.monedaEnvio = "$";
                            result.data.monedaRecibe = "S/.";
                        }


                        result.data.nombrePartner = detalle.RazonSocial;
                        result.data.subasta = subasta.SecredId.ToString();
                        result.data.tiempo = subasta.nTiempoConfitmacionOperacion ?? 0;
                        result.data.tipoCambio = detalle.TipoCambio ?? 0;
                        result.data.valorRecibe = subasta.nMontiRecibeCliente ?? 0;
                        result.data.valorEnvio = subasta.nMontoEnviaCliente ?? 0;
                        result.data.initSubasta = subasta.bInitSubasta ? 1 : 0;


                        result.success = true;
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

        public BaseResponse<string> ConfirmarOperacionSubasta(string subasta_, string password, int cliente)
        {
            BaseResponse<string> result = new BaseResponse<string>();


            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_ClienteUsuario usuario = null;
                        usuario = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.IdUsuario == cliente);
                        if (usuario == null)
                            throw new Exception("No se encuetra la subasta.");

                        string clave = Helper.Encrypt.EncryptKey(password);

                        if (usuario.Password != clave)
                            throw new Exception("La contraseña es incorrecta.");

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        subasta.vEstadoSubasta = "C";
                        subasta.dFechaConfirmacionOperacion = DateTime.Now;

                        string t=ConfigurationManager.AppSettings["tiempo_confirmacion"].ToString();
                        Tb_MD_Tiempos tiempo = context.Tb_MD_Tiempos.Where(x => x.vCodTransaccion == t).FirstOrDefault();

                        Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                        notificacion.IdUsuario = usuario.IdUsuario.ToString();
                        notificacion.IdCliente = usuario.IdCliente;
                        notificacion.Titulo = "Subasta en Curso";
                        notificacion.Mensaje = "Tu número de transaccion es: " + String.Format("{0:000000000}", subasta.nNumeroSubasta) + ".Usted confirmo la subasta con su contraseña, Ingrese su codigo de Voucher para continuar";
                        notificacion.Tipo = 0;
                        notificacion.vNumeroSubasta = subasta.nNumeroSubasta.ToString();
                        notificacion.vEstadoSubasta = EstadoSubasta.Confirmada;
                        notificacion.Url = "/Subasta/Envio/"+subasta.SecredId;
                        if (tiempo != null) {
                            int second = Convert.ToInt16( tiempo.nTiempoStandar);
                            notificacion.Fecha = DateTime.Now.AddSeconds(second);
                        }
                        
                      
                        notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                        context.Tb_MD_Notificacion.Add(notificacion);


                        context.SaveChanges();
                        transaction.Commit();

                        result.data = subasta.SecredId.ToString();
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

                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;

        }

        public BaseResponse<Subasta_ConfirmarPago_Response> getModelLoadConfirmarPago(string subasta_)
        {
            BaseResponse<Subasta_ConfirmarPago_Response> result = new BaseResponse<Subasta_ConfirmarPago_Response>();
            result.data = new Subasta_ConfirmarPago_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);
                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        Tb_MD_Subasta_Detalle detalle = null;
                        detalle = context.Tb_MD_Subasta_Detalle.FirstOrDefault(x => x.iIdSubastaDEtalle == subasta.TipoCambioGanador);
                        if (detalle == null)
                            throw new Exception("No se encuetra la subasta.");


                        result.data.transaccion = subasta.nNumeroSubasta;
                        if (subasta.vTipoOperacion == "C")
                        {
                            result.data.monedaEnvio = "S/.";
                            result.data.monedaRecibe = "$";

                        }
                        else
                        {
                            result.data.monedaEnvio = "$";
                            result.data.monedaRecibe = "S/.";
                        }

                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];

                        result.data.cuentaBancos = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == subasta.IdCliente && x.vMoneda==subasta.vMonedaEnviaCliente && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new ComboCuentasBancarias
                        {
                            id = x.iIdDatosBank,
                            nro = x.vNroCuenta,
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad
                        }).ToList();

                        result.data.cuentaBancosDestino = context.Tb_MD_ClientesDatosBancos.Where(x => x.iIdCliente == subasta.IdCliente && x.vMoneda==subasta.vMonedaRecibeCliente && x.vEstadoRegistro == EstadoRegistroTabla.Activo).Select(x => new ComboCuentasBancarias
                        {
                            id = x.iIdDatosBank,
                            nro = x.vNroCuenta,
                            logo = host + x.Tb_MD_Entidades_Financieras.vLogoEntidad
                        }).ToList();


                        result.data.nombreTitular = subasta.NombreCliente;
                        result.data.nombrePartner = detalle.RazonSocial;
                        result.data.sid = subasta.SecredId.ToString();
                        result.data.tiempo = subasta.nTiempoConfitmacionPago ?? 0;
                        result.data.tipoCambio = detalle.TipoCambio ?? 0;
                        result.data.valorRecibe = subasta.nMontiRecibeCliente ?? 0;
                        result.data.valorEnvio = subasta.nMontoEnviaCliente ?? 0;
                        result.data.fechaConfirmacion = subasta.dFechaConfirmacionOperacion ?? DateTime.Now;

                        result.success = true;




                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<string> ConfirmarPagoSubasta(string subasta_, string codigoOperacion, long cuentaOrigen, long cuentaDestino, int _usuario,string bancoFid,long cuentaFid)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_ClienteUsuario usuario = null;
                        usuario = context.Tb_MD_ClienteUsuario.FirstOrDefault(x => x.IdUsuario == _usuario);
                        if (usuario == null)
                            throw new Exception("No se encuetra la subasta.");

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);

                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        //Tb_MD_Subasta_Detalle detalle = null;
                        //detalle = context.Tb_MD_Subasta_Detalle.FirstOrDefault(x => x.iIdSubastaDEtalle == subasta.TipoCambioGanador);
                        //if (detalle == null)
                        //    throw new Exception("No se encuetra la subasta.");


                        subasta.cuentaBancoOrigen = cuentaOrigen;
                        subasta.cuentaBancoDestino = cuentaDestino;
                        subasta.NroOperacionPago = codigoOperacion;
                        subasta.dFechaConfirmacionPago = DateTime.Now;

                        //Subasta Fideicomiso
                       
                        subasta.vEstadoSubasta = EstadoSubasta.PagadaXCliente;


                        Tb_MD_ClientesDatosBancos c_origen = context.Tb_MD_ClientesDatosBancos.FirstOrDefault(x => x.iIdDatosBank == cuentaOrigen);
                        Tb_MD_ClientesDatosBancos c_destino = context.Tb_MD_ClientesDatosBancos.FirstOrDefault(x => x.iIdDatosBank == cuentaDestino);


                        Tb_MD_Subasta_Pago _subastaPago = new Tb_MD_Subasta_Pago();
                        _subastaPago.nNumeroSubasta = subasta.nNumeroSubasta;
                        _subastaPago.vNroDocumento = usuario.vNroDocumento;
                        _subastaPago.dFechaInformePago = subasta.dFechaConfirmacionPago;
                        _subastaPago.vNumOperacionPago = codigoOperacion;
                        _subastaPago.vCodBancoCliente = c_origen.vBanco;
                        _subastaPago.vNumeroCuenta = c_origen.vNroCuenta;
                        _subastaPago.vTipoMonedaTransferida = subasta.vMonedaEnviaCliente;
                        _subastaPago.nMontoTransferido = subasta.nMontoEnviaCliente;
                        _subastaPago.vCodBancoFideicomiso = bancoFid;
                        _subastaPago.vNumeroCuentaFideicomiso = cuentaFid.ToString();
                        // datos de cuenta destino
                        _subastaPago.vCodBancoDestinoCliente = c_destino.vBanco;
                        _subastaPago.nMontoTransferidoACliente = subasta.nMontiRecibeCliente;
                        _subastaPago.vTipoMonedaDestinoCliente = subasta.vMonedaRecibeCliente;
                        _subastaPago.vNumeroCuentaDestinoCliente = c_destino.vNroCuenta;

                        _subastaPago.iEstadoRegistro = EstadoRegistroTabla.Activo;

                        context.Tb_MD_Subasta_Pago.Add(_subastaPago);

                        /*----- Notificacion -----*/
                        Tb_MD_Notificacion limpNot = new Tb_MD_Notificacion();
                        string nrsubasta=subasta.nNumeroSubasta.ToString();
                        limpNot = context.Tb_MD_Notificacion.Where(x => x.vNumeroSubasta == nrsubasta && x.vEstadoSubasta == EstadoSubasta.Confirmada).FirstOrDefault();
                        if (limpNot != null) {
                            limpNot.iEstadoRegistro = EstadoRegistroTabla.NoActivo;
                        }

                        Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                        notificacion.IdUsuario = usuario.IdUsuario.ToString();
                        notificacion.IdCliente = usuario.IdCliente;
                        notificacion.Titulo = "En Proceso de Verificacion";
                        notificacion.Mensaje =  "Tu número de transaccion es: " + String.Format("{0:000000000}", subasta.nNumeroSubasta)+". Si tienes Cualquier pregunta, puedes contactarnos por nuestros canales de atención.";
                        notificacion.Tipo = 0;
                        notificacion.vNumeroSubasta = subasta.nNumeroSubasta.ToString();
                        notificacion.vEstadoSubasta = EstadoSubasta.PagadaXCliente;
                        notificacion.Url = "/Subasta/Verificacion/" + subasta.SecredId;
                        notificacion.Fecha = DateTime.Now.AddDays(2);
                        
                        notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                        context.Tb_MD_Notificacion.Add(notificacion);
                        
                        /*-------------------------*/

                        context.SaveChanges();
                        transaction.Commit();

                        result.data = subasta_;


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

                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }

            return result;
        }

        public BaseResponse<Subasta_Verificacion_Response> getModelVerificacionSubasta(string subasta_)
        {
            BaseResponse<Subasta_Verificacion_Response> result = new BaseResponse<Subasta_Verificacion_Response>();
            result.data = new Subasta_Verificacion_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);
                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        Tb_MD_Subasta_Pago subastaPago = null;
                        subastaPago = context.Tb_MD_Subasta_Pago.FirstOrDefault(x => x.nNumeroSubasta == subasta.nNumeroSubasta);
                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        result.data.bancoOriegn = subastaPago.vCodBancoCliente;
                        result.data.nroSubasta = subasta.nNumeroSubasta.ToString();
                        result.data.fechaConfirmacion = subasta.dFechaConfirmacionOperacion ?? DateTime.Now;
                        result.data.fechaPago = subasta.dFechaConfirmacionPago ?? DateTime.Now;
                        result.data.montoEnvia = subasta.nMontoEnviaCliente ?? 0;
                        result.data.montoRecibe = subasta.nMontiRecibeCliente ?? 0;
                        result.data.monedaEnvia = subasta.vMonedaEnviaCliente;
                        result.data.monedaRecibe = subasta.vMonedaRecibeCliente;
                        result.data.cliente = subasta.NombreCliente;
                        result.data.nroOperacion = subasta.NroOperacionPago;
                        result.data.sid = subasta.SecredId.ToString();

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

        public BaseResponse<Subasta_Recibe_Response> getModelRecibeSubasta(string subasta_)
        {
            BaseResponse<Subasta_Recibe_Response> result = new BaseResponse<Subasta_Recibe_Response>();
            result.data = new Subasta_Recibe_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Guid secredId = Guid.NewGuid();

                        try
                        {
                            secredId = Guid.Parse(subasta_);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Ocurrio un error con la subasta, es posible q no tenga acceso o la subasta ayaconcluido.");
                        }

                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.SecredId == secredId);
                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        Tb_MD_Subasta_Pago subastaPago = null;
                        subastaPago = context.Tb_MD_Subasta_Pago.FirstOrDefault(x => x.nNumeroSubasta == subasta.nNumeroSubasta);
                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        result.data.bancoOriegn = subastaPago.vCodBancoCliente;
                        //result.data.
                        var cliente=context.Tb_MD_Clientes.Where(m => m.iIdCliente == subasta.IdCliente).FirstOrDefault();
                        if(cliente!=null){
                        result.data.cliente = cliente.NombreCliente;
                        }
                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];
                        var bancoDestino = context.Tb_MD_Entidades_Financieras.Where(x => x.vCodEntidad == subastaPago.vCodBancoDestinoCliente).FirstOrDefault();

                        if (bancoDestino != null) {
                            result.data.bancoDestino = bancoDestino.vDesEntidad;
                            result.data.bancoDestinoLogo =host+ bancoDestino.vLogoEntidad;
                        }
                        
                        CultureInfo culture;
                        string specifier;
                        specifier = "N3";
                        culture = CultureInfo.CreateSpecificCulture("eu-ES");
                        //return  valorEnvio.ToString(specifier,CultureInfo.InvariantCulture); }
                        decimal montorecibe=subasta.nMontiRecibeCliente??0;
                        decimal montoenvia = subasta.nMontoEnviaCliente ?? 0;
                        result.data.fechaConfirmacion = subasta.dFechaConfirmacionOperacion ?? DateTime.Now;
                        result.data.fechaPago = subasta.dFechaConfirmacionPago ?? DateTime.Now;
                        result.data.fechaValidaRecibio = subastaPago.dFechaValidacionOperaciones?? DateTime.Now;
                        result.data.fechaInformePago = subastaPago.dFechaInformeContravalor ?? DateTime.Now;
                        result.data.numOpBancoCliente = subastaPago.vNumOpeBancoACliente;
                        result.data.montoEnvia = montoenvia.ToString(specifier, CultureInfo.InvariantCulture);
                        result.data.montoRecibe  = montorecibe.ToString(specifier,CultureInfo.InvariantCulture);
                        result.data.numeroCuentaDestino = subastaPago.vNumeroCuentaDestinoCliente;
                        result.data.monedaEnvia = subasta.vMonedaEnviaCliente ?? "";
                        result.data.monedaRecibe = subasta.vMonedaRecibeCliente ?? "";
                        result.data.nroOperacion = subasta.NroOperacionPago;
                        result.data.sid = subasta.SecredId.ToString();

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


        public BaseResponse<List<SubastaCompararProveedorResponse>> ListaSubastaComparar(SubastaCompararProveedorRequest param)
        {
            BaseResponse<List<SubastaCompararProveedorResponse>> valorRegistrados = new BaseResponse<List<SubastaCompararProveedorResponse>>();

            try
            {
                valorRegistrados.data = new List<SubastaCompararProveedorResponse>();

              
                #region Parametros
                var ventaParam = new SqlParameter { ParameterName = "venta", Value = param.venta };
                var montoParam = new SqlParameter { ParameterName = "monto", Value = param.monto };
             
                #endregion

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<SubastaCompararProveedorResponse>("exec Proc_Sel_comparar_proveedor @venta,@monto", ventaParam, montoParam).ToList<SubastaCompararProveedorResponse>();
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

        public PageResultSP<ListaOperacionSubastaCliente> ListaOperacionesCliente(PageResultParam param, int IdCurrenCliente)
        {
            PageResultSP<ListaOperacionSubastaCliente> valorRegistrados = new PageResultSP<ListaOperacionSubastaCliente>();
            try
            {
                valorRegistrados.data = new List<ListaOperacionSubastaCliente>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                var clienteParam = new SqlParameter { ParameterName = "IdCliente", Value = IdCurrenCliente };
                var tipoParam = new SqlParameter { ParameterName = "vTipoFiltro", Value = param.textFilter };
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<ListaOperacionSubastaCliente>("exec Proc_Sel_ListaOperacionCliente @PageNumber,@ItemsPerPage,@IdCliente,@vTipoFiltro", pageParam, itemsParam, clienteParam, tipoParam).ToList<ListaOperacionSubastaCliente>();

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



        public BaseResponse<Subasta_Verificacion_Response> getSubastaEstadoVerificia(SubastaRequest subasta_)
        {
            BaseResponse<Subasta_Verificacion_Response> result = new BaseResponse<Subasta_Verificacion_Response>();
            result.data = new Subasta_Verificacion_Response();

            using (MesaDineroContext context = new MesaDineroContext())
            {
               
                    try
                    {
                       int idsubasta=Convert.ToInt16(subasta_.idTransaccion);
                        Tb_MD_Subasta subasta = null;
                        subasta = context.Tb_MD_Subasta.FirstOrDefault(x => x.nNumeroSubasta == idsubasta);
                        if (subasta == null)
                            throw new Exception("No se encuetra la subasta.");

                        Subasta_Verificacion_Response sub = new Subasta_Verificacion_Response();
                        sub.estado = subasta.vEstadoSubasta;
                        result.data = sub;

                        result.success = true;

                    }
                    catch (Exception ex)
                    {
                        result.success = false;

                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                
            }

            return result;
        }




    }
}
