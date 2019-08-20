using MesaDinero.Domain;
using MesaDinero.Domain.DataAccess;
using MesaDinero.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text;
namespace MesaDinero.Admin.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    public class CorfidController : Controller
    {
        // GET: Corfid
        public ActionResult ListaOperacionesHistoricas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DescargarExcel(string transacciones)
        {
            CorfidDataAcces _corfidDataAccess=new CorfidDataAcces();
            BaseResponse<List<OperacionesHistoricas>> result=new BaseResponse<List<OperacionesHistoricas>>();
            result=_corfidDataAccess.generarExcel(transacciones);
            if (result.success)
            {


                string plantilla = System.Configuration.ConfigurationManager.AppSettings["operacionesHistoricas"];
                XSSFWorkbook hssfworkbook;
                string path = AppDomain.CurrentDomain.BaseDirectory + plantilla;


                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new XSSFWorkbook(file);
                }

                ISheet sheet1 = hssfworkbook.GetSheet("Reporte");
                IRow row = sheet1.GetRow(7);


                int i = 7;

                //List<OperacionesHistoricas> operacionesHistoricas;


                foreach (OperacionesHistoricas item in result.data)
                {
                    row = sheet1.GetRow(i);
                    if (row == null) row = sheet1.CreateRow(i);

                    this.SetCellValue(hssfworkbook, row, 1, item.fecha);
                    this.SetCellValue(hssfworkbook, row, 2, item.idTransaccion);
                    this.SetCellValue(hssfworkbook, row, 3, item.partnersAdjuntado);
                    this.SetCellValue(hssfworkbook, row, 4, item.usuario);
                    this.SetCellValue(hssfworkbook, row, 5, item.quiere== "V" ? "Compra" : "Venta");
                    //this.SetCellValue(hssfworkbook, row, 6, item.tipoMoneda);
                    this.SetCellValue(hssfworkbook, row, 6, item.quiere == "V" ? item.monto.ToString(): item.montoRecibe.ToString());//Monto en Dolares
                    this.SetCellValue(hssfworkbook, row, 7, item.precioPactado);
                    this.SetCellValue(hssfworkbook, row, 8, item.quiere == "V" ? item.montoRecibe.ToString() : item.monto.ToString());//Monto en Soles
                    this.SetCellValue(hssfworkbook, row, 9, item.estadoSubasta);
                    i++;
                }


                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "operacionesHistoricas.xlsx");

            }
            else {
                return RedirectToAction("ListaOperacionesHistoricas");
            }
        }

   


        #region FormatoExcel

        private ICellStyle normalRowStyle;

        private ICellStyle GetNormalRowStyle(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyle == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.FontName = "Calibri";
                normalRowStyle = hssfworkbook.CreateCellStyle();
                normalRowStyle.SetFont(font);
                normalRowStyle.BorderTop = BorderStyle.Thin;
                normalRowStyle.BorderRight = BorderStyle.Thin;
                normalRowStyle.BorderBottom = BorderStyle.Thin;
                normalRowStyle.BorderLeft = BorderStyle.Thin;
            }
            return normalRowStyle;
        }

        private ICellStyle numericRowStyle;
        private ICellStyle GetNumericRowStyle(XSSFWorkbook hssfworkbook)
        {
            if (numericRowStyle == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.FontName = "Calibri";
                numericRowStyle = hssfworkbook.CreateCellStyle();
                numericRowStyle.SetFont(font);
                numericRowStyle.BorderTop = BorderStyle.Thin;
                numericRowStyle.BorderRight = BorderStyle.Thin;
                numericRowStyle.BorderBottom = BorderStyle.Thin;
                numericRowStyle.BorderLeft = BorderStyle.Thin;
                numericRowStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("#,#0.00");
            }
            return numericRowStyle;
        }

        private ICellStyle dateRowStyle;
        private ICellStyle GetDateRowStyle(XSSFWorkbook hssfworkbook)
        {
            if (dateRowStyle == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.FontName = "Calibri";
                dateRowStyle = hssfworkbook.CreateCellStyle();
                dateRowStyle.SetFont(font);
                dateRowStyle.BorderTop = BorderStyle.Thin;
                dateRowStyle.BorderRight = BorderStyle.Thin;
                dateRowStyle.BorderBottom = BorderStyle.Thin;
                dateRowStyle.BorderLeft = BorderStyle.Thin;
                dateRowStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("dd/mm/yyyy");
            }
            return dateRowStyle;
        }


        private void SetCellValue(XSSFWorkbook hssfworkbook, IRow row, int i, bool? column)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyle(hssfworkbook);

            if (column.HasValue)
            {
                cell.SetCellValue(column.Value);
            }
        }

        private void SetCellValue(XSSFWorkbook hssfworkbook, IRow row, int i, int? column)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyle(hssfworkbook);

            if (column.HasValue)
            {
                cell.SetCellValue((double)column.Value);
            }
        }
        private void SetCellValue(XSSFWorkbook hssfworkbook, IRow row, int i, decimal? column)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);

            if (column.HasValue)
            {
                cell.CellStyle = GetNumericRowStyle(hssfworkbook);
                cell.SetCellValue((double)column.Value);
            }
            else
            {
                cell.CellStyle = GetNormalRowStyle(hssfworkbook);
            }
        }

        private void SetCellValue(XSSFWorkbook hssfworkbook, IRow row, int i, DateTime? column)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);

            if (column.HasValue)
            {
                cell.CellStyle = GetDateRowStyle(hssfworkbook);
                cell.SetCellValue(column.Value);
            }
            else
            {
                cell.CellStyle = GetNormalRowStyle(hssfworkbook);
            }
        }

        private void SetCellValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyle(hssfworkbook);

            cell.SetCellValue(value);
        }


        #endregion




    }
}