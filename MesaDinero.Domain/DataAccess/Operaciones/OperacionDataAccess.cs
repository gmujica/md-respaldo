using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Helper;
using MesaDinero.Domain.Model;
using MesaDinero.Domain.Model.operaciones;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess.operaciones
{
    public class OperacionDataAccess
    {

        public PageResultSP<VerificacionPagoResponse> ListaVerificarPago(PageResultParam param)
        {
            PageResultSP<VerificacionPagoResponse> valorRegistrados = new PageResultSP<VerificacionPagoResponse>();

            try
            {
                valorRegistrados.data = new List<VerificacionPagoResponse>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                var modoLista = new SqlParameter { ParameterName = "ModoLista", Value = param.textFilter };
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<VerificacionPagoResponse>("exec Proc_Sel_Verificacion_Pago @PageNumber,@ItemsPerPage,@ModoLista", pageParam, itemsParam, modoLista).ToList<VerificacionPagoResponse>();

                    
                    foreach (var cliente in valorRegistrados.data)
                    {
                        //cliente.fechaShort = cliente.fecha.ToString("dd/MM/yyyy");
                        if (cliente.fechaConf != null)
                        {
                            cliente.fechaShort = cliente.fechaConf.Value.ToString("dd/MM/yyyy");
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

        public PageResultSP<ListaGenerarPagoResponse> ListaGenerarPago(FiltroOperacionParam param)
        {
            PageResultSP<ListaGenerarPagoResponse> valorRegistrados = new PageResultSP<ListaGenerarPagoResponse>();

            try
            {
                valorRegistrados.data = new List<ListaGenerarPagoResponse>();

                if (param.textFilter == null) 
                {
                    param.textFilter = "";
                }

                if (param.searchFilter == null)
                {
                    param.searchFilter = "";
                }

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                var bancoParam = new SqlParameter { ParameterName = "BancoDestino", Value = param.textFilter };
                var tipoMoneParam= new SqlParameter { ParameterName = "TipoMoneda", Value = param.searchFilter };
                #endregion


                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<ListaGenerarPagoResponse>("exec Proc_Sel_Generar_Pago @PageNumber,@ItemsPerPage,@BancoDestino,@TipoMoneda", pageParam, itemsParam, bancoParam, tipoMoneParam).ToList<ListaGenerarPagoResponse>();


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

        public BaseResponse<string> actualizarEstadoVerificado(List<SubastaRequest> model, string nombreusuario)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.Count == 0) {
                            throw new Exception("Seleccione una Transaccion");
                        }

                        //string [] codigoTransaccion=model.idTransaccion.Split(',');

                        for (int i = 0; i < model.Count; i++)
                        {
                            Tb_MD_Subasta entidad = context.Tb_MD_Subasta.Find(Convert.ToInt16(model[i].idTransaccion));
                            Tb_MD_ClientesDatosBancos banco = context.Tb_MD_ClientesDatosBancos.Find(entidad.cuentaBancoDestino);
                            Tb_MD_ClientesDatosBancos bancoOrigen = context.Tb_MD_ClientesDatosBancos.Find(entidad.cuentaBancoOrigen);
                            //Tb_MD_Pre_Clientes cliente = context.Tb_MD_Pre_Clientes.Find(entidad.IdCliente);
                            if (entidad == null)
                            {
                                throw new Exception("Codigo no exite");
                            }
                            entidad.vNumInsPago = model[i].idPago;
                            entidad.vEstadoSubasta = model[i].estado;
                            context.SaveChanges();


                            /*Insertar a la tabla pagos*/
                            if (model[i].tipoValidacion != null && model[i].tipoValidacion != "")
                            {
                                int idSubasta = entidad.nNumeroSubasta;
                                DateTime fechaInformePago = DateTime.Now;

                                if (model[i].tipoValidacion.Trim() == "VO")
                                {
                                    /*ACTUALIZAR AHORA*/
                                    Tb_MD_Notificacion limpNot = new Tb_MD_Notificacion();
                                    string nrsubasta = entidad.nNumeroSubasta.ToString();
                                    limpNot = context.Tb_MD_Notificacion.Where(x => x.vNumeroSubasta == nrsubasta && x.vEstadoSubasta == EstadoSubasta.PagadaXCliente).FirstOrDefault();


                                    string estadoOperador = "";
                                    if (model[i].estado.Trim() == "G")
                                    {
                                        estadoOperador = "A";
                                        if (limpNot != null)
                                        {
                                            limpNot.Titulo = "Operación Verificada";
                                        }
                                    }
                                    else
                                    {
                                        estadoOperador = "O";
                                        if (limpNot != null)
                                        {
                                            limpNot.Titulo = "Operación Observada";
                                        }
                                    }
                                    /*ACTUALIZAR AHORA*/

                                    Tb_MD_Subasta_Pago subasta_pago_val = context.Tb_MD_Subasta_Pago.Find(idSubasta);

                                    if (subasta_pago_val == null)
                                    {
                                        Tb_MD_Subasta_Pago subasta_pago = new Tb_MD_Subasta_Pago();
                                        subasta_pago.nNumeroSubasta = idSubasta;
                                        subasta_pago.vCodBancoCliente = bancoOrigen.vBanco;
                                        subasta_pago.vNumeroCuenta = bancoOrigen.vNroCuenta;
                                        subasta_pago.dFechaInformePago = DateTime.Now;
                                        subasta_pago.vNumOperacionPago = entidad.NroOperacionPago;
                                        subasta_pago.vTipoMonedaTransferida = entidad.vMonedaEnviaCliente;
                                        subasta_pago.nMontoTransferido = entidad.nMontoEnviaCliente;
                                        //subasta_pago.vTipoPersona = cliente.vTipoCliente.ToString();
                                        //subasta_pago.vCodBancoFideicomiso = "";
                                        //subasta_pago.vNumeroCuentaFideicomiso = "";
                                        //subasta_pago.vNroDocumento = cliente.vNroDocumento;
                                        subasta_pago.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                        context.Tb_MD_Subasta_Pago.Add(subasta_pago);
                                    }
                                    Tb_MD_Subasta_Pago subasta_pago_find = context.Tb_MD_Subasta_Pago.Find(idSubasta);

                                    if (subasta_pago_find!=null) {
                                        subasta_pago_find.dFechaValidacionOperaciones = fechaInformePago;
                                        subasta_pago_find.vNumDocValidaDepositoOperaciones = nombreusuario;
                                        subasta_pago_find.vEstadoValOperador = estadoOperador;
                                        context.SaveChanges();
                                    }
                                }

                                //if (model[i].tipoValidacion.Trim() == "VF")
                                //{
                                //    string estadoOperador = "";
                                //    if (model[i].estado.Trim() == "G")
                                //    {
                                //        estadoOperador = "A";
                                //    }
                                //    else
                                //    {
                                //        estadoOperador = "O";
                                //    }
                                //    Tb_MD_Subasta_Pago subasta_pago_find = context.Tb_MD_Subasta_Pago.Find(idSubasta);

                                //    if (subasta_pago_find != null)
                                //    {
                                //        subasta_pago_find.dFechaValidacionFideicomiso = fechaInformePago;
                                //        subasta_pago_find.vNumDocValidaDepositoFideicomiso = nombreusuario;
                                //        subasta_pago_find.vEstadoValFideicomiso = estadoOperador;
                                //        context.SaveChanges();
                                //    }
                                
                                //}

                                if (model[i].tipoValidacion.Trim() == "EC")
                                {
                                    Tb_MD_Subasta_Pago subasta_pago_find = context.Tb_MD_Subasta_Pago.Find(idSubasta);

                                    if (subasta_pago_find != null)
                                    {
                                        subasta_pago_find.dFechaEnvioCorfid = DateTime.Now;
                                        context.SaveChanges();
                                    }
                                }

                                if (model[i].tipoValidacion.Trim() == "AP")
                                {
                                    Tb_MD_Subasta_Pago subasta_pago_find = context.Tb_MD_Subasta_Pago.Find(idSubasta);

                                    if (subasta_pago_find != null)
                                    {
                                        subasta_pago_find.dFechaInformeContravalor = DateTime.Now;
                                        subasta_pago_find.vNumOpeBancoACliente = model[i].idPago;
                                        subasta_pago_find.vObservacion = model[i].observacion;
                                        subasta_pago_find.vCodBancoDestinoCliente=banco.vBanco;
                                        subasta_pago_find.nMontoTransferidoACliente=entidad.nMontiRecibeCliente;
                                        subasta_pago_find.vTipoMonedaDestinoCliente = entidad.vMonedaRecibeCliente;
                                        subasta_pago_find.vNumeroCuentaDestinoCliente = banco.vNroCuenta;

                                        context.SaveChanges();

                                        /*Enviar Correo*/
                                        Tb_MD_Clientes cliente = context.Tb_MD_Clientes.Where(x => x.iIdCliente == entidad.IdCliente).FirstOrDefault();

                                        if (cliente != null)
                                        {
                                            
                                            string nombre = cliente.vNombre+" "+cliente.vApellido;
                                            string correo = cliente.vEmail;
                                            string monto = String.Format("{0:###,###,###,##0.00##}", entidad.nMontiRecibeCliente);
                                            string moneda = entidad.vMonedaRecibeCliente.ToString().ToUpper();
                                            CorreoHelper.SendCorreoPagoCliente(correo, nombre, monto, moneda,entidad.SecredId.ToString());
                                            //if (respuesta == true) { 

                                            //}

                                            Tb_MD_Notificacion limpNot = new Tb_MD_Notificacion();
                                            string nrsubasta = entidad.nNumeroSubasta.ToString();
                                            limpNot = context.Tb_MD_Notificacion.Where(x => x.vNumeroSubasta == nrsubasta && x.vEstadoSubasta == EstadoSubasta.PagadaXCliente).FirstOrDefault();
                                            if (limpNot != null)
                                            {
                                                limpNot.iEstadoRegistro = EstadoRegistroTabla.NoActivo;
                                            }

                                            Tb_MD_Notificacion notificacion = new Tb_MD_Notificacion();
                                            notificacion.IdUsuario = "";
                                            notificacion.IdCliente = cliente.iIdCliente;
                                            notificacion.Titulo = "Envio de Pago";
                                            notificacion.Mensaje =  "Tu número de transaccion es: " + String.Format("{0:000000000}", entidad.nNumeroSubasta)+". Se le deposito el tipo de cambio solicitado.";
                                            notificacion.Tipo = 0;
                                            notificacion.vNumeroSubasta = entidad.nNumeroSubasta.ToString();
                                            notificacion.vEstadoSubasta = EstadoSubasta.PagadaALCliente;
                                            notificacion.Url = "/Subasta/recibes/" + entidad.SecredId;
                                            notificacion.Fecha = DateTime.Now.AddDays(1);
                                            notificacion.iEstadoRegistro = EstadoRegistroTabla.Activo;
                                            context.Tb_MD_Notificacion.Add(notificacion);
                                            context.SaveChanges();
                                           


                                        }
                                        else {
                                            throw new Exception("No se encontro Datos del Cliente");
                                        }
                                    }
                                }


                            }
                          
                           



                        }

                        //Tb_MD_Subasta_Pago 



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

                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }

            }

            }

            return result;
        }

        public List<ExportFiles> generarTxt(string transacciones,int usuarioId)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaccion = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<ExportFiles> lista = new List<ExportFiles>();

                        if (transacciones == null || transacciones == "")
                        {
                            throw new Exception("Seleccione una Transaccion");
                        }

                        #region Parametros
                        var tranParam = new SqlParameter { ParameterName = "transaccion", Value = transacciones };
                        #endregion
                        List<ListaGenerarFormatoTxtResponse> listaOperaciones = new List<ListaGenerarFormatoTxtResponse>();

                        listaOperaciones = context.Database.SqlQuery<ListaGenerarFormatoTxtResponse>("exec Proc_Sel_Subasta_Generar_Txt @transaccion", tranParam).ToList<ListaGenerarFormatoTxtResponse>();

                        //Agrupacion por banco
                        var grupo = listaOperaciones.GroupBy(x =>  x.codigoBancoDestino).Select(gr => gr.ToList()).ToList();

                        grupo.ForEach(listaBanco => {

                            //Agrupacion por Moneda
                            listaBanco.GroupBy(y => y.tipoMoneda).Select(gm => gm.ToList()).ToList().ForEach(operacion => {

                                        //Recorrido por item
                                        operacion.ToList().ForEach(item =>
                                        {
                                               using (MemoryStream ms = new MemoryStream())
                                                {
                                              using (StreamWriter archivo = new StreamWriter(ms))
                                                {
                                                    string tipoPersona = "";
                                                    string moneda = "";
                                                    string enidadBancaria = "";
                                                    string texto = "";
                                                    if (item.tipoCliente == 1)
                                                    {
                                                        tipoPersona = "Persona";
                                                    }
                                                    else {
                                                        tipoPersona = "Empresa";
                                                    }
                                                    texto += "Numero de Operacion: " + item.idTransaccion + "\r\n";
                                                    texto += "Banco: " + item.bancoDestino.ToUpper() + "\r\n";
                                                    texto += "Tipo de Cuenta: " + item.tipoCuenta.ToUpper() + "\r\n";
                                                    texto += "Nro de Cuenta: " + item.cuentaDestino + "\r\n";
                                                    texto += "Moneda: " + item.tipoMoneda.ToUpper() + "\r\n";
                                                    texto += "Monto: " + item.monto + "\r\n";
                                                    texto += "Tipo de persona: " + tipoPersona.ToUpper() + "\r\n";
                                                    texto += "Nombre: " + item.usuario.ToUpper() + "\r\n";
                                                    texto += "Tipo Documento: " + item.tipoDocu + "\r\n";
                                                    texto += "Nro Documento: " + item.nroDocu;
                                                    moneda = item.tipoMoneda;
                                                    enidadBancaria = item.bancoDestino;


                                                    result.other = texto;
                                                  
                                                    archivo.WriteLine(texto);
                                                    archivo.Flush();
                                                    archivo.Close();
                                                    string dia = DateTime.Now.Day.ToString();
                                                    string mes = DateTime.Now.Month.ToString();
                                                    string hora = DateTime.Now.Hour.ToString();
                                                    string min = DateTime.Now.Minute.ToString();
                                                    string transacc = String.Format("{0:000000000}", item.idTransaccion);
                                                    
                                                    if (dia.Trim().Length == 1)
                                                    {
                                                        dia = "0" + dia;
                                                    }
                                                    if (mes.Trim().Length == 1)
                                                    {
                                                        mes = "0" + mes;
                                                    }
                                                    if (hora.Trim().Length == 1)
                                                    {
                                                        hora = "0" + hora;
                                                    }
                                                    if (min.Trim().Length == 1)
                                                    {
                                                        min = "0" + min;
                                                    }

                                                    string NombreArchivo = enidadBancaria.ToUpper() + "-" + moneda.ToUpper() + "-" + transacc + "-" + dia + "-" + mes + "-" + DateTime.Now.Year.ToString();
                                                  //+"-" + hora + "" + min + "" + DateTime.Now.Second.ToString() + "" + DateTime.Now.Millisecond.ToString();
                                                  
                                                    ExportFiles files = new ExportFiles();
                                                    files.Name = NombreArchivo;
                                                    files.Extension = "txt";
                                                    files.FileBytes = ms.ToArray();
                                                    lista.Add(files);

                                                    Tb_MD_Documentos documento = new Tb_MD_Documentos();
                                                    documento.vNombre = NombreArchivo;
                                                    documento.vExtension = files.Extension;
                                                    documento.vArchivo = files.FileBytes;
                                                    documento.vTipo = "OP";
                                                    documento.iEstadoRegistro = EstadoRegistroTabla.Activo;

                                                    context.Tb_MD_Documentos.Add(documento);
                                                    context.SaveChanges();

                                                    Tb_MD_DocOrdenPagoSubasta orden = new Tb_MD_DocOrdenPagoSubasta();
                                                    orden.nNumeroSubasta = item.idTransaccion;
                                                    orden.dFechaRegistro = DateTime.Now;
                                                    orden.vGeneradoPor = usuarioId;
                                                    orden.iEstadoRegistro = Convert.ToInt16(EstadoRegistroTabla.Activo);

                                                    documento.Tb_MD_DocOrdenPagoSubasta.Add(orden);
                                                  
                                                    context.SaveChanges();

                                                    
                                                   
                                                }


                                         } //-

                                        });

                                     
                                       
                            });

                        });


                        transaccion.Commit();
                        result.success = true;
                        return lista;

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

                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }

                }

            }

            return new List<ExportFiles>();
        }

        public ExportFiles descargarTxt(int idTransaccion) {
            ExportFiles file = new ExportFiles();
            try
            {
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    Tb_MD_Documentos documento = context.Tb_MD_DocOrdenPagoSubasta.Where(x => x.nNumeroSubasta == idTransaccion).FirstOrDefault().Tb_MD_Documentos;
                   

                    file.Name = documento.vNombre;
                    file.Extension = documento.vExtension;
                    file.FileBytes = documento.vArchivo;
                     //Name = x.,
                     //   Extension = x.vDesMoneda + " (" + x.vCodMoneda + ")",
                     //   FileBytes=
                }

                //result.success = true;  public string Name { get; set; }
        //public string Extension { get; set; }
        //public Byte[] FileBytes { get; set; }
            }
            catch (Exception ex)
            {
                //result.success = false;
                //result.ex = ex;
                //result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return file;
        
        }

        public PageResultSP<InstruccionPagoResponse> verListadoInstruccion(SubastaRequest param)
        {
            PageResultSP<InstruccionPagoResponse> valorRegistrados = new PageResultSP<InstruccionPagoResponse>();
            try
            {
                valorRegistrados.data = new List<InstruccionPagoResponse>();

               
                var idtransaccionParam = new SqlParameter { ParameterName = "IdTransaccion", Value = param.idTransaccion };
                
                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<InstruccionPagoResponse>("exec Proc_Sel_InstruccionPago @IdTransaccion", idtransaccionParam).ToList<InstruccionPagoResponse>();

                    foreach (var cliente in valorRegistrados.data)
                    {
                        string host = System.Configuration.ConfigurationManager.AppSettings["HostWeb"];
                        cliente.logoOrigen = host + cliente.logoOrigen;
                        cliente.logoDestino = host + cliente.logoDestino;

                        if (cliente.fecha != null)
                        {
                            cliente.fechaShort = cliente.fecha.Value.ToString("dd/MM/yyyy");
                        }
                    }
                    if (valorRegistrados.data.Count > 0)
                    {
               
                    }

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

        public PageResultSP<ListaAprobarPagoResponse> ListaAprobarPago(PageResultParam param)
        {
            PageResultSP<ListaAprobarPagoResponse> valorRegistrados = new PageResultSP<ListaAprobarPagoResponse>();

            try
            {
                valorRegistrados.data = new List<ListaAprobarPagoResponse>();
                
                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                #endregion


                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<ListaAprobarPagoResponse>("exec Proc_Sel_Aprobar_Pago @PageNumber,@ItemsPerPage", pageParam, itemsParam).ToList<ListaAprobarPagoResponse>();


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


        public BaseResponse<List<PartnerLiquidacionResponse>> generarLiquidacionExcel(int pagina,int cant, string numPartner,string numLiq,string estadoLiq)
        {
            BaseResponse<List<PartnerLiquidacionResponse>> result = new BaseResponse<List<PartnerLiquidacionResponse>>();
            result.data = new List<PartnerLiquidacionResponse>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                try
                {
                    result.data = new List<PartnerLiquidacionResponse>();
                    string estado = "";
                    if (estadoLiq == "9")
                    {
                        estado = "";
                    }
                    else
                    {
                        estado = estadoLiq;
                    }
                    #region Parametros
                    var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = pagina };
                    var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = cant };
                    var partnerParam = new SqlParameter { ParameterName = "numPartner", Value = numPartner };
                    var numliqParam = new SqlParameter { ParameterName = "numLiq", Value = numLiq };
                    var estadoParam = new SqlParameter { ParameterName = "estado", Value = estado };

                    #endregion

                    result.data = context.Database.SqlQuery<PartnerLiquidacionResponse>("exec Proc_Sel_LiquidacionPartner  @PageNumber,@ItemsPerPage,@numPartner,@numLiq,@estado", pageParam, itemsParam, partnerParam, numliqParam, estadoParam).ToList<PartnerLiquidacionResponse>();

                    result.success = true;
                    return result;

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

                    #endregion
                }
                catch (Exception ex)
                {
                    result.success = false;

                    result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }

            }

            return result;
        }

        //public PageResultSP<PartnerLiquidacionResponse> getListadoLiquidacionPartner(PageResultParam param, string numPartner)
        //{
        //    PageResultSP<PartnerLiquidacionResponse> result = new PageResultSP<PartnerLiquidacionResponse>();
        //    try
        //    {
        //        result.data = new List<PartnerLiquidacionResponse>();
        //        string idpartner = "";

        //        if (numPartner == "")
        //        {
        //            if (param.textFilter != null)
        //            {
        //                idpartner = param.textFilter;
        //            }

        //        }
        //        else
        //        {
        //            idpartner = numPartner;
        //        }

        //        int page = param.pageIndex + 1;
        //        #region Parametros
        //        var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
        //        var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
        //        var partnerParam = new SqlParameter { ParameterName = "numPartner", Value = idpartner };
        //        var numliqParam = new SqlParameter { ParameterName = "numLiq", Value = param.searchFilter };


        //        #endregion

        //        int total = 0;

        //        using (MesaDineroContext context = new MesaDineroContext())
        //        {
        //            result.data = context.Database.SqlQuery<PartnerLiquidacionResponse>("exec Proc_Sel_LiquidacionPartner @PageNumber,@ItemsPerPage,@numPartner,@numLiq", pageParam, itemsParam, partnerParam, numliqParam).ToList<PartnerLiquidacionResponse>();

        //            if (result.data.Count > 0)
        //            {
        //                total = Convert.ToInt32(result.data[0].total);
        //            }
        //        }

        //        #region Copiar Al Cual
        //        var pag = Utilities.ResultadoPagination(page, param.itemPerPage, total);

        //        result.itemperpage = pag.itemperpage;
        //        result.limit = pag.limit;
        //        result.numbersPages = pag.numbersPages;
        //        result.offset = pag.offset;
        //        result.page = pag.page;
        //        result.PageCount = pag.pageCount;
        //        result.total = pag.total;
        //        #endregion
        //        result.success = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        // copiar
        //        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        result.success = false;

        //    }

        //    return result;
        //}

       


    }
}
