using MesaDinero.Domain;
using MesaDinero.Domain.DataAccess.operaciones;
using MesaDinero.Domain.Model.operaciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using System.Text;
using MesaDinero.Domain.Model;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text;
using System.Globalization;

namespace MesaDinero.Admin.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    public class OperadorController : BaseController
    {
        //
        // GET: /Operador/
        public ActionResult Index()
        {
            //if (RolCurrenUser != "Operador")
            //    throw new HttpException(404, "No tienes permiso para acceder a esta sección");

            return View();
        }


        public ActionResult OperacionesConfirmadas()
        {
            return View();
        }

        public ActionResult VerificacionPago()
        {
            return View();
        }

        public ActionResult VerificacionPagoFideicomiso()
        {
            return View();
        }

        public ActionResult GenerarPagos()
        {
            return View();
        }

        public ActionResult ListaPartners()
        {
            return View();
        }
        
        public ActionResult LiquidacionPartner()
        {
            return View();
        }

       
        public ActionResult LiquidacionVerificaPartner()
        {
            return View();
        }

    
        public ActionResult LiquidacionPagaPartner()
        {
            return View();
        }
       
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DescargarExportableTXT(string transacciones)
        {
            OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
            BaseResponse<string> result = new BaseResponse<string>();
            List<ExportFiles> listaFiles = new List<ExportFiles>();

            string[] codigoTransaccion = transacciones.Split(',');



            listaFiles = _operadorDataAccess.generarTxt(transacciones, IdCurrenUser);

            if (listaFiles.Count > 1)
            {
                byte[] byteArray = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (ExportFiles f in listaFiles)
                        {
                            ZipArchiveEntry zipItem = zip.CreateEntry(f.Name + "." + f.Extension);
                            using (MemoryStream originalFileMemoryStream = new MemoryStream(f.FileBytes))
                            {
                                using (Stream entryStream = zipItem.Open())
                                {
                                    originalFileMemoryStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }
                    byteArray = memoryStream.ToArray();
                }
                string dia = DateTime.Now.Day.ToString();
                string mes = DateTime.Now.Month.ToString();
                string hora = DateTime.Now.Hour.ToString();
                string min = DateTime.Now.Minute.ToString();

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

                String NombreArchivo = "Transaccion-" + dia + "-" + mes + "-" + DateTime.Now.Year.ToString() + "-" + hora + "" + min + "" + DateTime.Now.Second.ToString() + "" + DateTime.Now.Millisecond.ToString();

                Response.AddHeader("Content-Disposition", "attachment; filename=" + NombreArchivo + ".zip");
                //return RedirectToAction("GenerarPagos");
                return File(byteArray, "application/zip");

            }
            else {
                if (listaFiles.Count != 0)
                {
                    byte[] byteArray = listaFiles[0].FileBytes;
                    String NombreArchivo = listaFiles[0].Name.ToString();
                    String ExtensionArchivo = listaFiles[0].Extension.ToString();
                    //MemoryStream ms=new MemoryStream(byteArray,false);
                    return File(byteArray, "text/" + ExtensionArchivo, NombreArchivo + "." + ExtensionArchivo);
                }
                else {
                    return RedirectToAction("GenerarPagos");
                }
            }
        }

        [AllowAnonymous]
        public FileResult Descargar(string trasaccion)
        {
            // Obtener contenido del archivo
            //string text = "El texto para mi archivo.";
            //var stream = new MemoryStream(Encoding.ASCII.GetBytes(text));

            //return File(stream, "text/plain", "Prueba.txt");
            OperacionDataAccess _operadorDataAccess = new OperacionDataAccess();
            ExportFiles file = new ExportFiles();
            file=_operadorDataAccess.descargarTxt(Convert.ToInt16( trasaccion));

            byte[] byteArray = file.FileBytes;
            String NombreArchivo = file.Name.ToString();
            String ExtensionArchivo = file.Extension.ToString();
            //MemoryStream ms = new MemoryStream(byteArray,false);

            return File(byteArray, "text/" + ExtensionArchivo, NombreArchivo + "." + ExtensionArchivo);
        }
        
        public ActionResult AprobarPagos() {
            return View();
                
        }

        public ActionResult ListaClienteRegistrados()
        {
            return View();
        }

        public FileResult DescargarFicha(string ruc)
        {
           
            MesaDinero.Domain.DataAccess.RegistroCliente _clienteDataAccess = new MesaDinero.Domain.DataAccess.RegistroCliente();
            MesaDinero.Domain.Model.operaciones.ExportFiles file = new MesaDinero.Domain.Model.operaciones.ExportFiles();
            file = _clienteDataAccess.descargarDocumento(ruc);

            byte[] byteArray = file.FileBytes;
            String NombreArchivo = file.Name.ToString();
            String ExtensionArchivo = file.Extension.ToString();

            return File(byteArray, "text/" + ExtensionArchivo, NombreArchivo + ExtensionArchivo);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DescargarLiquidacionExcel(string numPartner, string numLiq, string estadoLiq, int tipo)
        {
            OperacionDataAccess _operacionDataAccess = new OperacionDataAccess();
            BaseResponse<List<PartnerLiquidacionResponse>> result = new BaseResponse<List<PartnerLiquidacionResponse>>();

            if (numPartner == null)
            {
                numPartner = "";
            }
            if (tipo == 1)
            {
                numPartner = NroRucEmpresaCurrenUser;
            }
            result = _operacionDataAccess.generarLiquidacionExcel(1, 100, numPartner, numLiq, estadoLiq);
            if (result.success)
            {
                string plantilla = System.Configuration.ConfigurationManager.AppSettings["LiquidacionPartner"];
                XSSFWorkbook hssfworkbook;
                string path = AppDomain.CurrentDomain.BaseDirectory + plantilla;


                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new XSSFWorkbook(file);
                }

                ISheet sheet1 = hssfworkbook.GetSheet("Reporte");
                IRow row = sheet1.GetRow(7);


                int i = 19;
                decimal totalUsdCompra = 0;
                decimal totalSolesCompra = 0;

                decimal totalUsdVenta = 0;
                decimal totalSolesVenta = 0;

                decimal totalDolares = 0;
                decimal totalSoles = 0;

                string accionUSD = "";
                string accionPEN = "";

                string numliquidacion = "";
                DateTime fechaPrincipal = DateTime.Now;
                //List<OperacionesHistoricas> operacionesHistoricas;

                CultureInfo culture;
                string specifier;
                specifier = "N2";

                string specifier4;
                specifier4 = "N4";
                culture = CultureInfo.CreateSpecificCulture("eu-ES");

                row = sheet1.GetRow(11);
                if (row == null) row = sheet1.CreateRow(11);
                this.SetCellAumentValue(hssfworkbook, row, 5, result.data[0].partnersAdjuntado.ToString());

                //row = sheet1.GetRow(15);
                //if (row == null) row = sheet1.CreateRow(15);
                //this.SetCellGrisMontoValue(hssfworkbook, row, 4, accionUSD);
                //this.SetCellAzulValue(hssfworkbook, row, 5, totalDolares.ToString(specifier, CultureInfo.InvariantCulture));



                //fechaPrincipal
                result.data.Where(y => y.quiere == "Compra").ToList().ForEach(x =>
                {
                    row = sheet1.GetRow(i);
                    if (row == null) row = sheet1.CreateRow(i);
                    this.SetCellValue(hssfworkbook, row, 1, x.idTransaccion);
                    this.SetCellCenterValue(hssfworkbook, row, 2, x.fecha.Value.ToString("dd/mm/yyyy"));
                    this.SetCellCenterValue(hssfworkbook, row, 3, x.fecha.Value.ToString("HH:mm"));
                    this.SetCellValue(hssfworkbook, row, 4, x.cliente);
                    this.SetCellCenterValue(hssfworkbook, row, 5, x.quiere);
                    this.SetCellValue(hssfworkbook, row, 6, x.montoUSDCompraText);
                    this.SetCellValue(hssfworkbook, row, 7, x.cambioCompraText);
                    this.SetCellValue(hssfworkbook, row, 8, x.montoSolesCompraText);
                    if (x.fechaLiquidacion.HasValue)
                    {
                        fechaPrincipal = x.fechaLiquidacion.Value;
                    }
                    totalUsdCompra += x.montoUSDCompra;
                    totalSolesCompra += x.montoSolesCompra;
                    numliquidacion = x.numLiquidacion;
                    numPartner = x.numPartner;
                    i++;
                });

                row = sheet1.GetRow(i);
                if (row == null) row = sheet1.CreateRow(i);

                row = sheet1.GetRow(i + 1);
                if (row == null) row = sheet1.CreateRow(i + 1);
                this.SetCellNegritaValue(hssfworkbook, row, 5, "Partner envía");
                this.SetCellValue(hssfworkbook, row, 6, totalUsdCompra.ToString(specifier, CultureInfo.InvariantCulture));
                this.SetCellNegritaValue(hssfworkbook, row, 7, "Partner recibe");
                this.SetCellValue(hssfworkbook, row, 8, totalSolesCompra.ToString(specifier, CultureInfo.InvariantCulture));

                row = sheet1.GetRow(i + 3);
                if (row == null) row = sheet1.CreateRow(i + 3);
                row.Height = 600;

                this.SetCellHeaderValue(hssfworkbook, row, 1, "ID Transaccion");
                this.SetCellHeaderValue(hssfworkbook, row, 2, "Fecha Adjudicacion");
                this.SetCellHeaderValue(hssfworkbook, row, 3, "Hora");
                this.SetCellHeaderValue(hssfworkbook, row, 4, "Nombre Cliente");
                this.SetCellHeaderValue(hssfworkbook, row, 5, "Cliente");
                this.SetCellHeaderValue(hssfworkbook, row, 6, "Monto DOL");
                this.SetCellHeaderValue(hssfworkbook, row, 7, "Tip de Cambio ");
                this.SetCellHeaderValue(hssfworkbook, row, 8, "Monto SOL");

                i = i + 4;

                result.data.Where(y => y.quiere == "Venta").ToList().ForEach(x =>
                {
                    row = sheet1.GetRow(i);
                    if (row == null) row = sheet1.CreateRow(i);
                    this.SetCellValue(hssfworkbook, row, 1, x.idTransaccion);
                    this.SetCellCenterValue(hssfworkbook, row, 2, x.fecha.Value.ToString("dd/mm/yyyy"));
                    this.SetCellCenterValue(hssfworkbook, row, 3, x.fecha.Value.ToString("HH:mm"));
                    this.SetCellValue(hssfworkbook, row, 4, x.cliente);
                    this.SetCellCenterValue(hssfworkbook, row, 5, x.quiere);
                    this.SetCellValue(hssfworkbook, row, 6, x.montoUSDventaText);
                    this.SetCellValue(hssfworkbook, row, 7, x.cambioVentaText);
                    this.SetCellValue(hssfworkbook, row, 8, x.montoSolesventaText);
                    if (x.fechaLiquidacion.HasValue)
                    {
                        fechaPrincipal = x.fechaLiquidacion.Value;
                    }
                    totalUsdVenta += x.montoUSDventa;
                    totalSolesVenta += x.montoSolesventa;
                    numliquidacion = x.numLiquidacion;
                    numPartner = x.numPartner;
                    i++;
                });

                row = sheet1.GetRow(i);
                if (row == null) row = sheet1.CreateRow(i);

                row = sheet1.GetRow(i + 1);
                if (row == null) row = sheet1.CreateRow(i + 1);
                this.SetCellNegritaValue(hssfworkbook, row, 5, "Partner Recibe");
                this.SetCellValue(hssfworkbook, row, 6, totalUsdVenta.ToString(specifier, CultureInfo.InvariantCulture));
                this.SetCellNegritaValue(hssfworkbook, row, 7, "Partner envía");
                this.SetCellValue(hssfworkbook, row, 8, totalSolesVenta.ToString(specifier, CultureInfo.InvariantCulture));



                if (totalUsdCompra > totalUsdVenta)
                {
                    totalDolares = totalUsdCompra - totalUsdVenta;
                    accionUSD = "Envia USD:";
                }

                if (totalUsdVenta > totalUsdCompra)
                {
                    totalDolares = totalUsdVenta - totalUsdCompra;
                    accionUSD = "Recibe USD:";
                }


                if (totalSolesCompra > totalSolesVenta)
                {
                    totalSoles = totalSolesCompra - totalSolesVenta;
                    accionPEN = "Recibe PEN:";
                }

                if (totalSolesVenta > totalSolesCompra)
                {
                    totalSoles = totalSolesVenta - totalSolesCompra;
                    accionPEN = "Envia PEN:";
                }

                row = sheet1.GetRow(15);
                if (row == null) row = sheet1.CreateRow(15);
                this.SetCellGrisMontoValue(hssfworkbook, row, 4, accionUSD);
                this.SetCellAzulValue(hssfworkbook, row, 5, totalDolares.ToString(specifier, CultureInfo.InvariantCulture));

                row = sheet1.GetRow(16);
                if (row == null) row = sheet1.CreateRow(16);
                this.SetCellGrisMontoValue(hssfworkbook, row, 4, accionPEN);
                this.SetCellAzulValue(hssfworkbook, row, 5, totalSoles.ToString(specifier, CultureInfo.InvariantCulture));

                row = sheet1.GetRow(8);
                if (row == null) row = sheet1.CreateRow(8);
                this.SetCellGrisValue(hssfworkbook, row, 1, "Código de la liquidación " + numliquidacion);

                fechaPrincipal = DateTime.Now;
                row = sheet1.GetRow(9);
                if (row == null) row = sheet1.CreateRow(9);
                this.SetCellGrisValue(hssfworkbook, row, 1, "Fecha:" + fechaPrincipal.ToString("d MMMM") + " " + fechaPrincipal.Year.ToString() + " Hora: " + fechaPrincipal.ToString("hh:mm:ss tt", new CultureInfo("en-US")));

                //fechaPrincipal Fecha: 25 Abril 2019 Hora: 13:00 pm
                //
                row = sheet1.GetRow(12);
                if (row == null) row = sheet1.CreateRow(12);
                this.SetCellAumentValue(hssfworkbook, row, 5, numPartner);


                MemoryStream stream = new MemoryStream();
                hssfworkbook.Write(stream);

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LiquidacionPartner.xlsx");

            }
            else
            {
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
                font.FontName = "Verdana";
                normalRowStyle = hssfworkbook.CreateCellStyle();
                normalRowStyle.SetFont(font);

                normalRowStyle.BorderTop = BorderStyle.None;
                normalRowStyle.BorderRight = BorderStyle.None;
                normalRowStyle.BorderBottom = BorderStyle.None;
                normalRowStyle.BorderLeft = BorderStyle.None;
            }
            return normalRowStyle;
        }

        private ICellStyle normalRowCenterStyle;

        private ICellStyle GetNormalCenterRowStyle(XSSFWorkbook hssfworkbook)
        {
            if (normalRowCenterStyle == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.FontName = "Verdana";
                normalRowCenterStyle = hssfworkbook.CreateCellStyle();
                normalRowCenterStyle.SetFont(font);
                normalRowCenterStyle.Alignment = HorizontalAlignment.Center;
                normalRowCenterStyle.BorderTop = BorderStyle.None;
                normalRowCenterStyle.BorderRight = BorderStyle.None;
                normalRowCenterStyle.BorderBottom = BorderStyle.None;
                normalRowCenterStyle.BorderLeft = BorderStyle.None;
            }
            return normalRowCenterStyle;
        }

        
        private ICellStyle normalRowStyleAument;
        private ICellStyle GetNormalRowStyleAumentado(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyleAument == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "Verdana";
                normalRowStyleAument = hssfworkbook.CreateCellStyle();
                normalRowStyleAument.SetFont(font);
              
                normalRowStyleAument.BorderTop = BorderStyle.None;
                normalRowStyleAument.BorderRight = BorderStyle.None;
                normalRowStyleAument.BorderBottom = BorderStyle.None;
                normalRowStyleAument.BorderLeft = BorderStyle.None;
            }
            return normalRowStyleAument;
        }

        private ICellStyle numericRowStyle;
        private ICellStyle GetNumericRowStyle(XSSFWorkbook hssfworkbook)
        {
            if (numericRowStyle == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.FontName = "Verdana";
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
                font.FontName = "Verdana";
                dateRowStyle = hssfworkbook.CreateCellStyle();
                dateRowStyle.SetFont(font);
                dateRowStyle.BorderTop = BorderStyle.None;
                dateRowStyle.BorderRight = BorderStyle.None;
                dateRowStyle.BorderBottom = BorderStyle.None;
                dateRowStyle.BorderLeft = BorderStyle.None;
                dateRowStyle.DataFormat = hssfworkbook.CreateDataFormat().GetFormat("dd/mm/yyyy");
            }
            return dateRowStyle;
        }

        private ICellStyle normalRowStyleNegrita;
        private ICellStyle GetNormalRowStyleNegrita(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyleNegrita == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.IsBold = true;
                font.FontName = "Verdana";
                normalRowStyleNegrita = hssfworkbook.CreateCellStyle();
                normalRowStyleNegrita.SetFont(font);
                //normalRowStyle.LeftBorderColor =
                normalRowStyleNegrita.BorderTop = BorderStyle.None;
                normalRowStyleNegrita.BorderRight = BorderStyle.None;
                normalRowStyleNegrita.BorderBottom = BorderStyle.None;
                normalRowStyleNegrita.BorderLeft = BorderStyle.None;
            }
            return normalRowStyleNegrita;
        }

        private ICellStyle normalRowStyleGris;
        private ICellStyle GetNormalRowStyleGris(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyleGris == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.FontName = "Verdana";
                font.Color = NPOI.HSSF.Util.HSSFColor.Grey40Percent.Index;
                
                normalRowStyleGris = hssfworkbook.CreateCellStyle();
                normalRowStyleGris.SetFont(font);
                normalRowStyleGris.Alignment = HorizontalAlignment.Center;
                //normalRowStyle.LeftBorderColor =
                normalRowStyleGris.BorderTop = BorderStyle.None;
                normalRowStyleGris.BorderRight = BorderStyle.None;
                normalRowStyleGris.BorderBottom = BorderStyle.None;
                normalRowStyleGris.BorderLeft = BorderStyle.None;
            }
            return normalRowStyleGris;
        }

        private ICellStyle normalRowStyleGrisRecepcion;
        private ICellStyle GetNormalRowStyleGrisRecepcion(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyleGris == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "Verdana";
                font.Color = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;

                normalRowStyleGrisRecepcion = hssfworkbook.CreateCellStyle();
                normalRowStyleGrisRecepcion.SetFont(font);
                normalRowStyleGrisRecepcion.Alignment = HorizontalAlignment.Right;
                //normalRowStyle.LeftBorderColor =
                normalRowStyleGrisRecepcion.BorderTop = BorderStyle.None;
                normalRowStyleGrisRecepcion.BorderRight = BorderStyle.None;
                normalRowStyleGrisRecepcion.BorderBottom = BorderStyle.None;
                normalRowStyleGrisRecepcion.BorderLeft = BorderStyle.None;
            }
            return normalRowStyleGrisRecepcion;
        }


        
        private XSSFCellStyle normalRowStyleHeader;
        private XSSFCellStyle GetNormalRowStyleHeader(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyleHeader == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 8;
                font.Color= NPOI.HSSF.Util.HSSFColor.White.Index;
                font.IsBold = true;
                font.FontName = "Verdana";
                //byte[] rgb = new byte[3] { 192, 0, 0 };
                
                byte[] rgb = new byte[3] { 0, 68, 132 };
                //XSSFCellStyle normalRowStyleHeaders = (XSSFCellStyle)hssfworkbook.CreateCellStyle();
                //normalRowStyleHeaders.SetFillForegroundColor(new XSSFColor(System.Drawing.Color.Azure));
                //normalRowStyleHeaders.SetFillBackgroundColor(new XSSFColor(System.Drawing.Color.Black));

                //CellStyle style = workbook.createCellStyle();
                //style.setFillForegroundColor(IndexedColors.GREEN.getIndex());
                //style.setFillPattern(CellStyle.SOLID_FOREGROUND);

                XSSFCellStyle normalRowStyleHeaders = (XSSFCellStyle)hssfworkbook.CreateCellStyle();

                normalRowStyleHeaders.SetFillForegroundColor(new XSSFColor(rgb));
                
                normalRowStyleHeaders.FillPattern=FillPattern.SolidForeground;
                //normalRowStyleHeaders.SetFillBackgroundColor(new XSSFColor(rgb));
                //normalRowStyleHeaders.setFillPattern(XSSFCellStyle.); 
                //normalRowStyleHeaders.setFill;
                
                normalRowStyleHeaders.Alignment = HorizontalAlignment.Center;
                normalRowStyleHeaders.VerticalAlignment = VerticalAlignment.Center;
                normalRowStyleHeaders.SetFont(font);
              
                //XSSFCellStyle HeaderCellStyle1 = (XSSFCellStyle)hssfworkbook.CreateCellStyle();
                //normalRowStyleHeader.SetFillForegroundColor(new XSSFColor(rgb));

         


               
                //normalRowStyleHeader.bac = NPOI.HSSF.Util.HSSFColor.DarkBlue.Index;
                
                //normalRowStyle.LeftBorderColor =

                normalRowStyleHeaders.BorderTop = BorderStyle.None;
                normalRowStyleHeaders.BorderRight = BorderStyle.None;
                normalRowStyleHeaders.BorderBottom = BorderStyle.None;
                normalRowStyleHeaders.BorderLeft = BorderStyle.None;
                normalRowStyleHeader = normalRowStyleHeaders;
            }
            return normalRowStyleHeader;
        }

        private ICellStyle normalRowStyleAzul;
        private ICellStyle GetNormalAumentadoRowStyleAzul(XSSFWorkbook hssfworkbook)
        {
            if (normalRowStyleAzul == null)
            {
                IFont font = hssfworkbook.CreateFont();
                font.FontHeightInPoints = 10;
                font.FontName = "Verdana";
                font.IsBold = true;
                font.Color = NPOI.HSSF.Util.HSSFColor.RoyalBlue.Index;
                //65,105,225
                normalRowStyleAzul = hssfworkbook.CreateCellStyle();
                normalRowStyleAzul.SetFont(font);
                normalRowStyleAzul.Alignment = HorizontalAlignment.Left;
                //normalRowStyle.LeftBorderColor =
                normalRowStyleAzul.BorderTop = BorderStyle.None;
                normalRowStyleAzul.BorderRight = BorderStyle.None;
                normalRowStyleAzul.BorderBottom = BorderStyle.None;
                normalRowStyleAzul.BorderLeft = BorderStyle.None;
            }
            return normalRowStyleAzul;
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

        private void SetCellAumentValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyleAumentado(hssfworkbook);

            cell.SetCellValue(value);
        }

        private void SetCellNegritaValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyleNegrita(hssfworkbook);

            cell.SetCellValue(value);
        }

           private void SetCellHeaderValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyleHeader(hssfworkbook);
               
            cell.SetCellValue(value);
        }

               private void SetCellGrisValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyleGris(hssfworkbook);
               //cell.CellStyle.FillBackgroundColor=""
            cell.SetCellValue(value);
        }

        private void SetCellAzulValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalAumentadoRowStyleAzul(hssfworkbook);
               //cell.CellStyle.FillBackgroundColor=""
            cell.SetCellValue(value);
        }

          private void SetCellGrisMontoValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalRowStyleGrisRecepcion(hssfworkbook);
               //cell.CellStyle.FillBackgroundColor=""
            cell.SetCellValue(value);
        }
        

             private void SetCellCenterValue(XSSFWorkbook hssfworkbook, IRow row, int i, string value)
        {
            ICell cell = row.GetCell(i, MissingCellPolicy.CREATE_NULL_AS_BLANK);
            cell.CellStyle = GetNormalCenterRowStyle(hssfworkbook);
               //cell.CellStyle.FillBackgroundColor=""
            cell.SetCellValue(value);
        }

        
        

        #endregion

     


	}
}