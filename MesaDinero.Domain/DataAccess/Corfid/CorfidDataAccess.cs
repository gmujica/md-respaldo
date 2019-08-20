using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesaDinero.Domain.Helper;
using System.Data.SqlClient;
using MesaDinero.Data.PersistenceModel;

namespace MesaDinero.Domain.DataAccess
{
    public class CorfidDataAcces
    {

        public PageResultSP<OperacionesHistoricas> getOperacionesHistoricas(PageResultParam param)
        {
            PageResultSP<OperacionesHistoricas> valorRegistrados = new PageResultSP<OperacionesHistoricas>();

            try
            {
                valorRegistrados.data = new List<OperacionesHistoricas>();

                int page = param.pageIndex + 1;
                #region Parametros
                var pageParam = new SqlParameter { ParameterName = "PageNumber", Value = page };
                var itemsParam = new SqlParameter { ParameterName = "ItemsPerPage", Value = param.itemPerPage };
                var filtroParam = new SqlParameter { ParameterName = "vTipoFiltro", Value = param.textFilter };
                #endregion

                int total = 0;

                using (MesaDineroContext context = new MesaDineroContext())
                {
                    valorRegistrados.data = context.Database.SqlQuery<OperacionesHistoricas>("exec Proc_Sel_OperacionesHistoricas @PageNumber,@ItemsPerPage,@vTipoFiltro", pageParam, itemsParam, filtroParam).ToList<OperacionesHistoricas>();

                  
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

        public BaseResponse<List<OperacionesHistoricas>> generarExcel(string transacciones)
        {
            BaseResponse<List<OperacionesHistoricas>> result = new BaseResponse<List<OperacionesHistoricas>>();
            result.data = new List<OperacionesHistoricas>();

            using (MesaDineroContext context = new MesaDineroContext())
            {
                    try
                    {

                        if (transacciones == null || transacciones == "")
                        {
                            throw new Exception("Seleccione una Transaccion");
                        }

                        #region Parametros
                        var tranParam = new SqlParameter { ParameterName = "IdTrasaccion", Value = transacciones };
                        #endregion

                        result.data = context.Database.SqlQuery<OperacionesHistoricas>("exec Proc_Sel_OperacionesHistoricas_excel @IdTrasaccion", tranParam).ToList<OperacionesHistoricas>();

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
