//using BanChenGuar.Model.LoanManagement.Enum;
//using BanChenGuar.Model.WebApi.TurnOverLegal;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace OrderSort.Common
{
    /// <summary>
    /// 导出excel工具类
    /// 创建人：niu
    /// 创建时间:2017.07.10
    /// 修改人：张鹏超
    /// 修改内容：增加不带合计和数据格式化的导出 方法 ExportDataTableToExcel
    /// </summary>
    public class ExcelExportUtils
    {
        /// <summary>
        /// 导出带合计的Excel表格（合计为自动计算，并且为可选）
        /// </summary>
        /// <param name="dt">要导出的数据表</param>
        /// <param name="FileName">导出的文件名(文件名不带后缀)，后缀默认为.xlsx</param>
        /// <param name="hasSum">是否有合计：true:带合计,false:不带合计</param>
        /// <param name="sumColumn">需要计算合计的以英文逗号(,)分隔的列序号，列序号从1开始，如："1,2,3"</param>
        public static void ExportDataTable2ExcelByNPOI(DataTable dt, string FileName, bool hasSum, string sumColumn)
        {
            HttpResponse resp = HttpContext.Current.Response;
            resp.ContentEncoding = System.Text.Encoding.UTF8;

            if (HttpContext.Current.Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") == -1)//不是火狐浏览器，再将文件名称编码
            {
                FileName = System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            }
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            resp.ContentType = "application/ms-excel";//vnd.ms-excel

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            #region 整体默认样式
            //整体默认样式
            sheet.DisplayGridlines = false;//不显示网格线
            //默认宋体11号字
            IFont defaultFont = book.CreateFont();//默认宋体11号字
            defaultFont.FontHeightInPoints = 11;
            defaultFont.FontName = "宋体";
            #endregion
            #region 标题行
            //标题行
            IRow headerRow = sheet.CreateRow(0);
            headerRow.Height = 26 * 20;//标题行行高

            //标题字体样式
            IFont headerFont = book.CreateFont();
            headerFont.Boldweight = short.MaxValue;//粗体
            headerFont.FontHeightInPoints = defaultFont.FontHeightInPoints;
            headerFont.FontName = defaultFont.FontName;
            ICellStyle headerStyle = GetBlackBorder(book);
            headerStyle.WrapText = true;
            headerStyle.SetFont(headerFont);
            //设置每列默认样式，并设置列名
            foreach (DataColumn column in dt.Columns)
            {
                //标题列创建单元单元格，设置单元格样式和单元格的值
                ICell headerCell = headerRow.CreateCell(column.Ordinal);
                headerCell.SetCellValue(column.Caption.Split('|')[0]);
                headerCell.CellStyle = headerStyle;
                //设置列宽
                if (column.Caption.IndexOf("|") > 0)
                {
                    if (column.Caption.Split('|')[1].Length > 1)
                    {
                        sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32(column.Caption.Split('|')[1]) * 256);
                    }
                }
            }
            #endregion
            #region 数据行
            //金额数据格式
            IDataFormat format = book.CreateDataFormat();
            ICellStyle moneyDataStyle = GetBlackBorder(book);
            moneyDataStyle.SetFont(defaultFont);
            moneyDataStyle.DataFormat = format.GetFormat("#,##0.00");

            //金额数据4位小数格式
            IDataFormat Moneyformat = book.CreateDataFormat();
            ICellStyle DataStyleMoney = GetBlackBorder(book);
            DataStyleMoney.SetFont(defaultFont);
            DataStyleMoney.DataFormat = Moneyformat.GetFormat("#,##0.0000");
            //日期数据格式
            IDataFormat formattime = book.CreateDataFormat();
            ICellStyle timeDataStyle = GetBlackBorder(book);
            timeDataStyle.SetFont(defaultFont);
            timeDataStyle.DataFormat = formattime.GetFormat("yyyy-m-d");


            //警告格式
            IFont warnFont = book.CreateFont();//警告字体
            warnFont.FontHeightInPoints = 11;
            warnFont.FontName = "宋体";
            warnFont.Color = HSSFColor.Orange.Index;
            ICellStyle warnStyle = GetBlackBorder(book);
            warnStyle.SetFont(warnFont);
            //正常数据格式
            ICellStyle normalDataStyle = GetBlackBorder(book);
            normalDataStyle.SetFont(defaultFont);
            normalDataStyle.WrapText = true;
            //水平居左
            ICellStyle leftDataStyle = GetBlackBorderLeftCenter(book);
            leftDataStyle.SetFont(defaultFont);
            leftDataStyle.WrapText = true;
            //行索引，标题行为0，第一行数据为1
            int rowIndex = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                dataRow.Height = 26 * 16;//标题行行高
                foreach (DataColumn column in dt.Columns)
                {
                    ICell dataCell = dataRow.CreateCell(column.Ordinal);
                    switch (column.DataType.ToString())
                    {
                        case "System.Double":
                        case "System.Decimal":
                            double doubleValue = 0;
                            Double.TryParse(row[column.ColumnName].ToString(), out doubleValue);
                            dataCell.SetCellValue(doubleValue);
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                                dataCell.CellStyle.DataFormat = format.GetFormat("#,##0.00");
                            }
                            else if (column.ColumnName.IndexOf("抵押率") != -1)
                            {
                                dataCell.CellStyle = DataStyleMoney;
                                dataCell.CellStyle.DataFormat = Moneyformat.GetFormat("#,##0.0000");
                            }
                            else
                            {
                                dataCell.CellStyle = moneyDataStyle;
                            }
                            break;
                        case "System.Int":
                        case "System.Int32":
                            int intValue = 0;
                            int.TryParse(row[column.ColumnName].ToString(), out intValue);
                            dataCell.SetCellValue(intValue);
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                                dataCell.CellStyle.DataFormat = format.GetFormat("#,##0.00");
                            }
                            else
                            {
                                dataCell.CellStyle = normalDataStyle;
                            }
                            break;
                        case "System.DateTime":
                            DateTime dateTimeValue = new DateTime();
                            var result = DateTime.TryParse(row[column.ColumnName].ToString(), out dateTimeValue);
                            if (result)
                                dataCell.SetCellValue(dateTimeValue);
                            else
                                dataCell.SetCellValue("");
                            dataCell.CellStyle = timeDataStyle;
                            dataCell.CellStyle.DataFormat = formattime.GetFormat("yyyy-m-d");
                            break;
                        default:
                            dataCell.SetCellValue(row[column.ColumnName].ToString());
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                            }
                            else if (column.ColumnName.IndexOf("意见") != -1)
                            {
                                dataCell.CellStyle = leftDataStyle;
                            }
                            else
                            {
                                dataCell.CellStyle = normalDataStyle;
                            }
                            break;
                    }
                }

                rowIndex++;
            }
            #endregion
            #region 合计行
            if (hasSum && sumColumn.Length > 0)
            {
                int[] sumColumns = sumColumn.Split(',').Select(x => int.Parse(x) - 1).ToArray<int>();
                IRow footerRow = sheet.CreateRow(rowIndex);
                string c = "";
                foreach (DataColumn column in dt.Columns)
                {
                    ICell footerCell = footerRow.CreateCell(column.Ordinal);
                    if (column.Ordinal == 0)
                    {
                        footerCell.SetCellValue("合计：");
                        footerCell.CellStyle = normalDataStyle;
                    }
                    else
                    {
                        if (Array.IndexOf<int>(sumColumns, column.Ordinal) > -1)
                        {

                            if (column.Ordinal > 25)
                            {
                                c = ((char)(65 + column.Ordinal % 26)).ToString();
                                c = ((char)(65 + column.Ordinal / 26 - 1)).ToString() + c;
                            }
                            else
                            {
                                c = ((char)(65 + column.Ordinal)).ToString();
                            }
                            footerCell.SetCellFormula(string.Format("sum({0}2:{0}{1})", c, (dt.Rows.Count + 1).ToString()));//自动求和;
                            footerCell.CellStyle = moneyDataStyle;
                        }
                        else
                        {
                            footerCell.CellStyle = normalDataStyle;
                        }
                    }
                }
            }
            #endregion

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            resp.BinaryWrite(ms.ToArray());
            resp.End();
            book = null;
            ms.Close();
            ms.Dispose();
        }
        /// <summary>
        /// 保存excel到服务器
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="url"></param>
        /// <param name="hasSum"></param>
        /// <param name="sumColumn"></param>
        public static void SaveDataTable2HSSFWorkbookByNPOI(DataTable dt, string url, bool hasSum, string sumColumn)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            #region 整体默认样式
            //整体默认样式
            sheet.DisplayGridlines = false;//不显示网格线
            //默认宋体11号字
            IFont defaultFont = book.CreateFont();//默认宋体11号字
            defaultFont.FontHeightInPoints = 11;
            defaultFont.FontName = "宋体";
            #endregion
            #region 标题行
            //标题行
            IRow headerRow = sheet.CreateRow(0);
            headerRow.Height = 26 * 20;//标题行行高

            //标题字体样式
            IFont headerFont = book.CreateFont();
            headerFont.Boldweight = short.MaxValue;//粗体
            headerFont.FontHeightInPoints = defaultFont.FontHeightInPoints;
            headerFont.FontName = defaultFont.FontName;
            ICellStyle headerStyle = GetBlackBorder(book);
            headerStyle.WrapText = true;
            headerStyle.SetFont(headerFont);
            //设置每列默认样式，并设置列名
            foreach (DataColumn column in dt.Columns)
            {
                //标题列创建单元单元格，设置单元格样式和单元格的值
                ICell headerCell = headerRow.CreateCell(column.Ordinal);
                headerCell.SetCellValue(column.Caption.Split('|')[0]);
                headerCell.CellStyle = headerStyle;
                //设置列宽
                if (column.Caption.IndexOf("|") > 0)
                {
                    if (column.Caption.Split('|')[1].Length > 1)
                    {
                        sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32(column.Caption.Split('|')[1]) * 256);
                    }
                }
            }
            #endregion
            #region 数据行
            //金额数据格式
            IDataFormat format = book.CreateDataFormat();
            ICellStyle moneyDataStyle = GetBlackBorder(book);
            moneyDataStyle.SetFont(defaultFont);
            moneyDataStyle.DataFormat = format.GetFormat("#,##0.00");

            //金额数据4位小数格式
            IDataFormat Moneyformat = book.CreateDataFormat();
            ICellStyle DataStyleMoney = GetBlackBorder(book);
            DataStyleMoney.SetFont(defaultFont);
            DataStyleMoney.DataFormat = Moneyformat.GetFormat("#,##0.0000");
            //日期数据格式
            IDataFormat formattime = book.CreateDataFormat();
            ICellStyle timeDataStyle = GetBlackBorder(book);
            timeDataStyle.SetFont(defaultFont);
            timeDataStyle.DataFormat = formattime.GetFormat("yyyy-m-d");


            //警告格式
            IFont warnFont = book.CreateFont();//警告字体
            warnFont.FontHeightInPoints = 11;
            warnFont.FontName = "宋体";
            warnFont.Color = HSSFColor.Orange.Index;
            ICellStyle warnStyle = GetBlackBorder(book);
            warnStyle.SetFont(warnFont);
            //正常数据格式
            ICellStyle normalDataStyle = GetBlackBorder(book);
            normalDataStyle.SetFont(defaultFont);
            normalDataStyle.WrapText = true;
            //水平居左
            ICellStyle leftDataStyle = GetBlackBorderLeftCenter(book);
            leftDataStyle.SetFont(defaultFont);
            leftDataStyle.WrapText = true;
            //行索引，标题行为0，第一行数据为1
            int rowIndex = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                dataRow.Height = 26 * 16;//标题行行高
                foreach (DataColumn column in dt.Columns)
                {
                    ICell dataCell = dataRow.CreateCell(column.Ordinal);
                    switch (column.DataType.ToString())
                    {
                        case "System.Double":
                        case "System.Decimal":
                            double doubleValue = 0;
                            Double.TryParse(row[column.ColumnName].ToString(), out doubleValue);
                            dataCell.SetCellValue(doubleValue);
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                                dataCell.CellStyle.DataFormat = format.GetFormat("#,##0.00");
                            }
                            else if (column.ColumnName.IndexOf("抵押率") != -1)
                            {
                                dataCell.CellStyle = DataStyleMoney;
                                dataCell.CellStyle.DataFormat = Moneyformat.GetFormat("#,##0.0000");
                            }
                            else
                            {
                                dataCell.CellStyle = moneyDataStyle;
                            }
                            break;
                        case "System.Int":
                        case "System.Int32":
                            int intValue = 0;
                            int.TryParse(row[column.ColumnName].ToString(), out intValue);
                            dataCell.SetCellValue(intValue);
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                                dataCell.CellStyle.DataFormat = format.GetFormat("#,##0.00");
                            }
                            else
                            {
                                dataCell.CellStyle = normalDataStyle;
                            }
                            break;
                        case "System.DateTime":
                            DateTime dateTimeValue = new DateTime();
                            var result = DateTime.TryParse(row[column.ColumnName].ToString(), out dateTimeValue);
                            if (result)
                                dataCell.SetCellValue(dateTimeValue);
                            else
                                dataCell.SetCellValue("");
                            dataCell.CellStyle = timeDataStyle;
                            dataCell.CellStyle.DataFormat = formattime.GetFormat("yyyy-m-d");
                            break;
                        default:
                            dataCell.SetCellValue(row[column.ColumnName].ToString());
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                            }
                            else if (column.ColumnName.IndexOf("意见") != -1)
                            {
                                dataCell.CellStyle = leftDataStyle;
                            }
                            else
                            {
                                dataCell.CellStyle = normalDataStyle;
                            }
                            break;
                    }
                }

                rowIndex++;
            }
            #endregion
            #region 合计行
            if (hasSum && sumColumn.Length > 0)
            {
                int[] sumColumns = sumColumn.Split(',').Select(x => int.Parse(x) - 1).ToArray<int>();
                IRow footerRow = sheet.CreateRow(rowIndex);
                string c = "";
                foreach (DataColumn column in dt.Columns)
                {
                    ICell footerCell = footerRow.CreateCell(column.Ordinal);
                    if (column.Ordinal == 0)
                    {
                        footerCell.SetCellValue("合计：");
                        footerCell.CellStyle = normalDataStyle;
                    }
                    else
                    {
                        if (Array.IndexOf<int>(sumColumns, column.Ordinal) > -1)
                        {

                            if (column.Ordinal > 25)
                            {
                                c = ((char)(65 + column.Ordinal % 26)).ToString();
                                c = ((char)(65 + column.Ordinal / 26 - 1)).ToString() + c;
                            }
                            else
                            {
                                c = ((char)(65 + column.Ordinal)).ToString();
                            }
                            footerCell.SetCellFormula(string.Format("sum({0}2:{0}{1})", c, (dt.Rows.Count + 1).ToString()));//自动求和;
                            footerCell.CellStyle = moneyDataStyle;
                        }
                        else
                        {
                            footerCell.CellStyle = normalDataStyle;
                        }
                    }
                }
            }
            #endregion

            using (FileStream file = new FileStream(url, FileMode.Create)) {
                book.Write(file);
                file.Close();
            }
        }

        /// <summary>
        /// 导出带合计的Excel表格（合计为传过来的字符串，并且为可选）
        /// </summary>
        /// <param name="dt">要导出的数据表</param>
        /// <param name="FileName">导出的文件名(文件名不带后缀)，后缀默认为.xlsx</param>
        /// <param name="hasSum">是否有合计：true:带合计,false:不带合计</param>
        /// <param name="heStr">要在最现在合计列展示的内容</param>
        public static void ExportDataTable3ExcelByNPOI(DataTable dt, string FileName, bool hasSum, string heStr)
        {
            HttpResponse resp = HttpContext.Current.Response;
            resp.ContentEncoding = System.Text.Encoding.UTF8;
            if (HttpContext.Current.Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") == -1)//不是火狐浏览器，再将文件名称编码
            {
                FileName = System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
            }
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            resp.ContentType = "application/ms-excel";//vnd.ms-excel

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            #region 整体默认样式
            //整体默认样式
            sheet.DisplayGridlines = false;//不显示网格线
            //默认宋体11号字
            IFont defaultFont = book.CreateFont();//默认宋体11号字
            defaultFont.FontHeightInPoints = 11;
            defaultFont.FontName = "宋体";
            #endregion
            #region 标题行
            //标题行
            IRow headerRow = sheet.CreateRow(0);
            headerRow.Height = 26 * 20;//标题行行高

            //标题字体样式
            IFont headerFont = book.CreateFont();
            headerFont.Boldweight = short.MaxValue;//粗体
            headerFont.FontHeightInPoints = defaultFont.FontHeightInPoints;
            headerFont.FontName = defaultFont.FontName;
            ICellStyle headerStyle = GetBlackBorder(book);
            headerStyle.WrapText = true;
            headerStyle.SetFont(headerFont);
            //设置每列默认样式，并设置列名
            foreach (DataColumn column in dt.Columns)
            {
                //标题列创建单元单元格，设置单元格样式和单元格的值
                ICell headerCell = headerRow.CreateCell(column.Ordinal);
                headerCell.SetCellValue(column.Caption.Split('|')[0]);
                headerCell.CellStyle = headerStyle;
                //设置列宽
                if (column.Caption.IndexOf("|") > 0)
                {
                    if (column.Caption.Split('|')[1].Length > 1)
                    {
                        sheet.SetColumnWidth(column.Ordinal, Convert.ToInt32(column.Caption.Split('|')[1]) * 256);
                    }
                }
            }
            #endregion
            #region 数据行
            //金额数据格式
            IDataFormat format = book.CreateDataFormat();
            ICellStyle moneyDataStyle = GetBlackBorder(book);
            moneyDataStyle.SetFont(defaultFont);
            moneyDataStyle.DataFormat = format.GetFormat("#,##0.00");


            //警告格式
            IFont warnFont = book.CreateFont();//警告字体
            warnFont.FontHeightInPoints = 11;
            warnFont.FontName = "宋体";
            warnFont.Color = HSSFColor.Orange.Index;
            ICellStyle warnStyle = GetBlackBorder(book);
            warnStyle.SetFont(warnFont);
            //正常数据格式
            ICellStyle normalDataStyle = GetBlackBorder(book);
            normalDataStyle.SetFont(defaultFont);
            normalDataStyle.WrapText = true;
            //水平居左
            ICellStyle leftDataStyle = GetBlackBorderLeftCenter(book);
            leftDataStyle.SetFont(defaultFont);
            leftDataStyle.WrapText = true;
            //行索引，标题行为0，第一行数据为1
            int rowIndex = 1;
            foreach (DataRow row in dt.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                dataRow.Height = 26 * 16;//标题行行高
                foreach (DataColumn column in dt.Columns)
                {
                    ICell dataCell = dataRow.CreateCell(column.Ordinal);
                    switch (column.DataType.ToString())
                    {
                        case "System.Double":
                        case "System.Decimal":
                            double doubleValue = 0;
                            Double.TryParse(row[column.ColumnName].ToString(), out doubleValue);
                            dataCell.SetCellValue(doubleValue);
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                                dataCell.CellStyle.DataFormat = format.GetFormat("#,##0.00");
                            }
                            else
                            {
                                dataCell.CellStyle = moneyDataStyle;
                            }
                            break;
                        case "System.Int":
                        case "System.Int32":
                            int intValue = 0;
                            int.TryParse(row[column.ColumnName].ToString(), out intValue);
                            dataCell.SetCellValue(intValue);
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                                dataCell.CellStyle.DataFormat = format.GetFormat("#,##0.00");
                            }
                            else
                            {
                                dataCell.CellStyle = normalDataStyle;
                            }
                            break;
                        default:
                            dataCell.SetCellValue(row[column.ColumnName].ToString());
                            if (column.ColumnName.IndexOf("退费") != -1)
                            {
                                dataCell.CellStyle = warnStyle;
                            }
                            else if (column.ColumnName.IndexOf("意见") != -1)
                            {
                                dataCell.CellStyle = leftDataStyle;
                            }
                            else
                            {
                                dataCell.CellStyle = normalDataStyle;
                            }
                            break;
                    }
                }

                rowIndex++;
            }
            #endregion
            #region 合计行
            if (hasSum && heStr.Length > 0)
            {
                IRow footerRow = sheet.CreateRow(rowIndex);
                footerRow.Height = 26 * 22;//标题行行高
                foreach (DataColumn column in dt.Columns)
                {
                    ICell footerCell = footerRow.CreateCell(column.Ordinal);
                    footerCell.CellStyle = normalDataStyle;
                }
                Merge(sheet, rowIndex, rowIndex, 0, dt.Columns.Count - 1);
                footerRow.Cells[0].SetCellValue(heStr);
                //footerRow.Cells[0].CellStyle = normalDataStyle;
                //ICell footerCell = footerRow.add
                //footerCell.SetCellValue("合计：");
                //footerCell.CellStyle = normalDataStyle;
            }
            #endregion

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            resp.BinaryWrite(ms.ToArray());
            resp.End();
            book = null;
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// DataTable数据导出EXCEL 不带数据格式化样式
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="FileName">要导出的excel文件名</param>
        public static void ExportDataTableToExcel(DataTable dt, string FileName)
        {
            HttpResponse resp = HttpContext.Current.Response;
            resp.ContentEncoding = System.Text.Encoding.UTF8;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (HttpContext.Current.Request.ServerVariables["http_user_agent"].ToString().IndexOf("Firefox") == -1)//不是火狐浏览器，再将文件名称编码
                {
                    FileName = System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8);
                }
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
                resp.ContentType = "application/ms-excel";//vnd.ms-excel

                HSSFWorkbook book = new HSSFWorkbook();
                ISheet firstSheet = book.CreateSheet("Sheet1");
                //整体默认显示网格线
                firstSheet.DisplayGridlines = true;
                #region 标题行样式
                //标题行
                IRow headerRow = firstSheet.CreateRow(0);
                headerRow.Height = 26 * 20;//标题行行高

                //标题字体样式
                IFont headerFont = book.CreateFont();
                headerFont.Boldweight = short.MaxValue;//粗体
                headerFont.FontHeightInPoints = headerFont.FontHeightInPoints;
                headerFont.FontName = headerFont.FontName;
                ICellStyle headerStyle = GetBlackBorder(book);
                headerStyle.WrapText = true;
                headerStyle.SetFont(headerFont);
                #endregion

                #region 文本默认样式
                //正常数据格式
                ICellStyle normalDataStyle = GetBlackBorder(book);
                //默认宋体11号字
                IFont defaultFont = book.CreateFont();
                defaultFont.FontHeightInPoints = 11;
                defaultFont.FontName = "宋体";
                normalDataStyle.SetFont(defaultFont);
                normalDataStyle.WrapText = true;
                #endregion
                //设置首行列头显示
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell headerCell = headerRow.CreateCell(i);
                    headerCell.CellStyle = headerStyle;
                    headerCell.SetCellValue(dt.Columns[i].ColumnName?.ToString());
                }
                //设置数据行格式
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow dataRow = firstSheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell dataCell = dataRow.CreateCell(j);
                        dataCell.SetCellValue(dt.Rows[i][j]?.ToString());
                        dataCell.CellStyle = normalDataStyle;
                        //firstSheet.AutoSizeColumn(j); 目前增加这个有性能问题暂去掉
                    }
                }
                MemoryStream ms = new MemoryStream();
                book.Write(ms);
                resp.BinaryWrite(ms.ToArray());
                resp.End();
                book = null;
                ms.Close();
                ms.Dispose();
            }
        }
        /// <summary>
        /// 黑色边框居中
        /// </summary>
        /// <param name="book">Workbook实例对象</param>
        /// <returns></returns>
        public static ICellStyle GetBlackBorder(HSSFWorkbook book)
        {
            ICellStyle style = book.CreateCellStyle();
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.LeftBorderColor = HSSFColor.Black.Index;
            style.RightBorderColor = HSSFColor.Black.Index;
            style.TopBorderColor = HSSFColor.Black.Index;
            style.BottomBorderColor = HSSFColor.Black.Index;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            return style;
        }

        public static ICellStyle GetBlackBorderCenterCenter(HSSFWorkbook book)
        {
            ICellStyle style = book.CreateCellStyle();
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.LeftBorderColor = HSSFColor.Black.Index;
            style.RightBorderColor = HSSFColor.Black.Index;
            style.TopBorderColor = HSSFColor.Black.Index;
            style.BottomBorderColor = HSSFColor.Black.Index;
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            return style;
        }
        /// <summary>
        /// 黑色边框水平居左垂直居中
        /// </summary>
        /// <param name="book">Workbook实例对象</param>
        /// <returns></returns>
        public static ICellStyle GetBlackBorderLeftCenter(HSSFWorkbook book)
        {
            ICellStyle style = book.CreateCellStyle();
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.LeftBorderColor = HSSFColor.Black.Index;
            style.RightBorderColor = HSSFColor.Black.Index;
            style.TopBorderColor = HSSFColor.Black.Index;
            style.BottomBorderColor = HSSFColor.Black.Index;
            style.Alignment = HorizontalAlignment.Left;
            style.VerticalAlignment = VerticalAlignment.Center;
            return style;
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="startRow">开始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="startColumn">开始列</param>
        /// <param name="endColumn">结束列</param>
        public static void Merge(ISheet sheet, Int32 startRow, Int32 endRow, Int32 startColumn, Int32 endColumn)
        {
            //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
            sheet.AddMergedRegion(new CellRangeAddress(startRow, endRow, startColumn, endColumn));
        }
        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sheet">表</param>
        /// <param name="sourceRowNum">源行号</param>
        /// <param name="destRowNum">目标行号</param>
        public static void InsertRow(ISheet sheet, Int32 sourceRowNum, Int32 destRowNum)
        {
            IRow sourceRow = sheet.GetRow(sourceRowNum);
            IRow targetRow = sheet.GetRow(destRowNum);
            if (targetRow != null)
            {
                sheet.ShiftRows(destRowNum, sheet.LastRowNum, 1, true, false);
            }
            else
            {
                targetRow = sheet.CreateRow(destRowNum);
            }

            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {

                ICell sourceCell = sourceRow.GetCell(i);
                ICell targetCell = null;
                if (sourceCell == null)
                {
                    continue;
                }
                targetCell = targetRow.CreateCell(i);
                targetCell.CellStyle = sourceCell.CellStyle;
                targetCell.SetCellType(sourceCell.CellType);
            }
        }
        /// <summary>
        /// 获取指定单元格格式具有黑色边框垂直居中左右居中的CellStyle
        /// </summary>
        /// <param name="book">HSSFWorkbook对象</param>
        /// <param name="formatName">格式字符串</param>
        /// <returns></returns>
        public static ICellStyle GetCellStyleByFormat(HSSFWorkbook book, string formatName)
        {
            ICellStyle cellStyle = GetBlackBorder(book);
            IDataFormat format = book.CreateDataFormat();
            cellStyle.DataFormat = format.GetFormat(formatName);
            return cellStyle;
        }
       
        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                   IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将Excel以文件流转换DataTable
        /// </summary>
        /// <param name="hasTitle">是否有表头</param>
        /// <param name="path">文件路径</param>
        /// <param name="tableindex">文件簿索引</param>
        public static DataTable ExcelToDataTableFormPath(bool hasTitle = true, string path = "", int tableindex = 0)
        {
            //新建Workbook
            Workbook workbook = new Workbook();
            //将当前路径下的文件内容读取到workbook对象里面
            workbook.LoadFromFile(path);
            //得到第一个Sheet页
            Worksheet sheet = workbook.Worksheets[tableindex];
            return SheetToDataTable(hasTitle, sheet);
        }
        /// <summary>
        /// 将Excel以文件流转换DataTable
        /// </summary>
        /// <param name="hasTitle">是否有表头</param>
        /// <param name="stream">文件流</param>
        /// <param name="tableindex">文件簿索引</param>
        public static DataTable ExcelToDataTableFormStream(bool hasTitle = true, Stream stream = null, int tableindex = 0)
        {
            //新建Workbook
            Workbook workbook = new Workbook();
            //将文件流内容读取到workbook对象里面
            workbook.LoadFromStream(stream);
            //得到第一个Sheet页
            Worksheet sheet = workbook.Worksheets[tableindex];
            return SheetToDataTable(hasTitle, sheet);
        }

        private static DataTable SheetToDataTable(bool hasTitle, Worksheet sheet)
        {
            int iRowCount = sheet.Rows.Length;
            int iColCount = sheet.Columns.Length;
            DataTable dt = new DataTable();
            //生成列头
            for (int i = 0; i < iColCount; i++)
            {
                var name = "column" + i;
                if (hasTitle)
                {
                    var txt = sheet.Range[1, i + 1].Text;
                    if (!string.IsNullOrEmpty(txt)) name = txt;
                }
                while (dt.Columns.Contains(name)) name = name + "_1";//重复行名称会报错。
                dt.Columns.Add(new DataColumn(name, typeof(string)));
            }
            //生成行数据
            int rowIdx = hasTitle ? 2 : 1;
            for (int iRow = rowIdx; iRow <= iRowCount; iRow++)
            {
                DataRow dr = dt.NewRow();
                for (int iCol = 1; iCol <= iColCount; iCol++)
                {
                    dr[iCol - 1] = sheet.Range[iRow, iCol].Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


    }
}
