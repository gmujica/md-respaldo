using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain.Helper;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesaDinero.Domain.DataAccess.Admin
{
    public partial class  PartnerDataAccess
    {

        public BaseResponse<List<TipoCambioGarantizadoPartner_Response>> getAllTipoCambioGanantizadoDelDiaPartnert(string empresa)
        {
            BaseResponse<List<TipoCambioGarantizadoPartner_Response>> result = new BaseResponse<List<TipoCambioGarantizadoPartner_Response>>();

            try
            {

                #region Parametros
                var partnerParam = new SqlParameter { ParameterName = "partner", Value = empresa };
                #endregion

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<TipoCambioGarantizadoPartner_Response>("exec Proc_Sel_tipoCambioGarantizadoActual_Partner @partner", partnerParam).ToList<TipoCambioGarantizadoPartner_Response>();
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;
            }

            return result;
        }

        public BaseResponse<string> guardarTipoCambioDiaPartner(List<TipoCambioGarantizadoPartner_Response> model, string empresa)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        List<Tb_MD_Tipo_Cambio_Garantizado> oldRegistros = new List<Tb_MD_Tipo_Cambio_Garantizado>();
                        oldRegistros = context.Tb_MD_Tipo_Cambio_Garantizado.Where(x => x.vNumDocPartner.Equals(empresa) && x.UltimoCambioGarantizado == true).ToList();
                        foreach (var o in oldRegistros)
                        {
                            o.UltimoCambioGarantizado = false;
                        }

                        DateTime fecha = DateTime.Now;
                        foreach (var m in model)
                        {

                            if (m.montoMaximo <= 0)
                                break;



                            Tb_MD_Tipo_Cambio_Garantizado tcambio = new Tb_MD_Tipo_Cambio_Garantizado();
                            tcambio.vNumDocPartner = empresa;
                            tcambio.dFechaCreacion = fecha;
                            tcambio.nRango1_n = m.rango;
                            tcambio.nValorRangoMinimo = m.montoMinimo;
                            tcambio.nValorRangoMaximo = m.montoMaximo;
                            tcambio.vTipoMoneda = m.moneda;
                            tcambio.nValorCompra = m.tcCompra;
                            tcambio.nValorVenta = m.tcVenta;
                            tcambio.UltimoCambioGarantizado = true;

                            context.Tb_MD_Tipo_Cambio_Garantizado.Add(tcambio);
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

        public BaseResponse<List<SubastasActivasResponse>> getSubastasActivas(string partner)
        {
            BaseResponse<List<SubastasActivasResponse>> result = new BaseResponse<List<SubastasActivasResponse>>();
            result.data = new List<SubastasActivasResponse>();

            try
            {

                #region Parametros
                var partnerParam = new SqlParameter { ParameterName = "partner", Value = partner };
                #endregion

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<SubastasActivasResponse>("exec Proc_Sel_SubastasActivas_Partner @partner", partnerParam).ToList<SubastasActivasResponse>();
                }

                List<int> ids = result.data.Select(x => x.codigo).ToList();

                result.other = string.Join(",",ids);

                result.success = true;
            

            }
            catch (Exception ex)
            {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return result;
        }

        public BaseResponse<string> updateTipoCambioSubasta(int subasta, decimal tipoCambio)
        {
            BaseResponse<string> result = new BaseResponse<string>();
            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Tb_MD_Subasta_Detalle detalle = null;
                        detalle = context.Tb_MD_Subasta_Detalle.FirstOrDefault(x => x.iIdSubastaDEtalle == subasta);
                        if (detalle == null)
                            throw new Exception("Error Nulll, hay un problema con la subasta.");

                        result.success = true;

                        if(detalle.vTipoDetalle == "V")
                        {
                            detalle.nValorCompra = tipoCambio;
                        }
                        else
                        {
                            detalle.nValorVenta = tipoCambio;
                        }
                        detalle.TipoCambio = tipoCambio;


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
                        result.ex = ex;
                        transaction.Rollback();
                        result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }

        public PageResultSP<ClienteOperacionesResponse> getListadoOperacionPartnerAll(PageResultParam param,string rucEmpresa)
        {
            PageResultSP<ClienteOperacionesResponse> result = new PageResultSP<ClienteOperacionesResponse>();
            try
            {
                result.data = new List<ClienteOperacionesResponse>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };

                var numParam = new SqlParameter { ParameterName = "vNumDocumento", Value = rucEmpresa };
                var tipoParam = new SqlParameter { ParameterName = "vTipoFiltro", Value = param.textFilter };
                #endregion

                
                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    //result.data = context.Database.SqlQuery<ClienteOperacionesResponse>("exec Proc_Sel_OperacionesPartner @PageNumber,@ItemsPerPage,@vNumDocumento,@vTipoFiltro", pageParam, itemsParam, numParam, tipoParam).ToList<ClienteOperacionesResponse>();
                    result.data = context.Database.SqlQuery<ClienteOperacionesResponse>("exec Proc_Sel_OperacionesPartner @PageNumber,@ItemsPerPage,@vNumDocumento,@vTipoFiltro", pageParam, itemsParam, numParam, tipoParam).ToList<ClienteOperacionesResponse>();
                   
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


        public BaseResponse<string> guardarTipoCambioMercadoPartner(List<TipoCambioMercadoPartner_Response> model,string empresa)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        DateTime fecha = DateTime.Now;
                        foreach (var m in model)
                        {

                            List<Tb_MD_Tipo_Cambio_Mercado> oldRegistros = new List<Tb_MD_Tipo_Cambio_Mercado>();
                            oldRegistros = context.Tb_MD_Tipo_Cambio_Mercado.Where(x => x.vCodBanco.Equals(m.proveedor) && x.vTipoMoneda.Equals(m.moneda) && 
                                x.UltimoCambioMercado == true).ToList();
                            foreach (var o in oldRegistros)
                            {
                                o.UltimoCambioMercado = false;
                            }

                            Tb_MD_Tipo_Cambio_Mercado tcambio = new Tb_MD_Tipo_Cambio_Mercado();
                            tcambio.vCodBanco = m.proveedor;
                            tcambio.dFechaCreacion = fecha;
                            tcambio.nRango1_n = m.rango;
                            tcambio.nValorRangoMinimo = m.montoMinimo;
                            tcambio.nValorRangoMaximo = m.montoMaximo;
                            tcambio.vTipoMoneda = m.moneda;
                            tcambio.nValorCompra = m.tcCompra;
                            tcambio.nValorVenta = m.tcVenta;
                            tcambio.VRUCUsuario = empresa;
                            tcambio.UltimoCambioMercado = true;

                            context.Tb_MD_Tipo_Cambio_Mercado.Add(tcambio);
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

        public BaseResponse<List<TipoCambioMercadoPartner_Response>> getAllTipoCambioMercadoDelDiaPartnert(string empresa)
        {
            BaseResponse<List<TipoCambioMercadoPartner_Response>> result = new BaseResponse<List<TipoCambioMercadoPartner_Response>>();

            try
            {

                #region Parametros
                var partnerParam = new SqlParameter { ParameterName = "banco", Value = empresa };
                #endregion

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<TipoCambioMercadoPartner_Response>("exec Proc_Sel_tipoCambioMercadoActual_Partner @banco", partnerParam).ToList<TipoCambioMercadoPartner_Response>();
                }

                result.success = true;
            }
            catch (Exception ex)
            {
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                result.success = false;
            }

            return result;
        }

        public PageResultSP<PartnerListaAdjudicacionResponse> getListadoAdjudicacionPartner(PageResultParam param, string numPartner)
        {
            PageResultSP<PartnerListaAdjudicacionResponse> result = new PageResultSP<PartnerListaAdjudicacionResponse>();
            try
            {
                result.data = new List<PartnerListaAdjudicacionResponse>();
                string idpartner = "";
               

              
                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                var partnerParam = new SqlParameter { ParameterName = "numPartner", Value = numPartner };
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    result.data = context.Database.SqlQuery<PartnerListaAdjudicacionResponse>("exec Proc_Sel_Lista_Adjudicaciones @PageNumber,@ItemsPerPage,@numPartner", pageParam, itemsParam, partnerParam).ToList<PartnerListaAdjudicacionResponse>();

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


        public BaseResponse<List<PartnerListaAdjudicacionResponse>> ListaAdjudicacionExcel(string numPartner,string operaciones)
        {
            BaseResponse<List<PartnerListaAdjudicacionResponse>> result = new BaseResponse<List<PartnerListaAdjudicacionResponse>>();
            result.data = new List<PartnerListaAdjudicacionResponse>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                try
                {
                    result.data = new List<PartnerListaAdjudicacionResponse>();
                 
                    #region Parametros
                    var partnerParam = new SqlParameter { ParameterName = "numPartner", Value = numPartner };
                    var codigoParam = new SqlParameter { ParameterName = "Parametros", Value = operaciones };

                    #endregion

                    result.data = context.Database.SqlQuery<PartnerListaAdjudicacionResponse>("exec Proc_Sel_Lista_Adjudicaciones_excel @numPartner,@Parametros", partnerParam, codigoParam).ToList<PartnerListaAdjudicacionResponse>();

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


    }
}
