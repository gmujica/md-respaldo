using MesaDinero.Data.PersistenceModel;
using MesaDinero.Domain;
using MesaDinero.Domain.DataAccess.Admin;
using MesaDinero.Domain.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace MesaDinero.Admin.Controllers.Api
{
    [RoutePrefix("api")]
    public class PartnerController : ApiBaseController
    {
        [HttpPost]
        [Route("partner/upload-file")]
        public IHttpActionResult UploadTipoCambioGarantizado()
        {
            BaseResponse<List<TipoCambioGarantizadoPartner_Response>> result = new BaseResponse<List<TipoCambioGarantizadoPartner_Response>>();
            result.data = new List<TipoCambioGarantizadoPartner_Response>();
            result.success = true;
            int fila = 8;
            try 
	        {
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                System.Web.HttpPostedFile file = files[0];

                using (ExcelPackage package = new ExcelPackage(file.InputStream))
                {
                    using (ExcelWorksheet worksheet = package.Workbook.Worksheets.First())
                    {
                        var dimension = worksheet.Dimension;

                        int total = dimension.End.Row + 1;


                        if (dimension == null)
                            throw new Exception("El archivo no tiene el formato correcto");

                        if (total > 17)
                            throw new Exception("Solo se puede registrar como maximo 10 Tipos de cambio garantizado.");

                        for (int i = fila; i < total; i++)
                        {
                            TipoCambioGarantizadoPartner_Response tipoCambio = new TipoCambioGarantizadoPartner_Response();

                            string sRango = worksheet.Cells[i, 2].Value.ToString() ?? string.Empty;
                            string sMontoMinimo = worksheet.Cells[i, 3].Value.ToString() ?? string.Empty;
                            string sMontoMaximo = worksheet.Cells[i, 4].Value.ToString() ?? string.Empty;
                            string sMoneda = worksheet.Cells[i, 5].Value.ToString() ?? string.Empty;
                            string sTcCompra = worksheet.Cells[i, 6].Value.ToString() ?? string.Empty;
                            string sTcVenta = worksheet.Cells[i, 7].Value.ToString() ?? string.Empty;
                            string sSpread = worksheet.Cells[i, 8].Value.ToString() ?? string.Empty;

                            try
                            {
                                tipoCambio.rango = Convert.ToDecimal(sRango);
                                tipoCambio.montoMinimo = Convert.ToDecimal(sMontoMinimo);
                                tipoCambio.montoMaximo = Convert.ToDecimal(sMontoMaximo);
                                tipoCambio.tcCompra = Convert.ToDecimal(sTcCompra);
                                tipoCambio.tcVenta = Convert.ToDecimal(sTcVenta);
                                tipoCambio.spread = Convert.ToDecimal(sSpread);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }


                            tipoCambio.moneda = sMoneda;


                            result.data.Add(tipoCambio);
                        }



                    }
                }

                Guid key = Guid.NewGuid();

  

	        }
	        catch (Exception ex)
	        {
                result.success = false;
                result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
	        }

            return Ok(result);
        }

        [HttpPost]
        [Route("partner/tipoCambio-delDia")]
        public IHttpActionResult GetTipoCambioDelDiaXPartner()
        {
            BaseResponse<List<TipoCambioGarantizadoPartner_Response>> result = new BaseResponse<List<TipoCambioGarantizadoPartner_Response>>();

            Domain.DataAccess.Admin.PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
            result = _partnerDataAccess.getAllTipoCambioGanantizadoDelDiaPartnert(NroRucEmpresaCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("partner-tipoCambioMercado")]
        public IHttpActionResult GetTipoCambioMercadoDelDiaXPartner(FiltroBanco_Response banco)
        {
            BaseResponse<List<TipoCambioMercadoPartner_Response>> result = new BaseResponse<List<TipoCambioMercadoPartner_Response>>();

            Domain.DataAccess.Admin.PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
            result = _partnerDataAccess.getAllTipoCambioMercadoDelDiaPartnert(banco.codBanco);

            return Ok(result);
        }

        [HttpPost]
        [Route("partner/save-tipoCambioDiario")]
        public IHttpActionResult SaveTipoCambioDiario(List<TipoCambioGarantizadoPartner_Response> model)
        {
            BaseResponse<string> result = new BaseResponse<string>();

            Domain.DataAccess.Admin.PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
            result = _partnerDataAccess.guardarTipoCambioDiaPartner(model, NroRucEmpresaCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("partner-subastas-activas")]
        public IHttpActionResult getAllSubastasActivas()
        {
            BaseResponse<List<SubastasActivasResponse>> result = new BaseResponse<List<SubastasActivasResponse>>();

            Domain.DataAccess.Admin.PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
            result = _partnerDataAccess.getSubastasActivas(NroRucEmpresaCurrenUser);

            return Ok(result);
        }

        [HttpPost]
        [Route("partner-update-tipocambio-subastaActiva")]
        public IHttpActionResult updateTipoCambio_SubastaActiva(dynamic param)
        {
            int subasta_ = param.subasta;
            decimal tipocambio_ = param.tipocambio;

            BaseResponse<string> result = new BaseResponse<string>();
            PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
            _partnerDataAccess.updateTipoCambioSubasta(subasta_, tipocambio_);


            return Ok(result);
        }

           [HttpPost]
        [Route("partner-datosListadoPartnertAll")]
        public IHttpActionResult getAllDatosListarPartner(PageResultParam model)
        {
            PageResultSP<ClienteOperacionesResponse> result = new PageResultSP<ClienteOperacionesResponse>();
            Domain.DataAccess.Admin.PartnerDataAccess _partnerDataAccess = new Domain.DataAccess.Admin.PartnerDataAccess();
            //NroRucEmpresaCurrenUser
            result = _partnerDataAccess.getListadoOperacionPartnerAll(model, NroRucEmpresaCurrenUser);


            return Ok(result);
        }

           [HttpPost]
           [Route("partner-mercado/upload-file")]
           public IHttpActionResult UploadTipoCambioMercado()
           {
               BaseResponse<List<TipoCambioMercadoPartner_Response>> result = new BaseResponse<List<TipoCambioMercadoPartner_Response>>();
               result.data = new List<TipoCambioMercadoPartner_Response>();
               result.success = true;
               int fila = 8;
               try
               {
                   System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                   System.Web.HttpPostedFile file = files[0];

                   using (ExcelPackage package = new ExcelPackage(file.InputStream))
                   {
                       using (ExcelWorksheet worksheet = package.Workbook.Worksheets.First())
                       {
                           var dimension = worksheet.Dimension;

                           int total = dimension.End.Row + 1;


                           if (dimension == null)
                               throw new Exception("El archivo no tiene el formato correcto");

                           //if (total > 17)
                           //    throw new Exception("Solo se puede registrar como maximo 10 Tipos de cambio garantizado.");

                           for (int i = fila; i < total; i++)
                           {
                               TipoCambioMercadoPartner_Response tipoCambio = new TipoCambioMercadoPartner_Response();
                               string ruta = "";
                              

                               string sProveedor = worksheet.Cells[i, 2].Value.ToString() ?? string.Empty;
                               string sRango = worksheet.Cells[i, 3].Value.ToString() ?? string.Empty;
                               string sMontoMinimo = worksheet.Cells[i, 4].Value.ToString() ?? string.Empty;
                               string sMontoMaximo = worksheet.Cells[i, 5].Value.ToString() ?? string.Empty;
                               string sMoneda = worksheet.Cells[i, 6].Value.ToString() ?? string.Empty;
                               string sTcCompra = worksheet.Cells[i, 7].Value.ToString() ?? string.Empty;
                               string sTcVenta = worksheet.Cells[i, 8].Value.ToString() ?? string.Empty;
                               string sSpread = worksheet.Cells[i, 9].Value.ToString() ?? string.Empty;

                               Tb_MD_Entidades_Financieras banco = null;
                               using (MesaDineroContext context = new MesaDineroContext())
                               {
                                   banco = context.Tb_MD_Entidades_Financieras.Find(sProveedor);
                               }
                               ruta = banco.vLogoEntidad;

                               try
                               {
                                   tipoCambio.logo = ruta;
                                   tipoCambio.proveedor = sProveedor;
                                   tipoCambio.rango = Convert.ToDecimal(sRango);
                                   tipoCambio.montoMinimo = Convert.ToDecimal(sMontoMinimo);
                                   tipoCambio.montoMaximo = Convert.ToDecimal(sMontoMaximo);
                                   tipoCambio.tcCompra = Convert.ToDecimal(sTcCompra);
                                   tipoCambio.tcVenta = Convert.ToDecimal(sTcVenta);
                                   tipoCambio.spread = Convert.ToDecimal(sSpread);
                               }
                               catch (Exception ex)
                               {
                                   throw new Exception(ex.Message);
                               }


                               tipoCambio.moneda = sMoneda;


                               result.data.Add(tipoCambio);
                           }



                       }
                   }

                   Guid key = Guid.NewGuid();



               }
               catch (Exception ex)
               {
                   result.success = false;
                   result.error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
               }

               return Ok(result);
           }

           [HttpPost]
           [Route("partner/save-tipoMercadoDiario")]
           public IHttpActionResult SaveTipoMercadoDiario(List<TipoCambioMercadoPartner_Response> model)
           {
               BaseResponse<string> result = new BaseResponse<string>();

               Domain.DataAccess.Admin.PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
               result = _partnerDataAccess.guardarTipoCambioMercadoPartner(model, NroRucEmpresaCurrenUser);

               return Ok(result);
           }


        [Route("partner/lista-adjudicaciones")]
        [HttpPost]
        public IHttpActionResult getListaAdjudicaciones(PageResultParam model)
        {
            PartnerDataAccess _partnerDataAccess = new PartnerDataAccess();
            PageResultSP<PartnerListaAdjudicacionResponse> result = new PageResultSP<PartnerListaAdjudicacionResponse>();
            result = _partnerDataAccess.getListadoAdjudicacionPartner(model, NroRucEmpresaCurrenUser);
            return Ok(result);
        }





    }
}
